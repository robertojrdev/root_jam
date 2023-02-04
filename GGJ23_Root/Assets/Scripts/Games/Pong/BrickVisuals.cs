using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BrickVisuals : MonoBehaviour
{
    public MeshRenderer rend;

    private Material mat;

    private void Awake()
    {
        mat = rend.material;
    }

    public void UpdateColor(Color color, float blendTime = 0)
    {
        if (mat == null) mat = rend.material;

        if (blendTime > 0)
        {
            mat.DOColor(color, blendTime);
        }
        else mat.color = color;
    }
}