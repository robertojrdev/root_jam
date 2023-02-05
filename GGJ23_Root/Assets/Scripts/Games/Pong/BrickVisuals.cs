using UnityEngine;
using DG.Tweening;

public class BrickVisuals : MonoBehaviour
{
    public MeshRenderer mesh;
    public ParticleSystem[] bopVFX;
    public ParticleSystem hitEffects;

    private Material mat;

    private void Awake()
    {
        mat = mesh.material;
    }

    public void Show(bool show)
    {
        if (mesh != null) mesh.gameObject.SetActive(show);
        if (bopVFX != null)
        {
            Color c = show ? Color.white : Color.clear;
            foreach (ParticleSystem ps in bopVFX)
            {
                var main = ps.main;
                main.startColor = c;
            }
        }
    }

    public void OnHit()
    {
        Debug.Log("PLAY HIT EFFECTS!");
        if (hitEffects) hitEffects.Play();
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
