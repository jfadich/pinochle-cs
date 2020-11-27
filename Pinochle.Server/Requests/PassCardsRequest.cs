using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Server.Requests
{
    public class PassCardsRequest
    {
        public PinochleCard[] Cards { get; set; }
    }
}
