using System;
using System.Collections.Generic;
using JFadich.Pinochle.Engine.Tricks;
using JFadich.Pinochle.Engine.Cards;
using System.Linq;
using JFadich.Pinochle.Engine.Contracts;
using Pinochle.Engine;

namespace JFadich.Pinochle.Engine
{
    class Round : IGameRound
    {
        public PinochleCard.Suits Trump { get; protected set; }

        public Phases Phase { get; protected set; } = Phases.Dealing;

        public Seat Dealer { get; private set; }

        public WinningBidder Leader { get; private set; }

        public int[] TeamScore = new int[2] { 0,0};

        public List<Meld>[] MeldScore { get; protected set; }

        public int[] TrickScore { get; protected set; }

        public Auction Auction;

        public Arena Arena;

        public IHand[] Hands { get; private set; }

        public Round()
        {
            MeldScore = new List<Meld>[4];
        }

        public void Deal(Seat dealer)
        {
            PinochleDeck deck = PinochleDeck.Make();
            Dealer = dealer;

            Hands = (Hand[])deck.Shuffle().Deal(Game.NumberOfPlayers);

            Auction = new Auction();
            AdvancePhase();
        }

        public Hand PlayerHand(Seat player)
        {
            return (Hand)Hands[player.Position];
        }

        public void CallTrump(Seat player, PinochleCard.Suits trump)
        {
            if(Auction.WinningPosition != player.Position)
            {
                throw new Exceptions.PinochleRuleViolationException(player + " did not win the auction and cannot call trump");
            }

            Leader = new WinningBidder(player, Auction.WinningBid);
            Trump = trump;


            AdvancePhase();
        }

        public void PlaceBid(Seat player, int bid)
        {
            Auction.PlaceBid(player, bid);

            if ( ! Auction.IsOpen )
            {
                AdvancePhase();
            }
        }

        public void PassCards(Seat from, Seat to, PinochleCard[] cards)
        {
            PlayerHand(from).TakeCards(cards);

            PlayerHand(to).GiveCards(cards);

            if (from.Equals(Leader.Seat))
            {
                AdvancePhase();
            }
        }

        public void CalculateMeld(Seat player)
        {
            RegexHandEvaluator eval = new RegexHandEvaluator(); // todo move to class property

            MeldScore[player.Position] = eval.GetMeld(PlayerHand(player), Trump);
        }

        public void OpenAuction(Seat player)
        {

        }

        public void OpenArena()
        {
            Arena = new Arena(Trump, Leader.Seat);
        }

        public Trick PlayTrick(Seat player, PinochleCard play)
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

            return Arena.ActiveTrick;
        }

        public int[] CalculateTeamScore()
        {
            TeamScore = new int[2];
            TrickScore = Arena.GetTeamScore();

            // @todo check for nines 
            if (TrickScore[0] > 0)
            {
                TeamScore[0] += MeldScore[0].Sum(meld => meld.GetValue());
                TeamScore[0] += MeldScore[2].Sum(meld => meld.GetValue());
            }

            if(TrickScore[1] > 0)
            {
                TeamScore[1] += MeldScore[1].Sum(meld => meld.GetValue());
                TeamScore[1] += MeldScore[3].Sum(meld => meld.GetValue());
            }

            TeamScore[0] += TrickScore[0];
            TeamScore[1] += TrickScore[1];

            int winningTeam = Auction.WinningPosition & 1;
            bool metBid = Auction.WinningBid < TeamScore[winningTeam];

            if (!metBid)
            {
                TeamScore[winningTeam] = 0 - Auction.WinningBid;
            }

            return TeamScore;
        }

        public int[] GetCurrentTeamScore()
        {
            var score = new int[2];

            if(Phase > Phases.Passing)
            {
                score[0] += MeldScore[0].Sum(meld => meld.GetValue());
                score[0] += MeldScore[2].Sum(meld => meld.GetValue());

                score[1] += MeldScore[1].Sum(meld => meld.GetValue());
                score[1] += MeldScore[3].Sum(meld => meld.GetValue());
            }

            if(Phase >= Phases.Playing)
            {
                TrickScore = Arena.GetTeamScore();

                score[0] += TrickScore[0];
                score[1] += TrickScore[1];
            }

            return score;
        }

        public int CalculateTeamMeld(int team)
        {
            return MeldScore[team].Sum(meld => meld.GetValue()) +  MeldScore[team + 2].Sum(meld => meld.GetValue());
        }

        public void AdvancePhase()
        {
            if (Phase == Phases.Complete)
            {
                return;
            }

            Phase = (Phases)(Phase + 1);
        }

        public struct WinningBidder {
            public Seat Seat { get; }
            public int Bid { get; }

            public WinningBidder(Seat seat, int bid)
            {
                Seat = seat;
                Bid = bid;
            }
        }
    }
}
