namespace CardGame.Engine.UnitTests;

public class DumbCommandTests
{
    [Gwt("Given a null action",
        "when created",
        "an exception is thrown")]
    public void T0()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => new DumbCommand(null!));
        Assert.Equal("action", ex.ParamName);
    }

    [Gwt("Given a command",
        "when checked to see if it can execute with a null",
        "true is returned")]
    public void T1()
    {
        // Arrange
        var command = new DumbCommand(() => { });

        // Act
        var canExecute = command.CanExecute(null!);

        // Assert
        Assert.True(canExecute);
    }

    [Gwt("Given a command",
        "when executed",
        "the action is invoked")]
    public void T2()
    {
        // Arrange
        var invoked = false;
        var command = new DumbCommand(() => invoked = true);

        // Act
        command.Execute(new object());

        // Assert
        Assert.True(invoked);
    }
}