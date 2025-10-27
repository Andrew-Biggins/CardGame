using CardGame.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Engine.UnitTests;

public class HandValidationTests
{
    [GwtTheory("Given a hand validator",
           "when validating a list of cards",
           "then duplicate or invalid hands are detected")]
    [InlineData("2C,3D,4H", HandValidationResult.Valid)]
    [InlineData("2C,3D,2C", HandValidationResult.InvalidDuplicates)]
    [InlineData("2C,3D,2C,3D", HandValidationResult.InvalidDuplicates)]
    [InlineData("JR,JR,2C", HandValidationResult.Valid)]
    [InlineData("JR,JR,JR", HandValidationResult.InvalidTooManyJokers)]
    public void T0(string input, HandValidationResult expected)
    {
        // Arrange
        Assert.True(CsvCardParser.TryParseMany(input, out var cards));

        // Act
        var result = HandValidator.Validate(cards);

        // Assert
        Assert.Equal(expected, result);
    }
}
