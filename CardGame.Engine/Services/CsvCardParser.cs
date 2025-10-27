using CardGame.Engine.Model;

namespace CardGame.Engine.Services;

internal sealed class CsvCardParser
{
    public static bool TryParseMany(string input, out List<Card> cards)
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

    public static bool TryParseCard(string input, out Card card)
    {
        card = new Card(CardRank.None, CardSuit.None); // Placeholder for unrecognised card

        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
            return false;

        input = input.Trim().ToUpperInvariant();

        if (input.Length != 2) return false;

        if (input == "JR")
        {
            card = new Card(CardRank.Joker, CardSuit.None);
            return true;
        }

        var rankCode = input[0];
        var suitCode = input[1];

        var rank = rankCode switch
        {
            '2' => CardRank.Two,
            '3' => CardRank.Three,
            '4' => CardRank.Four,
            '5' => CardRank.Five,
            '6' => CardRank.Six,
            '7' => CardRank.Seven,
            '8' => CardRank.Eight,
            '9' => CardRank.Nine,
            'T' => CardRank.Ten,
            'J' => CardRank.Jack,
            'Q' => CardRank.Queen,
            'K' => CardRank.King,
            'A' => CardRank.Ace,
            _ => CardRank.None 
        };

        if (rank == CardRank.None) return false;

        var suit = suitCode switch
        {
            'H' => CardSuit.Hearts,
            'D' => CardSuit.Diamonds,
            'C' => CardSuit.Clubs,
            'S' => CardSuit.Spades,
            _ => CardSuit.None
        };

        if (suit == CardSuit.None) return false;

        card = new Card(rank, suit);

        return true;
    }
}