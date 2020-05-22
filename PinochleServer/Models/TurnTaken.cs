using JFadich.Pinochle.Engine;
using JFadich.Pinochle.Engine.Actions;
using JFadich.Pinochle.Engine.Events;
using PinochleServer.Models.Turns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PinochleServer.Models
{
    public class TurnTaken
    {
        public string TurnType { get; }

        public Seat NextPlayer { get; }

        public Seat Player { get; }

        public TurnTaken(ActionTaken gameEvent)
        {
            TurnType = gameEvent.Action.GetType().Name;
            Player = gameEvent.Action.Seat;
            NextPlayer = gameEvent.NextPlayer;
        }

        public static TurnTaken FromEvent(ActionTaken gameEvent, Seat seat)
        {
            if(gameEvent.Action is Deal)
            {
                return new PlayerDealt(gameEvent, seat);
            }
            if(gameEvent.Action is PlaceBid bidPlaced)
            {
                return new BidPlaced(gameEvent, bidPlaced.Bid);
            }


            return new TurnTaken(gameEvent);
        }
    }
}
