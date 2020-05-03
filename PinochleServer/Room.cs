using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Server.Models;

namespace JFadich.Pinochle.Server
{
    public class Room
    {
        public string Status { get; private set; }

        public enum Statuses
        {
            Matchmaking,
            Playing,
            Closed
        }

        public string Id { get; }

        public Player[] Players { get; }

        public IPinochleGame Game { get; }

        public bool IsPrivate { get; private set; }

        public Room(string id)
        {
            Game = GameFactory.Make();
            Id = id;
            Players = new Player[Game.PlayerCount];
            Status = "Seating";
        }
        public void MakePrivate()
        {
            IsPrivate = true;
        }

        public bool AddPlayer(Player player)
        {
            if (!CanSeat(player))
            {
                return false;
            }

            Players[player.Seat.Position] = player;

            return true;
        }

        private bool CanSeat(Player p)
        {
            if (Players[p.Seat.Position] != null)
            {
                return false;
            }

            return !IsFull();
        }

        public bool IsPlaying(string playerId)
        {
            foreach (var player in Players)
            {
                if (player != null && player.Id == playerId)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetOpenPosition()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool IsFull()
        {
            return GetOpenPosition() == -1;
        }

        public bool StartGame()
        {
            Status = "Active";

            Game.StartGame();

            return true;
        }
    }
}
