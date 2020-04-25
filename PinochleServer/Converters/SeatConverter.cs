using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Pinochle;

namespace PinochleServer.Converters
{
    public class SeatConverter : JsonConverter<Seat>
    {
            public override Seat Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, Seat seat, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(seat.Position);
            }
    }
}
