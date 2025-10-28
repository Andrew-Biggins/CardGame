using System.Text.RegularExpressions;

namespace CardGame.Engine.Services.Validation;
internal class InputValidator : IInputValidator
{
    // Basic allowed-chars check (letters/digits/space/comma)
    private static readonly Regex _charCheck = new(@"^[A-Za-z0-9\s,]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    // Disallow alnum + whitespace + alnum (space used as separator)
    private static readonly Regex _spaceSeparatorCheck = new(@"[A-Za-z0-9]\s+[A-Za-z0-9]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex _tokenCheck = new(@"^[A-Za-z0-9]{2}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public bool Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || !_charCheck.IsMatch(input) || _spaceSeparatorCheck.IsMatch(input))
            return false;

        // Split on comma and validate each trimmed token shape
        var parts = input.Split(',');
        foreach (var part in parts)
        {
            var token = part.Trim();
            if (string.IsNullOrEmpty(token) || !_tokenCheck.IsMatch(token))
                return false; 
        }

        return true;
    }
}