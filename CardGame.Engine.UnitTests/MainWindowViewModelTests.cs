using NSubstitute;

namespace CardGame.Engine.UnitTests;

public class MainWindowViewModelTests
{
    [Gwt("Given a MainWindow VM with a valid input",
        "when the compute score command is executed",
        "then the score is updated and the error is empty")]
    public void T0()
    {
        // Arrange
        var vm = new MainWindowViewModel(TestGameEngineValid);

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Equal(DummyScore, vm.Score);
        Assert.True(vm.HasScore);
        Assert.Equal(string.Empty, vm.Error);
    }

    [Gwt("Given a MainWindow VM with an invalid input",
        "when the compute score command is executed",
        "then the score is 0 and the error is invalid input")]
    public void T1()
    {
        // Arrange
        var vm = new MainWindowViewModel(TestGameEngineInvalid);

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Equal(0, vm.Score);
        Assert.False(vm.HasScore);
        Assert.Equal(ErrorMessages.InvalidInput, vm.Error);
    }

    [Gwt("Given a MainWindow VM with a valid card input",
        "when the compute score command is executed",
        "then property changed is raised for score and hasError")]
    public void T2()
    {
        // Arrange       
        var vm = new MainWindowViewModel(TestGameEngineValid);
        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Contains(nameof(vm.Score), raised);
        Assert.Contains(nameof(vm.HasScore), raised);
    }

    [Gwt("Given a MainWindow VM with a valid card input",
        "when the same input is set again",
        "then porperty changed is not raised for input")]
    public void T3()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        vm.Input = "2C";

        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        // Act
        vm.Input = "2C";

        // Assert
        Assert.DoesNotContain(nameof(vm.Input), raised);
    }

    [Gwt("Given a MainWindow VM with a computed score",
        "when the compute score command is executed with the same score",
        "then property changed is not raised for score")]
    public void T4()
    {
        // Arrange       
        var vm = new MainWindowViewModel(TestGameEngineValid);
        vm.ComputeScoreCommand.Execute(null); 

        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.DoesNotContain(nameof(vm.Score), raised);
    }

    [Gwt("Given a MainWindow VM with an error",
        "when the compute score command is executed with the same invalid input",
        "then property changed is not raised for Error")]
    public void T5()
    {
        // Arrange
        var vm = new MainWindowViewModel(TestGameEngineInvalid);
        vm.ComputeScoreCommand.Execute(null); 

        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        // Act 
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.DoesNotContain(nameof(vm.Error), raised);
    }

    [Gwt("Given a MainWindow VM with an initial error",
        "when the input is corrected and a successful compute occurs",
        "then rrror is cleared, score is updated and notifications are raised")]
    public void T6()
    {
        // Arrange
        var engine = Substitute.For<IGameEngine>();
        engine.Compute(Arg.Any<string>()).Returns(
            ScoreResult.Fail(ErrorMessages.CardNotRecognised),
            ScoreResult.Success(DummyScore)
        );

        var vm = new MainWindowViewModel(engine);

        // Act 
        vm.ComputeScoreCommand.Execute(null);
        Assert.Equal(ErrorMessages.CardNotRecognised, vm.Error);
        Assert.Equal(0, vm.Score);

        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Equal(DummyScore, vm.Score);
        Assert.Equal(string.Empty, vm.Error);
        Assert.True(vm.HasScore);

        Assert.Contains(nameof(vm.Score), raised);
        Assert.Contains(nameof(vm.Error), raised);
        Assert.Contains(nameof(vm.HasScore), raised);
    }

    [Gwt("Given a MainWindow VM with an initial valid score",
        "when the input becomes invalid and compute fails",
        "then score is cleared, error is set and notifications are raised")]
    public void T7()
    {
        // Arrange
        var engine = Substitute.For<IGameEngine>();
        engine.Compute(Arg.Any<string>()).Returns(
            ScoreResult.Success(DummyScore),
            ScoreResult.Fail(ErrorMessages.CardNotRecognised)
        );

        var vm = new MainWindowViewModel(engine);

        // Act
        vm.ComputeScoreCommand.Execute(null);
        Assert.Equal(DummyScore, vm.Score);
        Assert.True(vm.HasScore);

        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        vm.ComputeScoreCommand.Execute(null);

        // Assert 
        Assert.Equal(0, vm.Score);
        Assert.Equal(ErrorMessages.CardNotRecognised, vm.Error);
        Assert.False(vm.HasScore);

        Assert.Contains(nameof(vm.Score), raised);
        Assert.Contains(nameof(vm.Error), raised);
        Assert.Contains(nameof(vm.HasScore), raised);
    }

    [Gwt("Given a MainWindow VM",
       "when contructed with a null engine",
       "then the an argument null exception is thrown")]
    public void T8()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(null!));
        Assert.Equal("engine", ex.ParamName);
    }

    private static IGameEngine TestGameEngineValid
    {
        get
        {
            var engine = Substitute.For<IGameEngine>();
            engine.Compute(Arg.Any<string>()).Returns(ScoreResult.Success(DummyScore));
            return engine;
        }
    }

    private static IGameEngine TestGameEngineInvalid
    {
        get
        {
            var engine = Substitute.For<IGameEngine>();
            engine.Compute(Arg.Any<string>()).Returns(ScoreResult.Fail(ErrorMessages.InvalidInput));
            return engine;
        }
    }

    private const int DummyScore = 42;
}
