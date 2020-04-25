using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pinochle;
using PinochleServer.Models;
using Microsoft.AspNetCore.Authorization;
using PinochleServer.Requests;
using System.Security.Claims;

namespace PinochleServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LobbiesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;

        public LobbiesController(ILogger<GamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Room> Index([FromServices] GameManager games)
        {
            return games.PublicLobbies;
        }

        [Authorize(Roles = "Administrator,Player")]
        [HttpGet("all")]
        public List<Room> All([FromServices] GameManager games)
        {
            return games.AllLobbies;
        }

        [Authorize(Roles = "Player,Coordinator")]
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

            return Ok();
        }

        public IActionResult StartGame()
        {
            return Ok();
        }
    }
}
