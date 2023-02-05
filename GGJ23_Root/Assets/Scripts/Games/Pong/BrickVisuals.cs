using UnityEngine;
using DG.Tweening;

public class BrickVisuals : MonoBehaviour
{
    public MeshRenderer rend;
    public ParticleSystem hitEffects;

    private Material mat;

    private void Awake()
    {
        mat = rend.material;
    }

    public void OnHit()
    {
        if (hitEffects) hitEffects.Play();
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
