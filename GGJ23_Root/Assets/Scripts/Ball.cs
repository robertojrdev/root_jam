using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction;
    public Action<Collision> onBallCollision;

    public Rigidbody Rigidbody { get => rb; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        onBallCollision?.Invoke(other);
    }
}
