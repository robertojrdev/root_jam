using System;
using System.Collections;
using System.Collections.Generic;
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

    private Dictionary<string, Game> loadedGames = new Dictionary<string, Game>();
    private int currentGameId = -1;

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
        Events.Instance.onGameLoaded += OnGameLoaded;
        Events.Instance.onGameEnded += OnGameEnded;

        //loading all scenes
        for (int i = 0; i < scenesNames.Length; i++)
        {
            yield return loader.LoadSceneAsyncRoutine(scenesNames[i], true);
        }

        LoadNextGame();
    }

    private void OnGameLoaded(Game game)
    {
        loadedGames.Add(game.gameObject.scene.name, game);
    }

    private void OnGameEnded(Game obj)
    {
        LoadNextGame();
    }

    private void LoadNextGame()
    {
        if(currentGameId != -1)
        {
            var currentGame = loadedGames[scenesNames[currentGameId]];
            currentGame.CloseGame();
        }

        currentGameId = ++currentGameId % scenesNames.Length;
        var gameToLoad = loadedGames[scenesNames[currentGameId]];

        gameToLoad.StartGame();
    }
}

public class AsyncSceneLoader
{
    public bool ready { get; set; }
    public AsyncOperation operation { get; private set; }
    private string sceneName;

    public void LoadSceneAsync(string sceneName)
    {
        GameManager.Instance.StartCoroutine(LoadSceneAsyncRoutine(sceneName));
    }

    public IEnumerator LoadSceneAsyncRoutine(string sceneName, bool showOnLoad = false)
    {
        this.sceneName = sceneName;
        operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Check if the load has finished
            if (operation.progress >= 0.9f)
            {

                ready = true;

                if (showOnLoad)
                    yield return ShowScene();

                yield break;
            }

            yield return null;
        }
    }

    public IEnumerator ShowScene()
    {
        operation.allowSceneActivation = true;
        Debug.Log("SHOWING");

        while (operation.isDone == false)
            yield return null;

        Debug.Log("SHOWING-DONE");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        operation = null;
        ready = false;
    }
}