using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerRunnerManager : MonoBehaviour
{
    [Header("State")]
    public bool runner = true;
    public bool walker = false;

    [Header("Transition")]
    [Range(0, 1)]
    public float transitionSlider = 0f;

    [Header("Components")]
    public WalkerCamera wCam;
    public WalkerMovement wMove;
    public GameObject vfx;
    //public Camera cam;

    public void StartWalker()
    {
        runner = false;
        walker = true;

        wCam.Init();
        vfx.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //}
}
