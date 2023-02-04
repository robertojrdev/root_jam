using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position { get => transform.position; set { transform.position = value; } }
    public Quaternion rotation { get => transform.rotation; set { transform.rotation = value; } }
    public IController controller { get; set; }

    private void Update()
    {
        if (controller != null)
        {
            controller.ApplyMovement(this);
        }
    }
}