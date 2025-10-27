namespace CardGame.Engine.Model;

internal readonly record struct Card(CardValue Value, CardSuit Suit);

public enum CardSuit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades,
    None 
}

public enum CardValue
{
    None = 0,
    Joker = 1, // Not used for scoring
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14,
}
