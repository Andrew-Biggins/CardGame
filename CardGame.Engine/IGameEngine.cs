namespace CardGame.Engine;

public interface IGameEngine
{
    ScoreResult Compute(string? input);
}