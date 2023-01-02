using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Server.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using JFadich.Pinochle.Server.RealTime;
using JFadich.Pinochle.Engine.Events;
using PinochleServer.Models;
using JFadich.Pinochle.Engine.Tricks;
using JFadich.Pinochle.Engine.Models;
using JFadich.Pinochle.Engine.Actions;

namespace JFadich.Pinochle.Server
{
    public class GameManager
    {
        private readonly ConcurrentDictionary<string, GameRoom> Rooms = new ConcurrentDictionary<string, GameRoom>();
        private readonly HashSet<string> Lobbies = new HashSet<string>(); // list of room Ids that are open for new players
        private readonly ConcurrentDictionary<string, string> Players = new ConcurrentDictionary<string, string>(); // [playerId => gameId]

        private IHubContext<GameHub, IGameClient> gameHub;
        public List<GameRoom> PublicLobbies { get => AllLobbies.Where(item => !item.IsPrivate).ToList(); }
        public List<GameRoom> AllLobbies { get => Lobbies.Select(x => Rooms[x]).ToList(); }
        public List<GameRoom> PublicGames { get => Rooms.Where(item => !item.Value.IsPrivate && item.Value.Status == GameRoom.Statuses.Playing).Select(item => item.Value).ToList(); }
        public List<GameRoom> AllGames => Rooms.Where(item => item.Value.Status == GameRoom.Statuses.Playing).Select(item => item.Value).ToList();

        public const int MaxRooms = 100;
        public const int MaxLobbies = 10;

        public GameManager(IHubContext<GameHub, IGameClient> gameClients)
        {
            gameHub = gameClients;
        }

        public GameRoom GetPlayersRoom(string playerId)
        {
            if (Players.TryGetValue(playerId, out string roomId))
            {
                return GetRoom(roomId);
            }

            return null;
        }

        public GameRoom GetRoom(string id)
        {
            Rooms.TryGetValue(id, out GameRoom room);

            return room;
        }

        public bool HasLobby(string id)
        {
            return Lobbies.Contains(id);
        }

        public GameRoom NewRoom(bool isPrivate = false)
        {
            if( Rooms.Count >= MaxRooms || Lobbies.Count >= MaxLobbies)
            {
                return null;
            }

            string id = Guid.NewGuid().ToString("N");

            var room = new GameRoom(id, isPrivate);

            if(Rooms.TryAdd(id, room))
            {
                Lobbies.Add(id);
                gameHub.Clients.Group(Audiences.Matchmaking).AddedRoom(room);

                return room;
            }

            return null;
        }

        public GameRoom FindLobbyForPlayer(string playerId)
        {
            if (Players.ContainsKey(playerId))
            {
                return GetRoom(Players[playerId]);
            }

            foreach (var lobby in AllLobbies)
            {
                if(AddPlayerToRoom(lobby.Id, playerId))
                {
                    return lobby;
                }
            }

            GameRoom newRoom = NewRoom(false);

            if(newRoom != null && AddPlayerToRoom(newRoom.Id, playerId))
            {
                return newRoom;
            }

            return null;
        }

        public bool AddPlayerToRoom(string roomId, string playerId, int? position = null)
        {
            if (Players.ContainsKey(playerId))
            {
                return Players[playerId] == roomId;
            }

            var room = GetRoom(roomId);

            if(room == null || !room.AddPlayer(playerId, position))
            {
                return false;
            }

            Players[playerId] = roomId;
            gameHub.Clients.Groups(room.Id, Audiences.Matchmaking).PlayerJoined(room.Id, room.GetPlayer(playerId));

            if (room.IsFull())
            {
                StartGame(room); // @todo 
            }

            return true;
        }

        private bool StartGame(GameRoom room)
        {
            Lobbies.Remove(room.Id);

            room.AddGameListener((GameEvent gameEvent) =>
            {
                HandleGameEvent(room, gameEvent);
            });

            gameHub.Clients.Groups(room.Id, Audiences.Matchmaking).ClosedLobby(room.Id, Lobby.ClosedReasons.GameStarting.ToString());

            gameHub.Clients.Groups(room.Id, Audiences.AllGames).GameStarted(room.Id, room);

            return room.StartGame();
        }

        private void HandleGameEvent(GameRoom room, GameEvent gameEvent)
        {
            if (gameEvent is ActionTaken action)
            {
                BroadcastActionTaken(room, action);
            }

            if(gameEvent is PhaseCompleted phaseCompleted)
            {

            }
        }

        private void BroadcastActionTaken(GameRoom room, ActionTaken actionEvent)
        {
            if(actionEvent.Action is Deal)
            {
                for (int i = 0; i < room.Players.Length; i++) 
                {
                    gameHub.Clients.Groups(room.Id + ":position:" + i).ReceiveCards(room.Id, (actionEvent.Action as Deal).Hands[i].Cards);
                }
            }

            gameHub.Clients.Groups(room.Id, Audiences.AllGames).TurnTaken(room.Id, TurnTaken.FromEvent(actionEvent));
        }
    }
}
