using System;
using System.Collections.Generic;
using Pinochle.Cards;
using System.Linq;
using Pinochle.Exceptions;

namespace Pinochle.Tricks
{
    public class Arena
    {
        Card.Suits Trump;

        Seat Leader;

        List<Trick> Tricks;

        public Trick ActiveTrick;

        public Boolean IsPlaying = true;

        public Arena(Card.Suits trump, Seat leader)
        {
            Trump = trump;
            Leader = leader;

            Tricks = new List<Trick>();
        }

        public void StartTrick(PinochleCard card)
        {
            ActiveTrick = new Trick(Leader, card);
        }

        public void PlayTrick(Seat player , PinochleCard play)
        {
            if(ActiveTrick == null || ActiveTrick.IsCompleted)
            {
                if(player.Position != Leader.Position)
                {
                    return;
                }

                StartTrick(play);
            } else
            {
                ActiveTrick.Play(player, play);

                if (ActiveTrick.Plays.Count() == 4) // @todo remove magic number for player count
                {
                    ResolveTrick();
                }
            }
        }

        public void ResolveTrick()
        {
            Play winner = GetWinningPlay();

            ActiveTrick.Resolve(winner);

            Leader = winner.Position;
            Tricks.Add(ActiveTrick);

            if(Tricks.Count() == 12)
            {
                IsPlaying = false;
            }
        }

        public int[] GetTeamScore()
        {
            int[] teamScores = new int[2];

            teamScores[0] = Tricks.Sum(trick => (trick.WinningPlay.Position.Position == 0 || trick.WinningPlay.Position.Position == 2) ? trick.Score() : 0);
            teamScores[1] = Tricks.Sum(trick => (trick.WinningPlay.Position.Position == 1 || trick.WinningPlay.Position.Position == 3) ? trick.Score() : 0);

            return teamScores;
        }

        public Play GetWinningPlay()
        {
            ActiveTrick.WinningPlay = ActiveTrick.Plays.Aggregate(ActiveTrick.WinningPlay, (winner, next) => {
                if (winner.Card.IsSuit(next.Card) || next.Card.IsSuit(Trump))
                {
                    return CardBeatsOpponent(next.Card, winner.Card) ? next : winner;
                }

                return winner;
            });

            return ActiveTrick.WinningPlay;
        }

        /**
         * Determines if the first card beats the second card. The opponet is assumed to have played first and wins in ties.
         */
        protected Boolean CardBeatsOpponent(PinochleCard card, PinochleCard opponent)
        {
            if(card == opponent || (!card.IsSuit(ActiveTrick.LeadSuit) && !card.IsSuit(Trump)))
            {
                return false;
            }

            if (card.IsSuit(opponent))
            {
                return card.GetPinochleValue() > opponent.GetPinochleValue();
            }

            if (card.IsSuit(Trump))
            {
                return true;
            }

            return false;
        }

        public Boolean CanPlay(PinochleCard card, Hand hand, bool throwOnFail = false)
        {
            if (IsPlaying && (ActiveTrick == null || ActiveTrick.Plays.Count() == 0 || ActiveTrick.IsCompleted))
            {
                return true;
            }

            Play winningPlay = GetWinningPlay();
            List<PinochleCard> leadSuitCards = hand.Cards.Where(c => c.IsSuit(ActiveTrick.LeadSuit)).ToList();

            if ( card.IsSuit(ActiveTrick.LeadSuit) )
            {
                if (CardBeatsOpponent(card, winningPlay.Card))
                {
                    return true;
                }

                if(leadSuitCards.Where(c => CardBeatsOpponent(c, winningPlay.Card)).Any())
                {
                    if(throwOnFail)
                    {
                        throw new IllegalTrickException(String.Format("Cannot play {0}: You must beat the winning play if you can.", card));
                    }
                    return false; // You must beat the winning play if you can.
                }

                return true;
            } else if(card.IsSuit(Trump))
            {
                if (leadSuitCards.Any())
                {
                    if (throwOnFail)
                    {
                        throw new IllegalTrickException(String.Format("Cannot play {0}: You must follow suit if possible.", card));
                    }
                    return false; // You must follow suit if possible
                }

                if (CardBeatsOpponent(card, winningPlay.Card))
                {
                    return true;
                }

                if(hand.Cards.Where(c => c.IsSuit(Trump) && CardBeatsOpponent(c, card)).Any() )
                {
                    if (throwOnFail)
                    {
                        throw new IllegalTrickException(String.Format("Cannot play {0}: You must beat the winning play if you can.", card));
                    }
                    return false; // You must beat the winning play if you can.
                }

                return true;
            } else
            {
                if (leadSuitCards.Any())
                {
                    if (throwOnFail)
                    {
                        throw new IllegalTrickException(String.Format("Cannot play {0}: You must follow suit if possible.", card));
                    }
                    return false; // You must follow suit if possible
                }

                if(hand.Cards.Where(c => c.IsSuit(Trump)).Any())
                {
                    if (throwOnFail)
                    {
                        throw new IllegalTrickException(String.Format("Cannot play {0}: You must trump the trick if you can't follow suit.", card));
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
