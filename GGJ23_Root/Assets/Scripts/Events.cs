using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events Instance;


    public Action<Game> onGameLoaded;
    public Action<Game> onGameGameStarted;
    public Action<Game> onGameEnded;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}