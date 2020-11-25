using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine.Contracts
{
    public interface IGameRound
    {
        IHand[] Hands { get; }

        void Deal(Seat dealer);

        void PlaceBid(Seat player, int bid);

        void CallTrump(Seat player, PinochleCard.Suits trump);

        void PassCards(Seat from, Seat to, PinochleCard[] cards);

        Tricks.Trick PlayTrick(Seat player, PinochleCard play);

        void OpenAuction(Seat player);
    }
}
