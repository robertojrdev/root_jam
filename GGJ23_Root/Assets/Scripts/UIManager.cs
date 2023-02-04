using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject countdown;

    public void ShowCountdown(bool show)
    {
        countdown.gameObject.SetActive(show);
    }

    private void OnCountdownOver()
    {
        ShowCountdown(false);
    }
}
