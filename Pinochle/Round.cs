using System;
using System.Collections.Generic;
using Pinochle.Tricks;
using Pinochle.Cards;
using System.Linq;

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

        public Card.Suits Trump { get; protected set; }

        public Phases Phase { get; protected set; } = Phases.Dealing;

        public Player Dealer { get; private set; }

        public Player Leader { get; private set; }

        public int[] TeamScore = new int[2] { 0,0};

        public List<Meld>[] MeldScore { get; protected set; }

        public int[] TrickScore { get; protected set; }

        public Auction Auction;

        public Arena Arena;

        public Hand[] Hands { get; protected set; }

        public Round()
        {
            MeldScore = new List<Meld>[4];
        }

        public void Deal(Player dealer, int players)
        {
            PinochleDeck deck = PinochleDeck.Make();
            Dealer = dealer;

            Hands = deck.Shuffle().Deal(players);

            Auction = new Auction();
            AdvancePhase();
        }

        public Hand PlayerHand(Player player)
        {
            return Hands[player.Position];
        }

        public void CallTrump(Player player, Card.Suits trump)
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

        public void OpenArena()
        {
            Arena = new Arena(Trump, Leader);
        }

        public void PlayTrick(Player player, PinochleCard play)
        {
            if (Phase != Phases.Playing || Arena == null || !Arena.IsPlaying)
            {
                throw new Exceptions.IllegalTrickException("Arena is not open");
            }

            Arena.CanPlay(play, PlayerHand(player), true);

            PlayerHand(player).TakeCard(play);
            Arena.PlayTrick(player, play);

            if( ! Arena.IsPlaying )
            {
                AdvancePhase();
            }
        }

        public int[] CalculateTeamScore()
        {
            TeamScore = new int[2];
            TrickScore = Arena.GetTeamScore();

            if (TrickScore[0] > 0)
            {
                TeamScore[0] += MeldScore[0].Sum(meld => meld.GetValue());
                TeamScore[0] += MeldScore[2].Sum(meld => meld.GetValue());
            }

            if(TrickScore[1] > 0)
            {
                TeamScore[1] += MeldScore[1].Sum(meld => meld.GetValue());
                TeamScore[1] += MeldScore[2].Sum(meld => meld.GetValue());
            }

            TeamScore[0] += TrickScore[0];
            TeamScore[1] += TrickScore[1];

            if(Auction.WinningPosition == 0 || Auction.WinningPosition == 2){
                Auction.MetBid = Auction.WinningBid < TeamScore[0];
                if ( ! Auction.MetBid)
                {
                    TeamScore[0] = 0 - Auction.WinningBid;
                }
            } else
            {
                Auction.MetBid = Auction.WinningBid < TeamScore[1];
                if (!Auction.MetBid)
                {
                    TeamScore[1] = 0 - Auction.WinningBid;
                }
            }

            return TeamScore;
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
