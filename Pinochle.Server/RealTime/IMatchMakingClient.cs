using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Events;
using JFadich.Pinochle.Engine.Models;
using JFadich.Pinochle.Server.Models;
using PinochleServer.Models;

namespace JFadich.Pinochle.Server.RealTime
{
    public interface IMatchMakingClient
    {
        Task RoomAdded(GameRoom room);
        
        Task PlayerJoined(string roomId, Player player);
        
        Task LobbyClosed(string roomId, string reason);

        Task GameStarted(GameRoom room);
    }
}
