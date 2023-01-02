using System;
using System.Collections;
using System.Collections.Generic;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine.Contracts
{
    public interface IPinochleGame
    {
        int PlayerCount { get; }

        Seat ActivePlayer { get; }

        GameScore Score { get; }

        bool IsCompleted { get; }

        Phases CurrentPhase { get; }

        bool IsPhase(Phases phase);

        void AddGameListener(Action<Events.GameEvent> listener);

        void StartGame(int startingPosition = 0);

        IHand GetPlayerHand(Seat player);

        bool CanPlay(PinochleCard card);

        void TakeAction(PlayerAction action);

        ICollection<Meld> GetPlayerMeld(Seat player);

        // PlayerAction SuggestAction();
    }
}
