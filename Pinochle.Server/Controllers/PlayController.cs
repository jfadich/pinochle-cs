using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JFadich.Pinochle.Engine;
using Microsoft.AspNetCore.Authorization;
using JFadich.Pinochle.Server.Requests;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Server.Models;

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

        [HttpGet("hand")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int[]))]
        public IActionResult GetHand()
        {
            (GameRoom room, string playerId) = GetRoomAndPlayerId();

            return Ok(room.GetPlayerHand(playerId)?.Cards);
        }

        [HttpPost("bid")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult PlaceBid([FromBody] BidRequest request)
        {
            (GameRoom room, string playerId) = GetRoomAndPlayerId();

            room.PlaceBid(playerId, request.Bid);

            return Accepted();
        }

        [HttpPost("call_trump")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult CallTrump([FromBody] CallTrumpRequest request)
        {
            (GameRoom room, string playerId) = GetRoomAndPlayerId();

            room.CallTrump(playerId, request.Trump);

            return Accepted();
        }

        [HttpPost("pass_cards")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult PassCards([FromBody] PassCardsRequest request)
        {
            (GameRoom room, string playerId) = GetRoomAndPlayerId();

            room.PassCards(playerId, request.Cards);

            return Accepted();
        }

        [HttpPost("play_trick")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult PlayTrick([FromBody] PlayTrickRequest request)
        {
            (GameRoom room, string playerId) = GetRoomAndPlayerId();

            room.PlayTrick(playerId, request.Trick);

            return Accepted();
        }

        private (GameRoom, string) GetRoomAndPlayerId()
        {
            string playerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(playerId))
            {
                throw new Exception("Invalid player id.");
            }

            GameRoom room = _rooms.GetPlayersRoom(playerId);

            if (room == null)
            {
                throw new Exception("Room not found");
            }

            return (room, playerId);
        }
    }
}
