using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerMovement : MonoBehaviour
{
    public WalkerRunnerManager manager;

    [Header("Settings")]
    public float moveSpeed;
    public Transform orientation;
    public bool lockInput = false;
    public GameObject walkerObj;

    private Vector2 input;
    private Vector3 moveDir;
    
    // Components
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!manager.walker) return;
        UpdateInput();
    }

    private void FixedUpdate()
    {
        if (!manager.walker) return;
        MovePlayer();
    }

    private void UpdateInput()
    {
        if (lockInput) return;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * input.y + orientation.right * input.x;
        rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);

        Vector3 vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if(vel.magnitude > moveSpeed)
        {
            Vector3 limitVel = vel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    public void DisableMovement()
    {
        lockInput = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        walkerObj.SetActive(false);
    }

    public void EnableMovememt()
    {
        print("crl");

        lockInput = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        walkerObj.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
