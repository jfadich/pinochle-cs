using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pinochle;
using PinochleServer.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using PinochleServer.RealTime;

namespace PinochleServer
{
    public class GameManager
    {
        private readonly ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();
        private readonly List<string> Lobbies = new List<string>(); // list of room Ids that are open for new players
        private readonly ConcurrentDictionary<string, string> Players = new ConcurrentDictionary<string, string>(); // [playerId => gameId]

        private IHubContext<GameHub, IGameClient> gameHub;
        public List<Room> PublicLobbies { get => AllLobbies.Where(item => !item.IsPrivate).ToList(); }
        public List<Room> AllLobbies { get => Lobbies.Select(x => Rooms[x]).ToList(); }
        public List<Room> PublicRooms { get => Rooms.Where(item => !item.Value.IsPrivate).Select(item => item.Value).ToList(); }
        public List<Room> AllRooms { get => Rooms.Values.ToList(); }

        public const int MaxRooms = 100;
        public const int MaxLobbies = 10;

        public GameManager(IHubContext<GameHub, IGameClient> gameClients)
        {
            gameHub = gameClients;
        }

        public Room GetRoom(string id)
        {
            Rooms.TryGetValue(id, out Room room);

            return room;
        }

        public bool HasLobby(string id)
        {
            return Lobbies.Contains(id);
        }

        public Room NewRoom(bool isPrivate)
        {
            if( Rooms.Count >= MaxRooms || Lobbies.Count >= MaxLobbies)
            {
                return null;
            }

            string id = Guid.NewGuid().ToString("N");

            var room = new Room(id);

            if(isPrivate)
            {
                room.MakePrivate();
            }

            if(Rooms.TryAdd(id, room))
            {
                Lobbies.Add(id);
                gameHub.Clients.Group(Audiences.Matchmaking).AddedRoom(room);

                return room;
            }

            return null;
        }

        public Room FindRoomForPlayer(string playerId)
        {
            if (Players.ContainsKey(playerId))
            {
                return GetRoom(Players[playerId]);
            }

            foreach (var room in AllLobbies)
            {
                if(AddPlayerToRoom(room.Id, playerId))
                {
                    return room;
                }
            }

            Room newRoom = NewRoom(false);

            if(newRoom != null && AddPlayerToRoom(newRoom.Id, playerId))
            {
                return newRoom;
            }

            return null;
        }

        public bool AddPlayerToRoom(string roomId, string playerId)
        {
            var room = GetRoom(roomId);

            if(room == null)
            {
                return false;
            }

            int position = room.GetOpenPosition();

            if(position == -1)
            {
                return false;
            }

            return AddPlayerToRoom(roomId, new Player()
            {
                Id = playerId,
                Seat = new Seat(position)
            });
        }
        public bool AddPlayerToRoom(string lobbyId, string playerId, int? position)
        {
            if (position == null)
            {
                return AddPlayerToRoom(lobbyId, playerId);
            }

            return AddPlayerToRoom(lobbyId, new Player()
            {
                Id = playerId,
                Seat = new Seat((int)position)
            }); ;
        }

        public bool AddPlayerToRoom(string id, Player player)
        {
            if (Players.ContainsKey(player.Id))
            {
                return false;
            }

            var room = GetRoom(id);

            if(room == null || !room.AddPlayer(player))
            {
                return false;
            }

            Players[player.Id] = id;
            gameHub.Clients.Groups(room.Id, Audiences.Matchmaking).PlayerJoined(room.Id, player);

            if (room.IsFull())
            {
                StartGame(room);
            }

            return true;
        }

        private bool StartGame(Room room)
        {
            Lobbies.Remove(room.Id);

            gameHub.Clients.Groups(room.Id, Audiences.Matchmaking).ClosedLobby(room.Id, Lobby.ClosedReasons.GameStarting.ToString());

            return room.StartGame();
        }
        private static string GenerateID(int length)
        {
            return Guid.NewGuid().ToString("n");//.Substring(0, length);
        }
    }
}
