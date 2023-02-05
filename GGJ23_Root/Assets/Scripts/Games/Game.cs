using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class Game : MonoBehaviour
{
    public Volume volume;
    public int stages;
    protected int currentStage;
    public Action<int, int> OnStageUpdated;
    public Camera gameCam;
    private ChromaticAberration bloom;
    private ChromaticAberration chromaticAberration;

    private void Awake()
    {
        gameObject.SetActive(false);
        OnStageUpdated = null;

        Events.Instance.onGameLoaded?.Invoke(this);
    }

    protected virtual void Start()
    {
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out chromaticAberration);
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

    Coroutine chromaticAberrationRoutine = null;
    public void ShowChromaticAberration(float strength, float duration)
    {
        if (chromaticAberration == null) return;

        if (chromaticAberrationRoutine != null) StopCoroutine(chromaticAberrationRoutine);
        chromaticAberrationRoutine = StartCoroutine(ShowChromaticAberrationCR(strength, duration));
    }

    public IEnumerator ShowChromaticAberrationCR(float strength, float duration)
    {
        Debug.Log("should show CHROMATIC ABERRATION");
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = strength;

        float percentTime = 0;
        float elapsedTime = 0;
        float intensity = strength;

        while (percentTime < 1)
        {
            elapsedTime += Time.deltaTime;
            percentTime = elapsedTime / duration;
            if (percentTime > 1) percentTime = 1;
            chromaticAberration.intensity.value = Mathf.Lerp(strength, 0, percentTime);
            yield return null;
        }

        chromaticAberrationRoutine = null;
    }

}