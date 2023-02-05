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
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}