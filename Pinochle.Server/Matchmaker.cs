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
using Pinochle.Engine.Contracts;
using Pinochle.Engine.Events;
using Microsoft.Extensions.Logging;
using Pinochle.Engine.Events.Matchmaking;

namespace JFadich.Pinochle.Server
{
    public class Matchmaker : IMatchmaker
    {
        private readonly ConcurrentDictionary<string, GameRoom> Rooms = new ConcurrentDictionary<string, GameRoom>();
        private readonly HashSet<string> Lobbies = new HashSet<string>(); // list of room Ids that are open for new players
        private readonly ConcurrentDictionary<string, string> Players = new ConcurrentDictionary<string, string>(); // [playerId => gameId]

        private event Action<RoomEvent> RoomEvents;

        public List<GameRoom> PublicLobbies { get => AllLobbies.Where(item => !item.IsPrivate).ToList(); }
        public List<GameRoom> AllLobbies { get => Lobbies.Select(x => Rooms[x]).ToList(); }
        public List<GameRoom> PublicGames { get => Rooms.Where(item => !item.Value.IsPrivate && item.Value.Status == GameRoom.Statuses.Playing).Select(item => item.Value).ToList(); }
        public List<GameRoom> AllGames => Rooms.Where(item => item.Value.Status == GameRoom.Statuses.Playing).Select(item => item.Value).ToList();

        public const int MaxRooms = 100;
        public const int MaxLobbies = 10;

        public Matchmaker()
        {
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
            if (Rooms.Count >= MaxRooms || Lobbies.Count >= MaxLobbies)
            {
                return null;
            }

            string id = Guid.NewGuid().ToString("N");

            var room = new GameRoom(id, isPrivate);

            if (Rooms.TryAdd(id, room))
            {
                Lobbies.Add(id);

                RoomEvents?.Invoke(new RoomCreated(room));

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
                if (AddPlayerToRoom(lobby.Id, playerId))
                {
                    RoomEvents?.Invoke(new PlayerJoined(lobby, lobby.GetPlayer(playerId)));

                    return lobby;
                }
            }

            GameRoom newRoom = NewRoom(false);

            if (newRoom != null && AddPlayerToRoom(newRoom.Id, playerId))
            {
                RoomEvents?.Invoke(new PlayerJoined(newRoom, newRoom.GetPlayer(playerId)));

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

            if (room == null || !room.AddPlayer(playerId, position))
            {
                return false;
            }

            Players[playerId] = roomId;

            if (room.IsFull())
            {
                StartGame(room); // @todo 
            }

            return true;
        }

        private bool StartGame(GameRoom room)
        {
            Lobbies.Remove(room.Id);

            if(room.StartGame())
            {
                RoomEvents?.Invoke(new GameStarted(room));

                return true;
            }

            return false;
        }

        public void AddRoomListener(Action<RoomEvent> listener)
        {
            RoomEvents += listener;
        }
    }
}
