using CardGame.Engine.Model;

namespace CardGame.Engine.Services.Parsing;

internal class CsvCardParser : ICsvCardParser
{
    public bool TryParseMany(string input, out List<Card> cards)
    {
        cards = [];

        if (string.IsNullOrWhiteSpace(input))
            return false;

        var parts = input.Split(',');
        foreach (var part in parts)
        {
            if (!TryParseCard(part, out var card))
            {
                cards.Clear();
                return false;
            }
            cards.Add(card);
        }

        return true;
    }

    internal static bool TryParseCard(string input, out Card card)
    {
        card = new Card(CardValue.None, CardSuit.None); // Placeholder for unrecognised card

        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
            return false;

        input = input.Trim().ToUpperInvariant();

        if (input.Length != 2) return false; // Should be picked up by earlier checks, but be defensive

        if (input == "JR")
        {
            card = new Card(CardValue.Joker, CardSuit.None);
            return true;
        }

        var valueCode = input[0];
        var suitCode = input[1];

        var cardValue = valueCode switch
        {
            '2' => CardValue.Two,
            '3' => CardValue.Three,
            '4' => CardValue.Four,
            '5' => CardValue.Five,
            '6' => CardValue.Six,
            '7' => CardValue.Seven,
            '8' => CardValue.Eight,
            '9' => CardValue.Nine,
            'T' => CardValue.Ten,
            'J' => CardValue.Jack,
            'Q' => CardValue.Queen,
            'K' => CardValue.King,
            'A' => CardValue.Ace,
            _ => CardValue.None 
        };

        if (cardValue == CardValue.None) return false;

        var suit = suitCode switch
        {
            'H' => CardSuit.Hearts,
            'D' => CardSuit.Diamonds,
            'C' => CardSuit.Clubs,
            'S' => CardSuit.Spades,
            _ => CardSuit.None
        };

        if (suit == CardSuit.None) return false;

        card = new Card(cardValue, suit);

        return true;
    }
}