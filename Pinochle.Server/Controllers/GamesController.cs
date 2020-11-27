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

namespace JFadich.Pinochle.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly GameManager games;

        public GamesController(ILogger<GamesController> logger, GameManager gameManager)
        {
            games = gameManager;
        }

    /*    [Authorize(Roles = "Administrator")]
        [HttpGet]
        public List<Room> Index([FromServices] )
        {
            return games.PublicGames;
        }
        */
        /// <summary>
        /// Get all games.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<GameRoom>> All()
        {
            return this.Ok(games.AllGames);
        }

        /// <summary>
        /// Get a room by id.
        /// </summary>
        /// <param name="id">Room Id</param>
        /// <response code="404">The specified game does not exist.</response>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,Coordinator,Player,Observer")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult<GameRoom> Get(string id)
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
