using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public AudioSource[] sources;
    private int activeSource = -1;

    public void Interact()
    {
        activeSource++;
        if (activeSource > sources.Length - 1)
            activeSource = 0;

        for (int i = 0; i < sources.Length; i++)
            sources[i].volume = (i == activeSource ? 0.119f : 0f);
    }
}
