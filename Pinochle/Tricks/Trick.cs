using System;
using System.Collections.Generic;
using System.Linq;
using Pinochle.Cards;

namespace Pinochle.Tricks
{
    class Trick
    {
        public Card LeadCard { get; protected set; }

        public Card.Suits LeadSuit { get; protected set; }

        public List<Play> Plays;

        public Boolean IsCompleted = false;

        public Play WinningPlay;

        public Trick(Player leader, PinochleCard lead)
        {
            LeadCard = lead;
            LeadSuit = lead.getSuit();
            WinningPlay = new Play(leader, lead);
            Plays = new List<Play>{WinningPlay};
        }

        public Play Play(Player player , PinochleCard playedCard)
        {
            Play play = new Play(player, playedCard);
            Plays.Add(play);

            return play;
        }

       public int Score()
        {
            return Plays.Sum(play => play.Card.IsCounter() ? 10 : 0);
        }

        public void Resolve(Play winner)
        {
            IsCompleted = true;
            WinningPlay = winner;
        }
    }
}
