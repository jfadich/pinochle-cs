using System;
using System.Collections.Generic;
using System.Text;

namespace Pinochle
{
    class Round
    {
        public enum Phases
        {
            Dealing,
            Bidding,
            Calling,
            Passing,
            Playing,
            Complete
        }

        public Cards.Card.Suits Trump { get; protected set; }

        public Phases Phase { get; protected set; } = Phases.Dealing;

        public Player Dealer;

        public Player Leader;

        public List<Meld>[] MeldScore { get; protected set; }

        public int[] TrickScore { get; protected set; }

        public Auction Auction;

        public Hand[] Hands { get; protected set; }

        public Round()
        {
            MeldScore = new List<Meld>[4];
        }

        public void Deal(Player Dealer)
        {
            Cards.Deck deck = Cards.PinochleDeck.Make();
            this.Dealer = Dealer;

            Hands = deck.Shuffle().Deal(4);

            Auction = new Auction();
            AdvancePhase();
        }

        public Hand PlayerHand(Player player)
        {
            return Hands[player.Position];
        }

        public void CallTrump(Player player, Cards.Card.Suits trump)
        {
            if(Auction.WinningPosition != player.Position)
            {
                throw new Exceptions.PinochleRuleViolationException(player + " did not win the auction and cannot call trump");
            }

            Leader = player;
            Trump = trump;

            AdvancePhase();
        }

        public void CalculateMeld(Player player)
        {
            HandEvaluator eval = new HandEvaluator(PlayerHand(player), Trump);

            MeldScore[player.Position] = eval.GetMeld();
        }

        public void AdvancePhase()
        {
            if (Phase == Phases.Complete)
            {
                return;
            }

            Phase = (Phases)(Phase + 1);
        }
    }
}
