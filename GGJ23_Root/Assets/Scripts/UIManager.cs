using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject countdown;
    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple UI Managers");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ShowCountdown(bool show)
    {
        //Debug.Log("Show Countdown " + show);
        countdown.gameObject.SetActive(show);
    }
    public void ShowLoadingScreen(bool show)
    {
        //Debug.Log("Show Countdown " + show);
        loadingScreen.gameObject.SetActive(show);
    }

    private void OnCountdownOver()
    {
        ShowCountdown(false);
    }
}
