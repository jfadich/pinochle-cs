using System;
using Pinochle.Actions;

namespace Pinochle.Contracts
{
    public interface IPinochleGame
    {
        public int PlayerCount { get; }

        public void AddGameListener(Action<Events.GameEvent> listener);

        void StartGame(int startingPosition = 0);

        public void TakeAction(PlayerAction action);
    }
}
