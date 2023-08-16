using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using JFadich.Pinochle.Engine;
using Pinochle.Engine.Contracts;

namespace JFadich.Pinochle.Server.RealTime
{
    [Authorize]
    public class GameHub : Hub<IGameClient>
    {
        IMatchmaker Matchmaker;

        public GameHub(IMatchmaker matchmaker)
        {
            Matchmaker = matchmaker;
        }

        [Authorize(Roles = "Player")]
        public Task JoinTable()
        {
            var user = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            GameRoom room = Matchmaker.GetPlayersRoom(user);

            if (room != null)
            {
                var player = room.GetPlayer(user);

                if(player != null)
                {
                    Seat seat = player.Seat;

                    return Task.WhenAll(
                        Groups.AddToGroupAsync(Context.ConnectionId, room.Id),
                        Groups.AddToGroupAsync(Context.ConnectionId, $"{room.Id}:position:{seat.Position}"),
                        Groups.AddToGroupAsync(Context.ConnectionId, $"{room.Id}:team:{seat.TeamId}")
                    );
                } 
                else
                {
                    return Groups.AddToGroupAsync(Context.ConnectionId, room.Id);
                }
            }

            return null;
        }
    }
}
