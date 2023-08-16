using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Events;
using Pinochle.Engine.Events;
using System;
using System.Collections.Generic;

namespace Pinochle.Engine.Contracts
{
    /// <summary>
    /// @todo move this to server
    /// </summary>
    public interface IMatchmaker
    {
        List<GameRoom> AllGames { get; }
        List<GameRoom> AllLobbies { get; }
        List<GameRoom> PublicGames { get; }
        List<GameRoom> PublicLobbies { get; }

        bool AddPlayerToRoom(string roomId, string playerId, int? position = null);
        GameRoom FindLobbyForPlayer(string playerId);
        GameRoom GetPlayersRoom(string playerId);
        GameRoom GetRoom(string id);
        bool HasLobby(string id);

        void AddRoomListener(Action<RoomEvent> listener);
    }
}