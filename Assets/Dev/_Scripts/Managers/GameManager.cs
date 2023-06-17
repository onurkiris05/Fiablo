using System;

public class GameManager : Singleton<GameManager>
{
    public event Action<GameState> OnGameStateChange;
    public GameState State { get; private set; }

    private void Start() => InvokeOnStateChange(GameState.Start);

    public void InvokeOnStateChange(GameState state)
    {
        if (State == state) return;
        State = state;
        print($"Game State Changed to {state}");

        OnGameStateChange?.Invoke(state);
    }
}

[Serializable]
public enum GameState
{
    Start = 0,
    Moving = 1,
    Attacking = 2,
    Idle = 3,
    Died = 4
}