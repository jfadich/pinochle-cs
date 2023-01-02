using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Models;
using PinochleServer.Models;

namespace JFadich.Pinochle.Server.RealTime
{
    public interface IGameClient
    {
        Task AddedRoom(GameRoom room);
        Task PlayerJoined(string roomId, Player player);
        Task ClosedLobby(string roomId, string reason);

        Task GameStarted(string roomId, GameRoom room);

        Task TurnTaken(string roomId, TurnTaken turn);

        Task ReceiveCards(string roomId, PinochleCard[] cards);

        Task RemoveCards(string roomId, PinochleCard[] cards);
    }
}
