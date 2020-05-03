using System;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine.Contracts
{
    public interface IHand
    {
        Boolean HasCards(PinochleCard[] needles);

        PinochleCard TakeCard(PinochleCard requestedCard);

        PinochleCard[] TakeCards(PinochleCard[] requestedCards);

        void GiveCards(PinochleCard[] cards);
    }
}
