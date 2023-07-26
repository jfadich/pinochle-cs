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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Pinochle.Server.DataTransferObjects;

namespace JFadich.Pinochle.Server.Controllers
{
    /// <summary>
    /// Create or find rooms for players.
    /// </summary>
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("api/games/matchmaking")]
    public class MatchmakingController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;

        private readonly GameManager _gameManager;

        public MatchmakingController(ILogger<GamesController> logger, GameManager games)
        {
            _logger = logger;
            _gameManager = games;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameRoomDto>))]
        public ActionResult<List<GameRoom>> List()
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Coordinator"))
            {
                return this.Ok(_gameManager.Matchmaker.AllLobbies);
            }

            return this.Ok(_gameManager.Matchmaker.PublicLobbies);
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameRoomDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<GameRoom> Matchmaking([FromBody] MatchmakingRequest join)
        {
            var lobby = _gameManager.Matchmaker.FindLobbyForPlayer(join.PlayerId);

            if (lobby == null)
            {
                return BadRequest();
            }

            return Ok(lobby);
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var lobby = _gameManager.Matchmaker.AllLobbies.Where(room => room.Id == id);

            if (lobby == null)
            {
                return NotFound();
            }

            return Ok(lobby);
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost("{id}/join")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameRoomDto))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GameRoom> AddToRoom(string id, [FromBody] JoinRequest join)
        {
            if(!_gameManager.Matchmaker.HasLobby(id))
            {
                return NotFound();
            }
            //            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // @todo if player has game, return bad request

            if(!_gameManager.Matchmaker.AddPlayerToRoom(id, join.PlayerId, join.SeatPosition)) {
                return BadRequest();
            }

            return Ok(_gameManager.Matchmaker.GetRoom(id));
        }
    }
}
