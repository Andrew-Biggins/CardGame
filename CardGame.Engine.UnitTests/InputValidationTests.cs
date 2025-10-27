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
    public void T0(string input)
    {
        var isValid = _validator.Validate(input);
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
    public void T1(string input)
    {
        // Act
        var isValid = _validator.Validate(input);

        // Assert
        Assert.False(isValid);
    }
}
