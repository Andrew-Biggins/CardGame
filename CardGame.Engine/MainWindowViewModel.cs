using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CardGame.Engine;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IGameEngine _engine;

    private string _input = string.Empty;
    private int _score;
    private string _error = string.Empty;

    public MainWindowViewModel() : this(new GameEngine()) { }

    internal MainWindowViewModel(IGameEngine engine)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        ComputeScoreCommand = new DumbCommand(ComputeScore);
    }

    public string Input
    {
        get => _input;
        set => SetProperty(ref _input, value);
    }

    public int Score
    {
        get => _score;
        private set => SetProperty(ref _score, value);
    }

    public string Error
    {
        get => _error;
        private set => SetProperty(ref _error, value);
    }

    public ICommand ComputeScoreCommand { get; }

    public bool HasScore => string.IsNullOrEmpty(Error);

    private void ComputeScore()
    {
        var result = _engine.Compute(Input);

        if (result.IsSuccessful)
        {
            Score = result.Score;
            Error = string.Empty;
        }
        else
        {
            Score = 0;
            Error = result.Error ?? string.Empty;
        }

        OnPropertyChanged(nameof(HasScore));
    }

    private void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = "")
    { 
        if (!Equals(member, value))
        {
            member = value;
            OnPropertyChanged(propertyName);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
