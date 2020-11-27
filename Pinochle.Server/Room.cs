using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Engine.Events;
using JFadich.Pinochle.Engine.Exceptions;
using JFadich.Pinochle.Server.Models;

namespace JFadich.Pinochle.Server
{
    public class Room
    {
        public Statuses Status { get; private set; }

        public enum Statuses
        {
            Matchmaking,
            Playing,
            Closed
        }

        public string Id { get; }

        public Player[] Players { get; }

        private IPinochleGame _game { get; }

        public bool IsPrivate { get;}

        public Room(string id) : this(id, false) {  }

        public Room(string id, bool isPrivate)
        {
            _game = GameFactory.Make();
            Id = id;
            IsPrivate = isPrivate;
            Players = new Player[_game.PlayerCount];
            Status = Statuses.Matchmaking;
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

        private Player GetPlayer(string playerId)
        {
            foreach (var player in Players)
            {
                if (player != null && player.Id == playerId)
                {
                    return player;
                }
            }

            throw new InvalidActionException(string.Format("Player '{0}' is not in Room '{1}", playerId, this.Id));
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

        public Lobby ToLobby()
        {
            return new Lobby(Id, Players, IsPrivate);
        }

        public bool StartGame(string playerId = null)
        {
            if (string.IsNullOrEmpty(playerId))
            {
                _game.StartGame(_game.ActivePlayer.Position);
            }
            else
            {
                Player player = GetPlayer(playerId);
                _game.StartGame(player.Seat.Position);
            }

            Status = Statuses.Playing;

            return true;
        }

        public IHand GetPlayerHand(string playerId)
        {
            Player player = GetPlayer(playerId);

            return _game.GetPlayerHand(player.Seat);
        }

        public void PlaceBid(string playerId, int bid)
        {
            Player player = GetPlayer(playerId);

            _game.TakeAction(new PlaceBid(player.Seat, bid));
        }

        public void CallTrump(string playerId, Card.Suits trump)
        {
            Player player = GetPlayer(playerId);

            _game.TakeAction(new CallTrump(player.Seat, trump));
        }

        public void PassCards(string playerId, PinochleCard[] cards)
        {
            Player player = GetPlayer(playerId);

            _game.TakeAction(new PassCards(player.Seat, cards));
        }

        public void PlayTrick(string playerId, PinochleCard trick)
        {
            Player player = GetPlayer(playerId);

            _game.TakeAction(new PlayTrick(player.Seat, trick));
        }

        public void AddGameListener(Action<GameEvent> listener)
        {
            _game.AddGameListener(listener);
        }
    }
}
