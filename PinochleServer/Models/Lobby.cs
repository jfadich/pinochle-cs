using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinochle;

namespace PinochleServer.Models
{
    public class Lobby
    {
        public enum ClosedReasons {
            GameStarting,
            LobbyExpired
        };

        public string RoomId { get; }

        public Player[] Players { get; }

        public Lobby(string id, Player[] players)
        {
            RoomId = id;
            Players = players;
        }
    }
}
