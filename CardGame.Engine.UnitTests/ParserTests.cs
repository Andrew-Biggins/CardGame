using CardGame.Engine.Model;
using CardGame.Engine.Services.Parsing;

namespace CardGame.Engine.UnitTests;

public class ParserTests
{
    [GwtTheory("Given a CSV card parser",
           "when parsing a valid single card",
           "then the correct value and suit are returned")]
    [InlineData("2C", true, CardValue.Two, CardSuit.Clubs)]
    [InlineData("3C", true, CardValue.Three, CardSuit.Clubs)]
    [InlineData("4C", true, CardValue.Four, CardSuit.Clubs)]
    [InlineData("5C", true, CardValue.Five, CardSuit.Clubs)]
    [InlineData("6C", true, CardValue.Six, CardSuit.Clubs)]
    [InlineData("7C", true, CardValue.Seven, CardSuit.Clubs)]
    [InlineData("8C", true, CardValue.Eight, CardSuit.Clubs)]
    [InlineData("9C", true, CardValue.Nine, CardSuit.Clubs)]
    [InlineData("TC", true, CardValue.Ten, CardSuit.Clubs)]
    [InlineData("JC", true, CardValue.Jack, CardSuit.Clubs)]
    [InlineData("QC", true, CardValue.Queen, CardSuit.Clubs)]
    [InlineData("KC", true, CardValue.King, CardSuit.Clubs)]
    [InlineData("AC", true, CardValue.Ace, CardSuit.Clubs)]

    [InlineData("2D", true, CardValue.Two, CardSuit.Diamonds)]
    [InlineData("3D", true, CardValue.Three, CardSuit.Diamonds)]
    [InlineData("4D", true, CardValue.Four, CardSuit.Diamonds)]
    [InlineData("5D", true, CardValue.Five, CardSuit.Diamonds)]
    [InlineData("6D", true, CardValue.Six, CardSuit.Diamonds)]
    [InlineData("7D", true, CardValue.Seven, CardSuit.Diamonds)]
    [InlineData("8D", true, CardValue.Eight, CardSuit.Diamonds)]
    [InlineData("9D", true, CardValue.Nine, CardSuit.Diamonds)]
    [InlineData("TD", true, CardValue.Ten, CardSuit.Diamonds)]
    [InlineData("JD", true, CardValue.Jack, CardSuit.Diamonds)]
    [InlineData("QD", true, CardValue.Queen, CardSuit.Diamonds)]
    [InlineData("KD", true, CardValue.King, CardSuit.Diamonds)]
    [InlineData("AD", true, CardValue.Ace, CardSuit.Diamonds)]

    [InlineData("2H", true, CardValue.Two, CardSuit.Hearts)]
    [InlineData("3H", true, CardValue.Three, CardSuit.Hearts)]
    [InlineData("4H", true, CardValue.Four, CardSuit.Hearts)]
    [InlineData("5H", true, CardValue.Five, CardSuit.Hearts)]
    [InlineData("6H", true, CardValue.Six, CardSuit.Hearts)]
    [InlineData("7H", true, CardValue.Seven, CardSuit.Hearts)]
    [InlineData("8H", true, CardValue.Eight, CardSuit.Hearts)]
    [InlineData("9H", true, CardValue.Nine, CardSuit.Hearts)]
    [InlineData("TH", true, CardValue.Ten, CardSuit.Hearts)]
    [InlineData("JH", true, CardValue.Jack, CardSuit.Hearts)]
    [InlineData("QH", true, CardValue.Queen, CardSuit.Hearts)]
    [InlineData("KH", true, CardValue.King, CardSuit.Hearts)]
    [InlineData("AH", true, CardValue.Ace, CardSuit.Hearts)]

    [InlineData("2S", true, CardValue.Two, CardSuit.Spades)]
    [InlineData("3S", true, CardValue.Three, CardSuit.Spades)]
    [InlineData("4S", true, CardValue.Four, CardSuit.Spades)]
    [InlineData("5S", true, CardValue.Five, CardSuit.Spades)]
    [InlineData("6S", true, CardValue.Six, CardSuit.Spades)]
    [InlineData("7S", true, CardValue.Seven, CardSuit.Spades)]
    [InlineData("8S", true, CardValue.Eight, CardSuit.Spades)]
    [InlineData("9S", true, CardValue.Nine, CardSuit.Spades)]
    [InlineData("TS", true, CardValue.Ten, CardSuit.Spades)]
    [InlineData("JS", true, CardValue.Jack, CardSuit.Spades)]
    [InlineData("QS", true, CardValue.Queen, CardSuit.Spades)]
    [InlineData("KS", true, CardValue.King, CardSuit.Spades)]
    [InlineData("AS", true, CardValue.Ace, CardSuit.Spades)]

    [InlineData("JR", true, CardValue.Joker, CardSuit.None)]

    // Invalid or unrecognised cases
    [InlineData("", false, CardValue.None, CardSuit.None)]           // Empty
    [InlineData(" ", false, CardValue.None, CardSuit.None)]          // Whitespace
    [InlineData(null, false, CardValue.None, CardSuit.None)]         // Null
    [InlineData("1S", false, CardValue.None, CardSuit.None)]         // Invalid value
    [InlineData("2X", false, CardValue.None, CardSuit.None)]         // Invalid suit
    [InlineData("ZZ", false, CardValue.None, CardSuit.None)]         // Total nonsense
    [InlineData("TSC", false, CardValue.None, CardSuit.None)]        // Too long
    [InlineData("A", false, CardValue.None, CardSuit.None)]          // Too short
    [InlineData("J R", false, CardValue.None, CardSuit.None)]        // Wrong spacing
    [InlineData("2B", false, CardValue.None, CardSuit.None)]         // Suit not recognised
    [InlineData("1C", false, CardValue.None, CardSuit.None)]         // Value not recognised
    public void T0(string input, bool expectedResult, CardValue expectedvalue, CardSuit expectedSuit)
    {
        //Arrange & Act
        var result = CsvCardParser.TryParseCard(input, out var card);

        //Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedvalue, card.Value);
        Assert.Equal(expectedSuit, card.Suit);
    }

    [GwtTheory("Given a CSV card parser",
           "when parsing multiple cards",
           "then all valid cards are returned or parsing fails cleanly")]
    [InlineData("2C", true, 1)]                // Valid single
    [InlineData("2C,3D,AH", true, 3)]          // Valid multiple
    [InlineData(" 2C , 3D , AH ", true, 3)]    // Extra whitespace
    [InlineData("2c,3d,ah", true, 3)]          // Lowercase input
    [InlineData("2C,", false, 0)]              // Trailing comma -> invalid
    [InlineData(",2C", false, 0)]              // Leading comma -> invalid
    [InlineData("2C,,3D", false, 0)]           // Double commas -> invalid
    [InlineData("2C,1S,3D", false, 0)]         // Contains an unrecognised card
    [InlineData("2C 3D", false, 0)]            // Invalid separator (no commas)
    [InlineData("JR,2C,3D", true, 3)]          // Joker handling
    [InlineData("JR,2C,JR", true, 3)]          // Two Jokers allowed
    [InlineData("JR,JR,JR", true, 3)]          // Three Jokers - will fail at hand validation stage, but here still shows list
    public void T1(string input, bool expectedResult, int expectedCount)
    {
        // Arrange & Act
        var parser = new CsvCardParser();
        var result = parser.TryParseMany(input, out var cards);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedCount, cards.Count);
    }
}