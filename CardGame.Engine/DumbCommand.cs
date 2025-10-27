using System.Windows.Input;

namespace CardGame.Engine;

// Minimal ICommand implementation that always can execute and runs the provided action.
public sealed class DumbCommand(Action action) : ICommand
{
    public event EventHandler? CanExecuteChanged { add { } remove { } }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _action();

    private readonly Action _action = action ?? throw new ArgumentNullException(nameof(action));
}