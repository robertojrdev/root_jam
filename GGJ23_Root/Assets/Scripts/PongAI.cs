using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongAI : MonoBehaviour
{
    public float smoothSpeed;
    private Rigidbody rigidbody;
    private Transform target;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (!target)
            return;

        // if(target.position.x > 0)
        //     return;

        //move a rigidbody in a constant speed towards a target position


        // Get the Rigidbody component from the game object
        Rigidbody rb = GetComponent<Rigidbody>();

        // Set the speed of the rigidbody


        var targetPos = transform.position;
        targetPos.z = target.position.z;

        // Calculate the direction vector towards the target position
        Vector3 dir = (targetPos - transform.position);
        dir = Vector3.ClampMagnitude(dir, 1);

        // Move the rigidbody in that direction with a constant speed
        rb.MovePosition(transform.position + dir * Settings.Instance.pongPlayerMovementSpeed * 1.5f * Time.deltaTime);
    }
}