using JFadich.Pinochle.Engine.Exceptions;
using System;

namespace JFadich.Pinochle.Engine
{
    public class Seat
    {
        public int Position { get; }

        private const int PartnerMask = 0b00_00_0001;

        public int TeamId { get => Position & PartnerMask; }

        public static Seat ForPosition(int position)
        {
            if(!Game.IsValidPosition(position))
            {
                throw new PinochleRuleViolationException("Invalid seat position");
            }

            return new Seat(position);
        }

        public Seat(int position)
        {
            Position = Game.ValidatePosition(position);
        }

        public Boolean IsSameTeam(Seat seat)
        {
            return seat.TeamId == TeamId;
        }

        public int NextPosition(int sameTeam = 0) => (Position + 1 + sameTeam) & 3;

        public Seat PartnerSeat() => new Seat(NextPosition(1));

        public override string ToString()
        {
            return String.Format("Seat {0}", Position + 1);
        }
    }
}
