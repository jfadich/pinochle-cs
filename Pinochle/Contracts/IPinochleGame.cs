using System;
using System.Collections.Generic;
using System.Text;
using Pinochle.Actions;

namespace Pinochle.Contracts
{
    public interface IPinochleGame
    {
        public static int NumberOfPlayers;
        static IPinochleGame Make()
        {
            return new Game();
        }

        void AddGameListener(Action<Events.GameEvent> listener);

        public void TakeAction(PlayerAction action);
    }
}
