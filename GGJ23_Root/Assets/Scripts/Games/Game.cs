using System;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public int stages;
    protected int currentStage;
    public Action OnGameStarted;
    public Action OnStageUpdated;
    public Action OnGameEnded;

    public void StartGame(Game previousGame)
    {
        OnStartGame(previousGame);
        OnGameStarted?.Invoke();
    }
    protected abstract void OnStartGame(Game previousGame);

    public void StageUpdate()
    {
        currentStage++;
        OnUpdateStage();
        OnStageUpdated?.Invoke();
    }
    protected abstract void OnUpdateStage();


    public void FinishGame()
    {
        OnFinishGame();
        OnGameEnded?.Invoke();
    }
    protected abstract void OnFinishGame();
}