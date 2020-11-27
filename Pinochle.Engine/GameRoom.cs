using System;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Engine.Events;
using JFadich.Pinochle.Engine.Exceptions;
using JFadich.Pinochle.Engine.Models;

namespace JFadich.Pinochle.Engine
{
    public class GameRoom
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

        public bool IsPrivate { get; }

        public GameRoom(string id) : this(id, false) {  }

        public GameRoom(string id, bool isPrivate)
        {
            _game = GameFactory.Make();
            Id = id;
            IsPrivate = isPrivate;
            Players = new Player[_game.PlayerCount];
            Status = Statuses.Matchmaking;
        }

        public bool AddPlayer(string playerId, int? position = null)
        {
            if(position is null)
            {
                position = GetOpenPosition();

                if(position == -1)
                {
                    return false;
                }
            }

            Player player = new Player() { Id = playerId, Seat = Seat.ForPosition((int)position) };

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

            foreach (var player in Players)
            {
                if (player != null && player.Id == p.Id)
                {
                    return false;
                }
            }

            return !IsFull();
        }

        public Player GetPlayer(string playerId)
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

        private int GetOpenPosition()
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
            _game.StartGame();

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
