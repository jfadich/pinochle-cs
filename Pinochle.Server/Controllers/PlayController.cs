using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JFadich.Pinochle.Engine;
using Microsoft.AspNetCore.Authorization;
using JFadich.Pinochle.Server.Requests;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Server.Controllers
{
    [ApiController]
    [Route("api/play")]
    [Authorize(Roles = "Player")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class PlayController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;

        private readonly GameManager _rooms;

        public PlayController(ILogger<GamesController> logger, [FromServices] GameManager rooms)
        {
            _rooms = rooms;
            _logger = logger;
        }

        [HttpPost("start")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult Start()
        {
            (Room room, Seat seat) = GetRoomAndSeat();

            room.StartGame(seat.Position);

            return Accepted();
        }

        [HttpGet("hand")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Engine.Cards.PinochleCard[]))]
        public IActionResult GetHand()
        {
            (Room room, Seat seat) = GetRoomAndSeat();

            return Ok(room.Game.GetPlayerHand(seat)?.Cards);
        }

        [HttpPost("bid")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult PlaceBid([FromBody] BidRequest request)
        {
            (Room room, Seat seat) = GetRoomAndSeat();
            
            room.TakeAction(new PlaceBid(seat, request.Bid));

            return Accepted();
        }

        [HttpPost("call_trump")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult CallTrump([FromBody] CallTrumpRequest request)
        {
            (Room room, Seat seat) = GetRoomAndSeat();

            room.TakeAction(new CallTrump(seat, request.Trump));

            return Accepted();
        }

        [HttpPost("pass_cards")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult PassCards([FromBody] PassCardsRequest request)
        {
            (Room room, Seat seat) = GetRoomAndSeat();

            room.TakeAction(new PassCards(seat, request.Cards));

            return Accepted();
        }

        [HttpPost("play_trick")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult PlayTrick([FromBody] PlayTrickRequest request)
        {
            (Room room, Seat seat) = GetRoomAndSeat();

            room.TakeAction(new PlayTrick(seat, request.Trick));

            return Accepted();
        }

        private (Room, Seat) GetRoomAndSeat()
        {
            string roomId = User.FindFirst("room")?.Value;

            if (string.IsNullOrEmpty(roomId))
            {
                throw new Exception("Invalid room id.");
            }

            var room = _rooms.GetRoom(roomId);

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            if (!Int32.TryParse(User.FindFirst("position")?.Value, out int position))
            {
                throw new Exception("Invalid position");
            }

            return (room, Seat.ForPosition(position));
        }
    }
}
