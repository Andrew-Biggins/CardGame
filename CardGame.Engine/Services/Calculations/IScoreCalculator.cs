using CardGame.Engine.Model;

namespace CardGame.Engine.Services.Calculations;
internal interface IScoreCalculator
{
    int Calculate(IReadOnlyList<Card> cards);
}
