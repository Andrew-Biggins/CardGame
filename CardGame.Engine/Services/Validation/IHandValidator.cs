using CardGame.Engine.Model;

namespace CardGame.Engine.Services.Validation;
internal interface IHandValidator
{
    HandValidationResult Validate(IReadOnlyList<Card> cards);
}
