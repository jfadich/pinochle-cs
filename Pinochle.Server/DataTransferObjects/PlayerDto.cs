using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pinochle.Server.DataTransferObjects
{
    public class PlayerDto
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("seat")]
        public int Seat { get; set; }
    }
}
