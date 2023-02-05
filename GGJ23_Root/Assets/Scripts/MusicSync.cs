using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSync : MonoBehaviour
{
    public float timeToStart = 15;
    public AudioClip clip;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.Stop();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToStart);
        if(!clip)
            yield break;

        source.clip = clip;
        source.Play();
    }

    private void OnDisable()
    {
        source.Stop();
    }
}