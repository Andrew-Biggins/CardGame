using CardGame.Engine.Services.Calculations;
using CardGame.Engine.Services.Parsing;
using CardGame.Engine.Services.Validation;

namespace CardGame.Engine;

internal class GameEngine(IInputValidator inputValidator, ICsvCardParser parser, IHandValidator handValidator, IScoreCalculator scoreCalculator)
{
    private readonly IInputValidator _inputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
    private readonly ICsvCardParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    private readonly IHandValidator _handValidator = handValidator ?? throw new ArgumentNullException(nameof(handValidator));
    private readonly IScoreCalculator _scoreCalculator = scoreCalculator ?? throw new ArgumentNullException(nameof(scoreCalculator));

    internal GameEngine() : this(new InputValidator(), new CsvCardParser(), new HandValidator(), new ScoreCalculator()) { }

    public ScoreResult Compute(string input)
    {
        if (!_inputValidator.Validate(input))
            return ScoreResult.Fail(ErrorMessages.InvalidInput);

        if (!_parser.TryParseMany(input, out var cards))
            return ScoreResult.Fail(ErrorMessages.CardNotRecognised);

        var validation = _handValidator.Validate(cards);
        if (validation != HandValidationResult.Valid)
        {
            var msg = validation switch
            {
                HandValidationResult.InvalidDuplicates => ErrorMessages.DuplicateCards,
                HandValidationResult.InvalidTooManyJokers => ErrorMessages.TooManyJokers,
                _ => ErrorMessages.CardNotRecognised
            };

            return ScoreResult.Fail(msg);
        }

        var score = _scoreCalculator.Calculate(cards);
        return ScoreResult.Success(score);
    }
}

public readonly record struct ScoreResult(bool IsSuccessful, int Score, string Error = "")
{
    public static ScoreResult Success(int score) => new(true, score);
    public static ScoreResult Fail(string error) => new(false, 0, error);
}
