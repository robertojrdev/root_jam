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



    [SerializeField] private Camera camera;
    public Camera Camera { get => camera; }

    public static bool GamePlaying { get; set; }



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
        var loadOperation = SceneManager.LoadSceneAsync("Pong Scene", LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;
        yield return loadOperation;
        loadOperation.allowSceneActivation = true;
        bool ready = false;

        while (!loadOperation.isDone)
        {
            // Check if the load has finished
            if (loadOperation.progress >= 0.9f)
            {
                print("SCNEE READY");
                ready = true;
                break;
            }

            if(!ready)
                yield return null;
        }

        while (Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

        print("Called");
    }
}