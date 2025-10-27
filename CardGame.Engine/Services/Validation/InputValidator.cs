using System.Text.RegularExpressions;

namespace CardGame.Engine.Services.Validation;
internal class InputValidator : IInputValidator
{
    public bool Validate(string input)
    {
        if(string.IsNullOrWhiteSpace(input))
            return false;

        // Allow only alphanumerics, spaces and commas, but reject alnum + whitespace + alnum (e.g. "2D 3D")
        return Regex.IsMatch(input, @"^(?!.*[A-Za-z0-9]\s+[A-Za-z0-9])[A-Za-z0-9\s,]+$");
    }
}
