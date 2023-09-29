
using JFadich.Pinochle.Engine.Cards;

namespace JFadich.Pinochle.Engine.Tests.Cards
{
    public class CardTests
    {
        [Theory]
        [InlineData(1, "A♣")]
        [InlineData(10, "10♣")]
        [InlineData(13, "K♣")]
        [InlineData(12, "Q♣")]
        [InlineData(11, "J♣")]
        [InlineData(9, "9♣")]
        public void GeneratesShortNameRanksCorrectly(byte value, string expected)
        {
            Card card = new Card(value);

            string shortname = card.GetShortName();

            Assert.Equal(shortname, expected);
        }

        [Theory]
        [InlineData(1, "A♣")]
        [InlineData(17, "A♥")]
        [InlineData(33, "A♠")]
        [InlineData(49, "A♦")]
        public void GeneratesShortNameSuitCorrectly(byte value, string expected)
        {
            Card card = new Card(value);

            string shortname = card.GetShortName();

            Assert.Equal(shortname, expected);
        }

        [Fact]
        public void IsSuitWithDifferentSuitReturnsFalse()
        {
            Card clubs = new Card(Card.Ranks.Ace, Card.Suits.Clubs);
            Card diamonds = new Card(Card.Ranks.Ace, Card.Suits.Diamonds);

            Assert.False(clubs.IsSuit(diamonds));
        }

        [Fact]
        public void IsSuitSameSuitReturnsTrue()
        {
            Card ace = new Card(Card.Ranks.Ace, Card.Suits.Hearts);
            Card nine = new Card(Card.Ranks.Nine, Card.Suits.Hearts);

            Assert.True(ace.IsSuit(nine));
        }
    }
}