using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Mode
{
    Debug,
    Release
}

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Mode Mode = Mode.Release;
    public string[] scenesNames;

    public static bool GamePlaying { get; set; }

    private AsyncSceneLoader loader = new AsyncSceneLoader();

    public int currentGameId = 0;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Multiple Game Managers");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private IEnumerator Start()
    {
        Events.Instance.onGameEnded += OnGameEnded;

        loader.LoadSceneAsync(scenesNames[currentGameId]);
        while (!loader.ready)
        {
            yield return null;
        }

        loader.ShowScene();

        loader.LoadSceneAsync(scenesNames[currentGameId + 1]);
    }

    private void OnGameEnded(Game obj)
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        while (loader.ready == false)
            yield return null;

        //show scene already loaded
        loader.ShowScene();

        SceneManager.UnloadScene(scenesNames[currentGameId]);

        currentGameId++;

        //if the last scene return
        if (currentGameId + 1 >= scenesNames.Length)
            yield break;

        //start loading next scene
        loader.LoadSceneAsync(scenesNames[currentGameId]);
    }
}

public class AsyncSceneLoader
{
    public bool ready { get; set; }
    public AsyncOperation operation { get; private set; }

    public void LoadSceneAsync(string sceneName)
    {
        GameManager.Instance.StartCoroutine(LoadSceneAsyncRoutine(sceneName));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Check if the load has finished
            if (operation.progress >= 0.9f)
            {
                ready = true;
                yield break;
            }

            yield return null;
        }
    }

    public void ShowScene()
    {
        operation.allowSceneActivation = true;
        operation = null;
        ready = false;
    }
}