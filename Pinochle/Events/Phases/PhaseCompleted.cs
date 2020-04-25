
namespace Pinochle.Events.Phases
{
    public abstract class PhaseCompleted
    {
        public string Message { get; protected set; }
        public Round.Phases Phase { get; protected set; }

        public PhaseCompleted(Round.Phases phase, string message)
        {
            Phase = phase;
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
