using JFadich.Pinochle.Engine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pinochle.Server.DataTransferObjects
{
    public class GameRoomDto
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("players")]
        public PlayerDto[] Players { get; }
    }
}
