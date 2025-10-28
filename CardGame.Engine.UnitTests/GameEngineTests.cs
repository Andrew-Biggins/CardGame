using CardGame.Engine.Model;
using CardGame.Engine.Services.Calculations;
using CardGame.Engine.Services.Parsing;
using CardGame.Engine.Services.Validation;
using NSubstitute;

namespace CardGame.Engine.UnitTests;

public class GameEngineTests
{
    [Gwt("Given a game engine",
        "when an invalid input string is computed",
        "then the result is unsuccessful with the correct invalid input error message")]
    public void T0()
    {
        // Arrange
        var sut = TestGameEngine;
        SubInputValidator.Validate(Arg.Any<string>()).Returns(false);

        // Act
        var result = sut.Compute("Invalid input"); // value irrelevant, substitutes use Arg.Any<string>())

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.InvalidInput, result.Error);
    }

    [Gwt("Given a game engine",
        "when an input string with an unrecognised card is computed",
        "then the result is unsuccessful with the correct card not recognised error message")]
    public void T1()
    {
        // Arrange
        var sut = TestGameEngine;
        SubCsvCardParser.TryParseMany(Arg.Any<string>(), out Arg.Any<List<Card>>()).Returns(callInfo =>
        {
            callInfo[1] = new List<Card>();
            return false;
        });

        // Act
        var result = sut.Compute("ZZ");

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.CardNotRecognised, result.Error);
    }

    [Gwt("Given a game engine",
        "when an input string with duplicate cards is computed",
        "then the result is unsuccessful with the correct duplicate cards error message")]
    public void T2()
    {
        // Arrange
        var sut = TestGameEngine;
        SubHandValidator.Validate(Arg.Any<List<Card>>()).Returns(HandValidationResult.InvalidDuplicates);

        // Act
        var result = sut.Compute("2C, 2C");

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.DuplicateCards, result.Error);
    }

    [Gwt("Given a game engine",
        "when an input string with too many jokers is computed",
        "then the result is unsuccessful with the correct too many jokers error message")]
    public void T3()
    {
        // Arrange
        var sut = TestGameEngine;
        SubHandValidator.Validate(Arg.Any<List<Card>>()).Returns(HandValidationResult.InvalidTooManyJokers);

        // Act
        var result = sut.Compute("JR, JR, JR");

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.TooManyJokers, result.Error);
    }

    [Gwt("Given a game engine",
        "when a valid hand is computed",
        "then the result is successful with the correct score")]
    public void T4()
    {
        // Arrange
        var sut = TestGameEngine;

        // Act
        var result = sut.Compute("TS, 2C");

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(DummyScore, result.Score);
    }

    [Gwt("Given a game engine",
        "when a valid hand is computed",
        "then the validators, parser and calculator are all called in the correct order")]
    public void T5()
    {
        // Arrange
        var sut = TestGameEngine;

        // Act
        var result = sut.Compute("TS, 2C");

        // Assert
        Received.InOrder(() =>
        {
            SubInputValidator.Received(1).Validate(Arg.Any<string>());
            _ = SubCsvCardParser.Received(1).TryParseMany(Arg.Any<string>(), out Arg.Any<List<Card>>());
            SubHandValidator.Received(1).Validate(Arg.Any<List<Card>>());
            SubScoreCalculator.Received(1).Calculate(Arg.Any<IReadOnlyList<Card>>());
        });
    }

    [Gwt("Given a game engine",
        "when an invalid input is computed",
        "then the parser, hand validator and calculator are not called")]
    public void T6()
    {
        // Arrange
        var sut = TestGameEngine;
        SubInputValidator.Validate(Arg.Any<string>()).Returns(false);

        // Act
        _ = sut.Compute("Invalid input");

        // Assert
        _ = SubCsvCardParser.DidNotReceive().TryParseMany(Arg.Any<string>(), out Arg.Any<List<Card>>());
        SubHandValidator.DidNotReceive().Validate(Arg.Any<List<Card>>());
        SubScoreCalculator.DidNotReceive().Calculate(Arg.Any<IReadOnlyList<Card>>());
    }

    [Gwt("Given a game engine",
        "when an unrecognised card is computed",
        "then the hand validator and calculator are not called")]
    public void T7()
    {
        // Arrange
        var sut = TestGameEngine;
        SubCsvCardParser.TryParseMany(Arg.Any<string>(), out Arg.Any<List<Card>>()).Returns(callInfo =>
        {
            callInfo[1] = new List<Card>();
            return false;
        });

        // Act
        _ = sut.Compute("ZZ");

        // Assert
        SubHandValidator.DidNotReceive().Validate(Arg.Any<List<Card>>());
        SubScoreCalculator.DidNotReceive().Calculate(Arg.Any<IReadOnlyList<Card>>());
    }

    [Gwt("Given a game engine",
        "when an invalid hand is computed",
        "then the calculator is not called")]
    public void T8()
    {
        // Arrange
        var sut = TestGameEngine;
        SubHandValidator.Validate(Arg.Any<IReadOnlyList<Card>>()).Returns(HandValidationResult.InvalidDuplicates);

        // Act
        _ = sut.Compute("2C, 2C");

        // Assert
        SubScoreCalculator.DidNotReceive().Calculate(Arg.Any<IReadOnlyList<Card>>());
    }

    [Gwt("Given a game engine",
        "when a valid hand is computed",
        "then the calculator is passed the correct card")]
    public void T9()
    {
        // Arrange
        var sut = TestGameEngine;

        // Act
        _ = sut.Compute("TS, 2C");

        // Assert
        SubScoreCalculator.Received(1).Calculate(Arg.Is<IReadOnlyList<Card>>(list =>
           list.Count == 2 &&
           list[0].Value == CardValue.Ten && list[0].Suit == CardSuit.Spades &&
           list[1].Value == CardValue.Two && list[1].Suit == CardSuit.Clubs));
    }

    [Gwt("Given a game engine",
        "when a null input is computed",
        "then the result is unsuccessful and downstream components are not called")]
    public void T10()
    {
        // Arrange
        var sut = TestGameEngine;
        SubInputValidator.Validate(Arg.Any<string>()).Returns(false);

        // Act
        var result = sut.Compute(null!);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.InvalidInput, result.Error);
        _ = SubCsvCardParser.DidNotReceive().TryParseMany(Arg.Any<string>(), out Arg.Any<List<Card>>());
        SubHandValidator.DidNotReceive().Validate(Arg.Any<List<Card>>());
        SubScoreCalculator.DidNotReceive().Calculate(Arg.Any<IReadOnlyList<Card>>());
    }

    [Gwt("Given a game engine",
        "when contructed with a null input validator",
        "then the an argument null exception is thrown")]
    public void T11()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new GameEngine(null!, SubCsvCardParser, SubHandValidator, SubScoreCalculator));
        Assert.Equal("inputValidator", ex.ParamName);
    }

    [Gwt("Given a game engine",
        "when contructed with a null parser",
        "then the an argument null exception is thrown")]
    public void T12()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new GameEngine(SubInputValidator, null!, SubHandValidator, SubScoreCalculator));
        Assert.Equal("parser", ex.ParamName);
    }

    [Gwt("Given a game engine",
        "when contructed with a null hand validator",
        "then the an argument null exception is thrown")]
    public void T13()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new GameEngine(SubInputValidator, SubCsvCardParser, null!, SubScoreCalculator));
        Assert.Equal("handValidator", ex.ParamName);
    }

    [Gwt("Given a game engine",
        "when contructed with a null calculator",
        "then the an argument null exception is thrown")]
    public void T14()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new GameEngine(SubInputValidator, SubCsvCardParser, SubHandValidator, null!));
        Assert.Equal("scoreCalculator", ex.ParamName);
    }

    private GameEngine TestGameEngine
    {
        get
        {
            SubInputValidator.Validate(Arg.Any<string>()).Returns(true);
            SubCsvCardParser.TryParseMany(Arg.Any<string>(), out Arg.Any<List<Card>>()).Returns(callInfo =>
            {
                callInfo[1] = _parsedCards;
                return true;
            });

            SubHandValidator.Validate(Arg.Any<List<Card>>()).Returns(HandValidationResult.Valid);
            SubScoreCalculator.Calculate(Arg.Any<IReadOnlyList<Card>>()).Returns(DummyScore);

            return new GameEngine(SubInputValidator, SubCsvCardParser, SubHandValidator, SubScoreCalculator);
        }
    }

    private readonly IInputValidator SubInputValidator = Substitute.For<IInputValidator>();
    private readonly ICsvCardParser SubCsvCardParser = Substitute.For<ICsvCardParser>();
    private readonly IHandValidator SubHandValidator = Substitute.For<IHandValidator>();
    private readonly IScoreCalculator SubScoreCalculator = Substitute.For<IScoreCalculator>();

    private readonly List<Card> _parsedCards = [  new(CardValue.Ten, CardSuit.Spades),
                                                  new(CardValue.Two, CardSuit.Clubs) ];

    private const int DummyScore = 42;
}
