using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Tricks;

namespace JFadich.Pinochle.Engine.Contracts
{
    public interface IGameRound
    {
        IHand[] Hands { get; }

        void Deal(Seat dealer);

        void PlaceBid(Seat player, int bid);

        void CallTrump(Seat player, PinochleCard.Suits trump);

        void PassCards(Seat from, Seat to, PinochleCard[] cards);

        Trick PlayTrick(Seat player, PinochleCard play);

        void OpenAuction(Seat player);
    }
}
