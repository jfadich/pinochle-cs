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
    public class MatchmakingHub : Hub<IMatchMakingClient>
    {
        [Authorize(Roles = "Administrator,Coordinator")]
        public Task WatchMatchmaking()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, Audiences.Matchmaking);
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        public Task WatchGames()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, Audiences.AllGames);
        }

        [Authorize(Roles = "Administrator,Coordinator,Observer")]
        public Task SubscribeToRoom(string room)
        {
            if (room != null)
            {
                return Groups.AddToGroupAsync(Context.ConnectionId, room);
            }

            return null;
        }
    }
}
