using CardGame.Engine.Services.Calculations;
using CardGame.Engine.Services.Parsing;
using CardGame.Engine.Services.Validation;

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
    // Full deck with two jokers (max score)
    [InlineData("2C,2D,2H,2S,3C,3D,3H,3S,4C,4D,4H,4S,5C,5D,5H,5S,6C,6D,6H,6S,7C,7D,7H,7S,8C,8D,8H,8S,9C,9D,9H,9S,TC,TD,TH,TS,JC,JD,JH,JS,QC,QD,QH,QS,KC,KD,KH,KS,AC,AD,AH,AS,JR,JR", 4160)]
    public void T0(string input, int expectedScore)
    {
        // Arrange
        var parser = new CsvCardParser();
        var validator = new HandValidator();
        Assert.True(parser.TryParseMany(input, out var cards));
        Assert.Equal(HandValidationResult.Valid, validator.Validate(cards));
        var calculator = new ScoreCalculator();

        // Act
        var result = calculator.Calculate(cards);

        // Assert
        Assert.Equal(expectedScore, result);
    }
}
