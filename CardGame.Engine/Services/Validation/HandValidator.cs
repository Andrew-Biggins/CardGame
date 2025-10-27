using CardGame.Engine.Model;

namespace CardGame.Engine.Services.Validation;

internal class HandValidator : IHandValidator
{
    public HandValidationResult Validate(IReadOnlyList<Card> cards)
    {
        // Count Jokers first
        var jokerCount = cards.Count(c => c.Value == CardValue.Joker);
        if (jokerCount > 2)
            return HandValidationResult.InvalidTooManyJokers;

        // Check duplicates (ignore Jokers)
        var duplicates = cards
            .Where(c => c.Value != CardValue.Joker)
            .GroupBy(c => new { c.Value, c.Suit })
            .Any(g => g.Count() > 1);

        if (duplicates)
            return HandValidationResult.InvalidDuplicates;

        return HandValidationResult.Valid;
    }
}

public enum HandValidationResult
{
    Valid,
    InvalidDuplicates,
    InvalidTooManyJokers
}
