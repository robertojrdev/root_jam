using System;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public int stages;
    protected int currentStage;
    public Action OnStageUpdated;

    private void Awake()
    {
        SetupGame();
        StartGame();
    }

    protected virtual void SetupGame() { }

    public void StartGame()
    {
        OnStartGame();
        Events.Instance.onGameGameStarted?.Invoke(this);
    }
    protected virtual void OnStartGame() { }

    public void StageUpdate()
    {
        currentStage++;
        OnUpdateStage();
        OnStageUpdated?.Invoke();
    }
    protected virtual void OnUpdateStage() { }


    public void FinishGame()
    {
        OnFinishGame();
        Events.Instance.onGameEnded?.Invoke(this);
    }
    protected virtual void OnFinishGame() { }
}