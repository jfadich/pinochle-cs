using System;
using Pinochle.Cards;

namespace Pinochle.Contracts
{
    public interface IHand
    {
        Boolean HasCards(PinochleCard[] needles);

        PinochleCard TakeCard(PinochleCard requestedCard);

        PinochleCard[] TakeCards(PinochleCard[] requestedCards);

        void GiveCards(PinochleCard[] cards);
    }
}
