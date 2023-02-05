using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioReference
{
    public string id;
    public AudioClip audio;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    private AudioSource audioSource;
    public List<AudioReference> audioReferences = new List<AudioReference>();

    private void Awake()
    {
        // Ensure only one instance of the SFX Manager exists in the scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySFX(string id)
    {
        AudioClip audioClip = instance.audioReferences.Find(x => x.id == id)?.audio;
        if (audioClip != null)
        {
            instance.audioSource.PlayOneShot(audioClip);
        }
    }
}
