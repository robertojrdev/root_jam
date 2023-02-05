using UnityEngine;
using DG.Tweening;

public static class CamShake
{
    public static void Shake(Camera cam, float duration, float strength = 3, int vibrato = 10, float randomness = 90, bool fadeOut = true)
    {
        cam.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }
}

