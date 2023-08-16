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
using Pinochle.Server.DataTransferObjects;

namespace JFadich.Pinochle.Server.Controllers
{
    [ApiController]
    [Route("api/games")]
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
        /// Get a list of games.
        /// </summary>
        /// <remarks>
        /// If the user is an administrator or coordinator, then return all games. Otherwise return public games
        /// </remarks>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet()]
        [ProducesResponseType(typeof(List<GameRoomDto>), StatusCodes.Status200OK)]
        public ActionResult<List<GameRoom>> List()
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Coordinator"))
            {
                return this.Ok(games.Matchmaker.AllGames);
            }

            return this.Ok(games.Matchmaker.PublicGames);
        }

        /// <summary>
        /// Get a room by id.
        /// </summary>
        /// <param name="id">Room Id</param>
        /// <response code="404">The specified game does not exist.</response>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,Coordinator,Player,Observer")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GameRoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult<GameRoom> Get(string id)
        {
            if ((User.IsInRole("Player") || User.IsInRole("Observer")) && User.FindFirst("room")?.Value != id)
            {
                return Forbid();
            }

            var game = games.Matchmaker.GetRoom(id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }
    }
}
