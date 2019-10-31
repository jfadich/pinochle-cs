using System;
using System.Collections.Generic;
using Pinochle.Cards;
namespace Pinochle.Events.Turns
{
    class PassedCards : PlayerTurn
    {
        public Card[] Cards;
        public PassedCards(Player giver, Player reciever, Card[] cards) : base(giver, String.Format("{0} passed cards to {1}", giver, reciever)) {
            Cards = cards;
        }
    }
}
