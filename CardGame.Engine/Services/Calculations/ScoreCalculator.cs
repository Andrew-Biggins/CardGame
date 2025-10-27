using CardGame.Engine.Model;

namespace CardGame.Engine.Services.Calculations;

internal class ScoreCalculator : IScoreCalculator
{
    public int Calculate(IReadOnlyList<Card> cards)
    {
        // Separate Jokers
        int jokerCount = cards.Count(c => c.Value == CardValue.Joker);
        var nonJokers = cards.Where(c => c.Value != CardValue.Joker);

        // Sum card values
        int total = nonJokers.Sum(c => (int)c.Value * SuitMultiplier(c.Suit));

        // Double once per Joker (validator already caps at 2, but be defensive)
        if (jokerCount > 0)
        {
            int factor = 1 << Math.Min(jokerCount, 2); // 1, 2, or 4
            total *= factor;
        }

        return total;
    }

    private static int SuitMultiplier(CardSuit suit) => suit switch
    {
        CardSuit.Clubs => 1,
        CardSuit.Diamonds => 2,
        CardSuit.Hearts => 3,
        CardSuit.Spades => 4,
        CardSuit.None => 0, // None (for Joker)
        _ => throw new NotImplementedException()
    };
}
