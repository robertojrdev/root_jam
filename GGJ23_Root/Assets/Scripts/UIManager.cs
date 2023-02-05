using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject countdown;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Multiple UI Managers");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void ShowCountdown(bool show)
    {
        Debug.Log("Show Countdown " + show);
        countdown.gameObject.SetActive(show);
    }

    private void OnCountdownOver()
    {
        ShowCountdown(false);
    }
}
