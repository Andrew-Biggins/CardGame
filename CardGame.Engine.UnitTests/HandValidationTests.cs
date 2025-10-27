using CardGame.Engine.Services;
using CardGame.Engine.Services.Parsing;
using CardGame.Engine.Services.Validation;

namespace CardGame.Engine.UnitTests;

public class HandValidationTests
{
    [GwtTheory("Given a hand validator",
           "when validating a list of cards",
           "then the correct validation result is returned")]
    [InlineData("2C,3D,4H", HandValidationResult.Valid)]
    [InlineData("2C,3D,2C", HandValidationResult.InvalidDuplicates)]
    [InlineData("2C,3D,2C,3D", HandValidationResult.InvalidDuplicates)]
    [InlineData("JR,JR,2C", HandValidationResult.Valid)]
    [InlineData("JR,JR,JR", HandValidationResult.InvalidTooManyJokers)]
    public void T0(string input, HandValidationResult expected)
    {
        // Arrange
        var parser = new CsvCardParser();
        Assert.True(parser.TryParseMany(input, out var cards));
        var validator = new HandValidator();

        // Act
        var result = validator.Validate(cards);

        // Assert
        Assert.Equal(expected, result);
    }
}
