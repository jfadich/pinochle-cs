using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    public class Seat
    {
        public int Position { get; }

        private const int PartnerMask = 0b00_00_0001;

        public int TeamId { get => Position & PartnerMask; }

        public Seat(int position)
        {
            Position = Game.ValidPosition(position) ? position : 0;
        }

        public Boolean IsSameTeam(Seat seat)
        {
            return seat.TeamId == TeamId;
        }

        public override string ToString()
        {
            return String.Format("Seat {0}", Position + 1);
        }
    }
}
