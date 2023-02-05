using UnityEngine;
using DG.Tweening;

public class BrickVisuals : MonoBehaviour
{
    public MeshRenderer mesh;
    public ParticleSystem[] bopVFX;
    public ParticleSystem hitEffects;

    private Material mat;

    private Color[] vfxColors;

    private void Awake()
    {
        mat = mesh.material;

        vfxColors = new Color[bopVFX.Length];
        for (int i = 0; i < vfxColors.Length; i++)
        {
            var main = bopVFX[i].main;
            vfxColors[i] = main.startColor.color;
        }
    }

    public void Show(bool show)
    {
        if (mesh != null) mesh.gameObject.SetActive(show);
        if (bopVFX != null)
        {
            for (int i = 0; i < bopVFX.Length; i++)
            {
                Color c = show ? vfxColors[i] : Color.clear;
                var main = bopVFX[i].main;
                main.startColor = c;
            }
        }
    }

    public void OnHit()
    {
        Debug.Log("PLAY HIT EFFECTS!");
        if (hitEffects)
        {
            if (!hitEffects.gameObject.activeSelf) hitEffects.gameObject.SetActive(true);
            hitEffects.Play();
        }
    }

    public void UpdateColor(Color color, float blendTime = 0)
    {
        if (mat == null) mat = mesh.material;

        if (blendTime > 0)
        {
            mat.DOColor(color, blendTime);
        }
        else mat.color = color;
    }
}
