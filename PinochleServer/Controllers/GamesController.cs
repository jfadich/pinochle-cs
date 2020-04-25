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

namespace PinochleServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;

        public GamesController(ILogger<GamesController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public List<Room> Index([FromServices] GameManager games)
        {
            return games.PublicRooms;
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        [HttpPost]
        public IActionResult Matchmaking([FromBody] JoinRequest join, [FromServices] GameManager games)
        {
            //var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var room = games.FindRoomForPlayer(join.PlayerId);

            if (room == null)
            {
                return BadRequest();
            }

            return Ok(room.Id);
        }

        [Authorize(Roles = "Player,Coordinator")]
        [HttpGet("{id}")]
        public IActionResult Get(string id, [FromServices] GameManager games)
        {
            var game = games.GetRoom(id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }
    }
}
