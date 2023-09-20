
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
        public void GeneratesShortNameCorrectly(byte value, string expected)
        {
            Card card = new Card(value);

            string shortname = card.GetShortName();

            Assert.Equal(shortname, expected);
        }
    }
}