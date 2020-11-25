using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Server.Converters
{
    public class PinochleCardConverter : JsonConverter<PinochleCard>
    {
            public override PinochleCard Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, PinochleCard card, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(card.Value);
            }
    }
}
