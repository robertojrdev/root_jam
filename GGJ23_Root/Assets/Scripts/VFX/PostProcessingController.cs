using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    public Volume volume;
    public float bloomIntensity = 0f;
    public float chromaticAberrationIntensity = 0f;
    public float paniniProjectionIntensity = 0f;

    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private PaniniProjection paniniProjection;

    void Start()
    {
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out paniniProjection);
    }

    void Update()
    {
        if(bloom != null)
            bloom.intensity.value = bloomIntensity;

        if(chromaticAberration != null)    
            chromaticAberration.intensity.value = chromaticAberrationIntensity;

        if(paniniProjection != null)    
            paniniProjection.distance.value = paniniProjectionIntensity;
    }
}