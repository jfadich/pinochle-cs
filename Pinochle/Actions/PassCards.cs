using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Cards;

namespace Pinochle.Actions
{
    class PassCards : PlayerAction
    {
        public PinochleCard[] Cards { get; }

        public Seat ToSeat { get; }

        public PassCards(Seat seat, PinochleCard[] cards) : base(seat, Round.Phases.Passing) 
        {
            Cards = cards;
            ToSeat = Seat.PartnerSeat();
        }
        public override bool Apply(Round round)
        {
            round.PassCards(Seat, ToSeat, Cards);

            return true;
        }

        public override string ToString()
        {
            return String.Format("{0} passed cards to {1}", Seat, ToSeat);
        }
    }
}
