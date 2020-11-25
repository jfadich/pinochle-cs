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

namespace JFadich.Pinochle.Server
{
    public class GameManager
    {
        private readonly ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();
        private readonly List<string> Lobbies = new List<string>(); // list of room Ids that are open for new players
        private readonly ConcurrentDictionary<string, string> Players = new ConcurrentDictionary<string, string>(); // [playerId => gameId]

        private IHubContext<GameHub, IGameClient> gameHub;
        public List<Lobby> PublicLobbies { get => AllLobbies.Where(item => !item.IsPrivate).ToList(); }
        public List<Lobby> AllLobbies { get => Lobbies.Select(x => Rooms[x].ToLobby()).ToList(); }
        public List<Room> PublicGames { get => Rooms.Where(item => !item.Value.IsPrivate && item.Value.Status == Room.Statuses.Playing).Select(item => item.Value).ToList(); }
        public List<Room> AllGames => Rooms.Where(item => item.Value.Status == Room.Statuses.Playing).Select(item => item.Value).ToList();

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

        public Lobby FindLobbyForPlayer(string playerId)
        {
            if (Players.ContainsKey(playerId))
            {
                return GetRoom(Players[playerId]).ToLobby();
            }

            foreach (var lobby in AllLobbies)
            {
                if(AddPlayerToRoom(lobby.Id, playerId))
                {
                    return lobby;
                }
            }

            Room newRoom = NewRoom(false);

            if(newRoom != null && AddPlayerToRoom(newRoom.Id, playerId))
            {
                return newRoom.ToLobby();
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
                Seat = Seat.ForPosition(position)
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
                Seat = Seat.ForPosition((int)position)
            }); ;
        }

        public bool AddPlayerToRoom(string id, Player player)
        {
            if (Players.ContainsKey(player.Id))
            {
                if(Players[player.Id] == id)
                {
                    return true;
                }

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
                StartGame(room, 0); // @todo 
            }

            return true;
        }

        private bool StartGame(Room room, int startingPosition)
        {
            Lobbies.Remove(room.Id);

            room.AddGameListener((GameEvent gameEvent) =>
            {
                HandleGameEvent(room, gameEvent);
            });

            gameHub.Clients.Groups(room.Id, Audiences.Matchmaking).ClosedLobby(room.Id, Lobby.ClosedReasons.GameStarting.ToString());

            gameHub.Clients.Groups(room.Id, Audiences.AllGames).GameStarted(room.Id, room);

            return room.StartGame(startingPosition);
        }

        private void HandleGameEvent(Room room, GameEvent gameEvent)
        {
            if (gameEvent is ActionTaken action)
            {
                BroadcastActionTaken(room, action);
            }
        }

        private void BroadcastActionTaken(Room room, ActionTaken actionEvent)
        {
            if(actionEvent.Action is Engine.Actions.Deal)
            {
                for (int i = 0; i < room.Players.Length; i++) 
                {
                    gameHub.Clients.Groups(room.Id + ":position:" + i).TurnTaken(room.Id, TurnTaken.FromEvent(actionEvent, Seat.ForPosition(i)));
                }
            } else
            {
                gameHub.Clients.Groups(room.Id, Audiences.AllGames).TurnTaken(room.Id, TurnTaken.FromEvent(actionEvent, null));
            }
        }
    }
}
