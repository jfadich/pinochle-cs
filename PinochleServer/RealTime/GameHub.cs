using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using JFadich.Pinochle.Engine;

namespace JFadich.Pinochle.Server.RealTime
{
    [Authorize]
    public class GameHub : Hub<IGameClient>
    {
        public override Task OnConnectedAsync()
        {
            var user = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return base.OnConnectedAsync();
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        public Task SubscribeToMatchmaking()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, Audiences.Matchmaking);
        }

        [Authorize(Roles = "Administrator,Coordinator")]
        public Task SubscribeToAllGames()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, Audiences.AllGames);
        }

        [Authorize(Roles = "Player")]
        public Task SubscribeToMyRoom()
        {
            var room = Context.User.FindFirstValue("room");

            if (room != null)
            {
                return Groups.AddToGroupAsync(Context.ConnectionId, room);
            }

            return null;
        }
        [Authorize(Roles = "Administrator,Coordinator")]
        public Task SubscribeToRoom(string room)
        {
            if (room != null)
            {
                return Groups.AddToGroupAsync(Context.ConnectionId, room);
            }

            return null;
        }

        [Authorize(Roles = "Player")]
        public Task SubscribeToGamePosition()
        {
            var room = Context.User.FindFirst("room")?.Value;

            var tokenPosition = Context.User.FindFirst("position");

            if (room != null && tokenPosition != null && Int32.TryParse(tokenPosition.Value, out int position))
            {
                return Task.WhenAll(
                    Groups.AddToGroupAsync(Context.ConnectionId, $"{room}:position:{position}"), 
                    Groups.AddToGroupAsync(Context.ConnectionId, room + ":team:" + Seat.ForPosition(position).TeamId)
                );
            }

            return null;
        }
    }
}
