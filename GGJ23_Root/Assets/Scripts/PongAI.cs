using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PongAI : MonoBehaviour 
{
    private Rigidbody rigidbody;    
    private Transform target;    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update() 
    {
        if(!target)    
            return;

        // rigidbody.position = Settings.
    }
}