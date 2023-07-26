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
using Pinochle.Engine.Events.Matchmaking;
using System.Text.RegularExpressions;

namespace JFadich.Pinochle.Server
{
    public class GameManager
    {
        private readonly IHubContext<GameHub, IGameClient> gameHub;
        private readonly IHubContext<MatchmakingHub, IMatchMakingClient> matchmakingHub;

        public readonly IMatchmaker Matchmaker;

        public GameManager(IHubContext<GameHub, IGameClient> gameClients, IHubContext<MatchmakingHub, IMatchMakingClient> matchmakingHub, IMatchmaker matchmaker)
        {
            gameHub = gameClients;
            this.matchmakingHub = matchmakingHub;
            this.Matchmaker = matchmaker;

            this.Matchmaker.AddRoomListener(this.HandleRoomEvent);
        }

        private void HandleRoomEvent(RoomEvent roomEvent)
        {
            switch(roomEvent)
            {
                case RoomCreated roomCreated:
                    roomCreated.Room.AddGameListener((GameEvent gameEvent) => { HandleGameEvent(roomCreated.Room, gameEvent); });
                    matchmakingHub.Clients.Group(Audiences.Matchmaking).RoomAdded(roomCreated.Room);
                    break;
                case PlayerJoined playerJoined:
                    gameHub.Clients.Groups(playerJoined.Room.Id, Audiences.Matchmaking).PlayerJoined(playerJoined.Player);
                    matchmakingHub.Clients.Groups(playerJoined.Room.Id, Audiences.Matchmaking).PlayerJoined(playerJoined.Room.Id, playerJoined.Player);
                    break;
                case GameStarted gameStarted:
                    matchmakingHub.Clients.Groups(Audiences.Matchmaking).LobbyClosed(gameStarted.Room.Id, Lobby.ClosedReasons.GameStarting.ToString());
                    matchmakingHub.Clients.Groups(Audiences.AllGames).GameStarted(gameStarted.Room);
                    break;
            }
        }

        private void HandleGameEvent(GameRoom room, GameEvent gameEvent)
        {
            switch(gameEvent)
            {
                case ActionTaken action:
                    BroadcastActionTaken(room, action);
                    break;
                case PhaseCompleted phaseCompleted:
                    break;
            }
        }

        private void BroadcastActionTaken(GameRoom room, ActionTaken actionEvent)
        {
            if(actionEvent.Action is Deal deal)
            {
                for (int i = 0; i < room.Players.Length; i++) 
                {
                    gameHub.Clients.Groups(room.Id + ":position:" + i).CardsReceived(deal.Hands[i].Cards);
                }
            }

            gameHub.Clients.Groups(room.Id, Audiences.AllGames).TurnTaken(TurnTaken.FromEvent(actionEvent));
        }
    }
}
