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
    [Route("api/status")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly GameManager games;

        public StatusController(ILogger<GamesController> logger, GameManager gameManager)
        {
            games = gameManager;
        }

        /// <summary>
        /// Get status of server.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(StatusDto), StatusCodes.Status200OK)]
        public ActionResult<List<GameRoom>> GetStatus()
        {
            return this.Ok(new StatusDto()
            {
                Lobbies = games.Matchmaker.AllLobbies.Count,
                Games = games.Matchmaker.AllGames.Count
            });
        }
    }
}
