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

namespace JFadich.Pinochle.Server.Controllers
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
            return games.PublicGames;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        public List<Room> All([FromServices] GameManager games)
        {
            return games.AllGames;
        }

        [Authorize(Roles = "Administrator,Coordinator,Player,Observer")]
        [HttpGet("{id}")]
        public IActionResult Get(string id, [FromServices] GameManager games)
        {
            if ((User.IsInRole("Player") || User.IsInRole("Observer")) && User.FindFirst("room")?.Value != id)
            {
                return Forbid();
            }

            var game = games.GetRoom(id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }
    }
}
