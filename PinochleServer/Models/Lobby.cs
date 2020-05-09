using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;

namespace JFadich.Pinochle.Server.Models
{
    public class Lobby
    {
        public enum ClosedReasons {
            GameStarting,
            LobbyExpired
        };

        public string Id { get; }

        public Player[] Players { get; }

        public bool IsPrivate { get; }

        public Lobby(string id, Player[] players, bool isPrivate)
        {
            Id = id;
            Players = players;
            IsPrivate = isPrivate;
        }
    }
}
