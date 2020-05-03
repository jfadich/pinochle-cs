using System;
using JFadich.Pinochle.Engine.Actions;

namespace JFadich.Pinochle.Engine.Contracts
{
    public interface IPinochleGame
    {
        public int PlayerCount { get; }

        public void AddGameListener(Action<Events.GameEvent> listener);

        void StartGame(int startingPosition = 0);

        public void TakeAction(PlayerAction action);
    }
}
