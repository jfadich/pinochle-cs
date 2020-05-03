using System;
using System.Collections.Generic;
using System.Linq;
using Pinochle.Cards;

namespace Pinochle.Tricks
{
    public class Trick
    {
        public Card LeadCard { get; private set; }

        public Card.Suits LeadSuit { get; private set; }

        public List<Play> Plays;

        public Boolean IsCompleted = false;

        public Play WinningPlay;

        public Trick(Seat leader, PinochleCard lead)
        {
            LeadCard = lead;
            LeadSuit = lead.getSuit();
            WinningPlay = new Play(leader, lead);
            Plays = new List<Play>{WinningPlay};
        }

        public Play Play(Seat player , PinochleCard playedCard)
        {
            Play play = new Play(player, playedCard);
            Plays.Add(play);

            return play;
        }

       public int Score()
        {
            return Plays.Sum(play => play.Card.IsCounter() ? 1 : 0);
        }

        public void Resolve(Play winner)
        {
            IsCompleted = true;
            WinningPlay = winner;
        }
    }
}
