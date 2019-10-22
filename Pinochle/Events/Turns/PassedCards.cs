using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle.Events.Turns
{
    class PassedCards : PlayerTurn
    {
        public PassedCards(Player giver, Player reciever) : base(giver, String.Format("{0} passed cards to {1}", giver, reciever)) { }
    }
}
