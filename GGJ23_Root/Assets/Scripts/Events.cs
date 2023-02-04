using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events Instance;


    public Action<Game> onGameLoaded;

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