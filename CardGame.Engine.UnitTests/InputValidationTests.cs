using CardGame.Engine.Services.Validation;

namespace CardGame.Engine.UnitTests;

public class InputValidationTests
{
    private readonly InputValidator _validator = new();

    [GwtTheory("Given an input validator",
        "when validating a valid top-level input syntax",
        "then true is returned")]
    [InlineData("2S, 3D")]
    [InlineData("2s,3d")]
    [InlineData("JR,2C")]
    [InlineData("2C")]
    [InlineData("2S ,3D")]
    [InlineData("ZZ ,3D")]
    [InlineData("AB ,CD, EF, GH, IJ, KL, MN, OP, QR, ST, UV, WX, YZ, 01, 23, 45, 67, 89")]
    public void T0(string input)
    {
        // Act
        var isValid = _validator.Validate(input);

        // Assert
        Assert.True(isValid);
    }

    [GwtTheory("Given an input validator",
        "when validating an invalid top-level input syntax",
        "then false")]
    [InlineData("")]               // empty
    [InlineData(" ")]              // whitespace
    [InlineData(null)]             // null
    // Bad separators
    [InlineData("2S|3D")]
    [InlineData("2S;3D")]
    [InlineData("2S&3D")]
    [InlineData("2S/3D")]
    [InlineData("2S:3D")]
    [InlineData("2S_3D")]
    // Space/tab/newline separators or space used as separator
    [InlineData("2S 3D")]
    [InlineData("2S\t3D")]
    [InlineData("2S\n3D")]
    [InlineData("JR JR")]
    // Rogue commas
    [InlineData(",2S")]
    [InlineData("2S,")]
    [InlineData("2S,,3D")]
    [InlineData("2S, ,3D")]
    // Malformed cards
    [InlineData("TOO, LONG")]
    [InlineData("2C, THX")]       // too long
    [InlineData("S")]             // too short  
    [InlineData("TC, S")]         // one too short
    public void T1(string input)
    {
        // Act
        var isValid = _validator.Validate(input);

        // Assert
        Assert.False(isValid);
    }

    [Gwt("Given an input validator",
     "when validating a very long but well-formed list",
     "then true is returned")]
    public void T2()
    {
        // Arrange
        var longList = string.Join(",", Enumerable.Repeat("AB", 5_000));

        // Act & Assert
        Assert.True(_validator.Validate(longList));
    }
}
