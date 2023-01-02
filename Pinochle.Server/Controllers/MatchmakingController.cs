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

namespace JFadich.Pinochle.Server.Controllers
{
    /// <summary>
    /// Create or find rooms for players.
    /// </summary>
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("games/[controller]")]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<GameRoom>> Index()
        {
            return this.Ok(_gameManager.PublicLobbies);
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<GameRoom> Matchmaking([FromBody] MatchmakingRequest join)
        {
            var lobby = _gameManager.FindLobbyForPlayer(join.PlayerId);

            if (lobby == null)
            {
                return BadRequest();
            }

            return Ok(lobby);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<GameRoom>> All()
        {
            return this.Ok(_gameManager.AllLobbies);
        }

        [Authorize(Roles = "Administrator,Player,Coordinator")]
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if(!User.IsInRole("Coordinator") && User.FindFirst("room")?.Value != id) {
                return Forbid();
            }

            var lobby = _gameManager.AllLobbies.Where(room => room.Id == id);

            if (lobby == null)
            {
                return NotFound();
            }

            return Ok(lobby);
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost("{id}/join")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<GameRoom> AddToRoom(string id, [FromBody] JoinRequest join)
        {
            if(!_gameManager.HasLobby(id))
            {
                return NotFound();
            }
            //            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!_gameManager.AddPlayerToRoom(id, join.PlayerId, join.SeatPosition)) {
                return BadRequest();
            }

            return Ok(_gameManager.GetRoom(id));
        }
    }
}
