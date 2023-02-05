using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunnerController : MonoBehaviour
{
    public int track = 1;
    public InfiniteRunner infiniteRunner;
    public float speed = 10f;

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            track--;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            track++;

        track = Mathf.Clamp(track, 0, infiniteRunner.tracks.Length-1);

        pos.x = Mathf.Lerp(pos.x, track * infiniteRunner.tracks[track].position.x, speed * Time.deltaTime);
        transform.position = pos;
    }
}