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

namespace JFadich.Pinochle.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("games/[controller]")]
    public class MatchmakingController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;

        public MatchmakingController(ILogger<GamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Lobby> Index([FromServices] GameManager games)
        {
            return games.PublicLobbies;
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        [HttpPost]
        public IActionResult Matchmaking([FromBody] JoinRequest join, [FromServices] GameManager games)
        {
            var lobby = games.FindLobbyForPlayer(join.PlayerId);

            if (lobby == null)
            {
                return BadRequest();
            }

            return Ok(lobby);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        public List<Lobby> All([FromServices] GameManager games)
        {
            return games.AllLobbies;
        }

        [Authorize(Roles = "Administrator,Player,Coordinator")]
        [HttpGet("{id}")]
        public IActionResult Get(string id, [FromServices] GameManager games)
        {
            if(!User.IsInRole("Coordinator") && User.FindFirst("room")?.Value != id) {
                return Forbid();
            }

            var lobby = games.AllLobbies.Where(room => room.Id == id);

            if (lobby == null)
            {
                return NotFound();
            }

            return Ok(lobby);
        }

        [Authorize(Roles = "Coordinator")]
        [HttpPost("{id}/join")]
        public IActionResult AddToRoom(string id, [FromBody] JoinRequest join, [FromServices] GameManager games)
        {
            if(!games.HasLobby(id))
            {
                return NotFound();
            }
            //            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(!games.AddPlayerToRoom(id, join.PlayerId, join.SeatPosition)) {
                return BadRequest();
            }

            return Ok(games.GetRoom(id).ToLobby());
        }
    }
}
