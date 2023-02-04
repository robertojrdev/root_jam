using System;
using UnityEngine;

public abstract class Game
{
    public int stages;
    protected int currentStage;
    public Action OnGameStarted;
    public Action OnStageUpdate;
    public Action OnGameEnded;

    public virtual void StartGame(Game previousGame)
    {
        OnGameStarted?.Invoke();
    }

    public virtual void StageUpdate()
    {
        currentStage++;
        OnStageUpdate?.Invoke();
    }

    public virtual void FinishGame()
    {
        OnGameEnded?.Invoke();
    }
}