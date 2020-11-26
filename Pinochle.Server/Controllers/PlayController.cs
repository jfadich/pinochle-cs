using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Server.Models;
using Microsoft.AspNetCore.Authorization;
using JFadich.Pinochle.Server.Requests;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Start()
        {
            string roomId = User.FindFirst("room")?.Value;

            if (roomId == null)
            {
                return BadRequest("No Game Found");
            }

            var room = _rooms.GetRoom(roomId);

            if (room == null)
            {
                return NotFound();
            }

            if(!Int32.TryParse(User.FindFirst("position")?.Value, out int position)) 
            {
                position = 0;
            }

            room.StartGame(position);

            return NoContent();
        }

        [HttpPost("bid")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PlaceBid([FromBody] PlaceBid request)
        {
            string roomId = User.FindFirst("room")?.Value;
            
            if (string.IsNullOrEmpty(roomId))
            {
                return BadRequest("No Game Found");
            }

            var room = _rooms.GetRoom(roomId);

            if (room == null)
            {
                return NotFound();
            }

            if (!Int32.TryParse(User.FindFirst("position")?.Value, out int position))
            {
                return BadRequest("invalid position");
            }

            room.TakeAction(new Engine.Actions.PlaceBid(Seat.ForPosition(position), request.Bid));

            return Ok();
        }
    }
}
