using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PinochleServer.Models;

namespace PinochleServer.RealTime
{
    public interface IGameClient
    {
        Task AddedRoom(Room room);
        Task PlayerJoined(string roomId, Player player);
        Task ClosedLobby(string roomId, string reason);

        Task RecieveCards(string roomId, Pinochle.Cards.PinochleCard[] cards);
        Task RemoveCards(string roomId, Pinochle.Cards.PinochleCard[] cards);
    }
}
