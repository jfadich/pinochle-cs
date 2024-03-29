﻿using System;
using JFadich.Pinochle.Engine.Contracts;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine.Actions
{
    public class PassCards : PlayerAction
    {
        public PinochleCard[] Cards { get; }

        public Seat ToSeat { get; }

        public PassCards(Seat seat, PinochleCard[] cards) : base(seat, Phases.Passing) 
        {
            Cards = cards;
            ToSeat = Seat.PartnerSeat();
        }
        public override bool Apply(IGameRound round)
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
