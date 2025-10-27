using CardGame.Engine.Model;

namespace CardGame.Engine.Services.Parsing;

internal interface ICsvCardParser
{
    bool TryParseMany(string input, out List<Card> cards);
}