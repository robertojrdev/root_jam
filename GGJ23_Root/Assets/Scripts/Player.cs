using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position { get => transform.position; set { transform.position = value; } }
    public Quaternion rotation { get => transform.rotation; set { transform.rotation = value; } }
    public IController controller { get; set; }

    public Rigidbody rb;
    public ParticleSystem hitVFX;

    private void Update()
    {
        if (controller != null && GameManager.GamePlaying)
        {
            controller.ApplyMovement(this);
        }
    }


    public void PlayHitVFX()
    {
        if (hitVFX != null)
        {
            if (!hitVFX.gameObject.activeSelf) hitVFX.gameObject.SetActive(true);
            hitVFX.Play();
        }
    }
}