﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Server.Models;

namespace JFadich.Pinochle.Server.RealTime
{
    public interface IGameClient
    {
        Task AddedRoom(Room room);
        Task PlayerJoined(string roomId, Player player);
        Task ClosedLobby(string roomId, string reason);

        Task RecieveCards(string roomId, PinochleCard[] cards);
        Task RemoveCards(string roomId, PinochleCard[] cards);
    }
}
