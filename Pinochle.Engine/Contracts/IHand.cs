using System;
using JFadich.Pinochle.Engine.Cards;
using System.Collections.Generic;

namespace JFadich.Pinochle.Engine.Contracts
{
    public interface IHand
    {
        public PinochleCard[] Cards { get; }

        Boolean HasCards(PinochleCard[] needles);

        PinochleCard TakeCard(PinochleCard requestedCard);

        PinochleCard[] TakeCards(PinochleCard[] requestedCards);

        void GiveCards(PinochleCard[] cards);
    }
}
