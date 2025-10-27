using CardGame.Engine.Services;

namespace CardGame.Engine.UnitTests;
public class ScoreTests
{
    [GwtTheory("Given a score calculator",
           "when evaluating a list of cards",
           "then the correct total is returned")]
    // Single card per suit
    [InlineData("2C", 2)]
    [InlineData("2D", 4)]
    [InlineData("2H", 6)]
    [InlineData("2S", 8)]
    // Face cards
    [InlineData("JC", 11)]
    [InlineData("QC", 12)]
    [InlineData("KC", 13)]
    [InlineData("AC", 14)]
    // Multiple cards
    [InlineData("3C,4C", 7)]
    [InlineData("TC,TD,TH,TS", 100)]
    // Joker doubling
    [InlineData("2C,JR", 4)]
    [InlineData("TC,TD,TH,TS,JR", 200)]
    [InlineData("TC,TD,TH,TS,JR,JR", 400)]
    // Joker alone is worth zero
    [InlineData("JR", 0)]
    [InlineData("JR,JR", 0)]
    public void T0(string input, int expectedScore)
    {
        // Arrange
        Assert.True(CsvCardParser.TryParseMany(input, out var cards));
        Assert.Equal(HandValidationResult.Valid, HandValidator.Validate(cards));

        // Act
        var result = ScoreCalculator.Calculate(cards);

        // Assert
        Assert.Equal(expectedScore, result);
    }
}
