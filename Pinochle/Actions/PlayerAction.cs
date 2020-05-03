using Pinochle.Contracts;

namespace Pinochle.Actions
{
    public abstract class PlayerAction
    {
        public Phases Phase { get;}

        public Seat Seat { get; }

        public PlayerAction (Seat seat, Phases phase)
        {
            Phase = phase;
            Seat = seat;
        }

        public abstract bool Apply(IGameRound round);
    }
}
