using System;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public int stages;
    protected int currentStage;
    public Action<int, int> OnStageUpdated;
    public Camera gameCam;

    private void Awake()
    {
        gameObject.SetActive(false);
        OnStageUpdated = null;

        Events.Instance.onGameLoaded?.Invoke(this);
    }

    /// <summary>
    /// Called when the scene is loaded. 
    /// Good to get whiteboard values and initialize positions, etc.
    /// </summary>
    protected virtual void SetupGame() { }

    public void StartGame()
    {
        GameManager.Instance.mainCam = gameCam;
        SetupGame();
        gameObject.SetActive(true);
        OnStartGame();
        Events.Instance.onGameGameStarted?.Invoke(this);
    }

    /// <summary>
    /// Called when the previous scene is unloaded and this game should start.
    /// Transition from previous game to this game should be handled here.
    /// </summary>
    protected virtual void OnStartGame() { }

    public void RestartGame()
    {
        Debug.Log("Restart game");
        OnRestartGame();
    }

    /// <summary>
    /// Called when the player loses and the game restarts
    /// </summary>
    protected virtual void OnRestartGame() { }

    public void StageUpdate()
    {
        currentStage++;
        OnUpdateStage();
        OnStageUpdated?.Invoke(currentStage, stages);
    }
    protected virtual void OnUpdateStage() { }


    public void FinishGame()
    {
        OnFinishGame();
        Events.Instance.onGameEnded?.Invoke(this);
    }

    /// <summary>
    /// Called when the game ends. 
    /// Writing values to the Whiteboard should be handled here.
    /// </summary>
    protected virtual void OnFinishGame() { }

    public void CloseGame()
    {
        gameObject.SetActive(false);
        OnGameUnloaded();
    }


    /// <summary>
    /// This is called when the game is closed, it's meant to set things when removing the game after it's disabled
    /// </summary>
    protected virtual void OnGameUnloaded() { }
}