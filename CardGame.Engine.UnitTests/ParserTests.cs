using CardGame.Engine.Model;
using CardGame.Engine.Services;

namespace CardGame.Engine.UnitTests;

public class ParserTests
{
    [GwtTheory("Given a CSV card parser",
           "when parsing a valid single card",
           "then the correct rank and suit are returned")]
    [InlineData("2C", true, CardRank.Two, CardSuit.Clubs)]
    [InlineData("3C", true, CardRank.Three, CardSuit.Clubs)]
    [InlineData("4C", true, CardRank.Four, CardSuit.Clubs)]
    [InlineData("5C", true, CardRank.Five, CardSuit.Clubs)]
    [InlineData("6C", true, CardRank.Six, CardSuit.Clubs)]
    [InlineData("7C", true, CardRank.Seven, CardSuit.Clubs)]
    [InlineData("8C", true, CardRank.Eight, CardSuit.Clubs)]
    [InlineData("9C", true, CardRank.Nine, CardSuit.Clubs)]
    [InlineData("TC", true, CardRank.Ten, CardSuit.Clubs)]
    [InlineData("JC", true, CardRank.Jack, CardSuit.Clubs)]
    [InlineData("QC", true, CardRank.Queen, CardSuit.Clubs)]
    [InlineData("KC", true, CardRank.King, CardSuit.Clubs)]
    [InlineData("AC", true, CardRank.Ace, CardSuit.Clubs)]

    [InlineData("2D", true, CardRank.Two, CardSuit.Diamonds)]
    [InlineData("3D", true, CardRank.Three, CardSuit.Diamonds)]
    [InlineData("4D", true, CardRank.Four, CardSuit.Diamonds)]
    [InlineData("5D", true, CardRank.Five, CardSuit.Diamonds)]
    [InlineData("6D", true, CardRank.Six, CardSuit.Diamonds)]
    [InlineData("7D", true, CardRank.Seven, CardSuit.Diamonds)]
    [InlineData("8D", true, CardRank.Eight, CardSuit.Diamonds)]
    [InlineData("9D", true, CardRank.Nine, CardSuit.Diamonds)]
    [InlineData("TD", true, CardRank.Ten, CardSuit.Diamonds)]
    [InlineData("JD", true, CardRank.Jack, CardSuit.Diamonds)]
    [InlineData("QD", true, CardRank.Queen, CardSuit.Diamonds)]
    [InlineData("KD", true, CardRank.King, CardSuit.Diamonds)]
    [InlineData("AD", true, CardRank.Ace, CardSuit.Diamonds)]

    [InlineData("2H", true, CardRank.Two, CardSuit.Hearts)]
    [InlineData("3H", true, CardRank.Three, CardSuit.Hearts)]
    [InlineData("4H", true, CardRank.Four, CardSuit.Hearts)]
    [InlineData("5H", true, CardRank.Five, CardSuit.Hearts)]
    [InlineData("6H", true, CardRank.Six, CardSuit.Hearts)]
    [InlineData("7H", true, CardRank.Seven, CardSuit.Hearts)]
    [InlineData("8H", true, CardRank.Eight, CardSuit.Hearts)]
    [InlineData("9H", true, CardRank.Nine, CardSuit.Hearts)]
    [InlineData("TH", true, CardRank.Ten, CardSuit.Hearts)]
    [InlineData("JH", true, CardRank.Jack, CardSuit.Hearts)]
    [InlineData("QH", true, CardRank.Queen, CardSuit.Hearts)]
    [InlineData("KH", true, CardRank.King, CardSuit.Hearts)]
    [InlineData("AH", true, CardRank.Ace, CardSuit.Hearts)]

    [InlineData("2S", true, CardRank.Two, CardSuit.Spades)]
    [InlineData("3S", true, CardRank.Three, CardSuit.Spades)]
    [InlineData("4S", true, CardRank.Four, CardSuit.Spades)]
    [InlineData("5S", true, CardRank.Five, CardSuit.Spades)]
    [InlineData("6S", true, CardRank.Six, CardSuit.Spades)]
    [InlineData("7S", true, CardRank.Seven, CardSuit.Spades)]
    [InlineData("8S", true, CardRank.Eight, CardSuit.Spades)]
    [InlineData("9S", true, CardRank.Nine, CardSuit.Spades)]
    [InlineData("TS", true, CardRank.Ten, CardSuit.Spades)]
    [InlineData("JS", true, CardRank.Jack, CardSuit.Spades)]
    [InlineData("QS", true, CardRank.Queen, CardSuit.Spades)]
    [InlineData("KS", true, CardRank.King, CardSuit.Spades)]
    [InlineData("AS", true, CardRank.Ace, CardSuit.Spades)]

    [InlineData("JR", true, CardRank.Joker, CardSuit.None)]

    //
    // Invalid or unrecognised cases
    //
    [InlineData("", false, CardRank.None, CardSuit.None)]           // Empty
    [InlineData(" ", false, CardRank.None, CardSuit.None)]          // Whitespace
    [InlineData(null, false, CardRank.None, CardSuit.None)]         // Null
    [InlineData("1S", false, CardRank.None, CardSuit.None)]         // Invalid rank
    [InlineData("2X", false, CardRank.None, CardSuit.None)]         // Invalid suit
    [InlineData("ZZ", false, CardRank.None, CardSuit.None)]         // Total nonsense
    [InlineData("TSC", false, CardRank.None, CardSuit.None)]        // Too long
    [InlineData("A", false, CardRank.None, CardSuit.None)]          // Too short
    [InlineData("J R", false, CardRank.None, CardSuit.None)]        // Wrong spacing
    [InlineData("2B", false, CardRank.None, CardSuit.None)]         // Suit not recognised
    [InlineData("1C", false, CardRank.None, CardSuit.None)]         // Rank not recognised
    public void T0(string input, bool expectedResult, CardRank expectedRank, CardSuit expectedSuit)
    {
        //Arrange & Act
        var result = CsvCardParser.TryParseCard(input, out var card);

        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedRank, card.Value);
        Assert.Equal(expectedSuit, card.Suit);
    }
}