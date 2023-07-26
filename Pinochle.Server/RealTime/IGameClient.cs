using System.Threading.Tasks;
using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Cards;
using JFadich.Pinochle.Engine.Models;
using PinochleServer.Models;

namespace JFadich.Pinochle.Server.RealTime
{
    public interface IGameClient
    {
        Task PlayerJoined(Player player);

        Task TurnTaken(TurnTaken turn);

        Task CardsReceived(PinochleCard[] cards);

        Task CardsTaken(PinochleCard[] cards);
    }
}
