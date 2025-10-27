using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGame.Engine.UnitTests;
public class MainWindowViewModelTests
{
    [Gwt("Given a MainWindow VM with a valid input",
        "when the compute score command is executed",
        "then the score is updated and the error is empty")]
    public void T0()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        vm.Input = "2C"; 

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Equal(2, vm.Score);
        Assert.True(vm.HasScore);
        Assert.Equal(string.Empty, vm.Error);
    }

    [Gwt("Given a MainWindow VM with an invalid input",
        "when the compute score command is executed",
        "then the score is 0 and the error is invalid input")]
    public void T1()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        vm.Input = "2S 3D"; // space used as separator -> invalid input

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Equal(0, vm.Score);
        Assert.False(vm.HasScore);
        Assert.Equal(ErrorMessages.InvalidInput, vm.Error);
    }

    [Gwt("Given a MainWindow VM with an unrecognised card input",
        "when the compute score command is executed",
        "then the score is 0 and the error is unrecognised card")]
    public void T2()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        vm.Input = "1S"; 

        // Act
        vm.ComputeScoreCommand.Execute(null);

        // Assert
        Assert.Equal(0, vm.Score);
        Assert.False(vm.HasScore);
        Assert.Equal(ErrorMessages.CardNotRecognised, vm.Error);
    }

    [Gwt("Given a MainWindow VM with a valid card input",
        "when the compute score command is executed",
        "then porperty changed is raised for score and hasError")]
    public void T3()
    {
        // Arrange
        var vm = new MainWindowViewModel();
        var raised = new List<string>();
        vm.PropertyChanged += (_, e) => raised.Add(e.PropertyName);

        vm.Input = "2C";

        // Act
        vm.ComputeScoreCommand.Execute(null);

        Assert.Contains(nameof(vm.Score), raised);
        Assert.Contains(nameof(vm.HasScore), raised);
    }
}
