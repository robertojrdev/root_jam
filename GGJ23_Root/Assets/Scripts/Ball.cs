using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 direction;
    public Action<Collision> onBallCollision;

    private List<Transform> collidedObjects = new List<Transform>();

    public Rigidbody Rigidbody { get => rb; }

    private void Awake()
    {
        // rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collidedObjects.Contains(other.transform)) return;
        //Debug.Log("BALL COLLIDED WITH " + other.gameObject.name);
        collidedObjects.Add(other.transform);
        onBallCollision?.Invoke(other);
    }
    private void OnCollisionExit(Collision other)
    {
        if (collidedObjects.Contains(other.transform))
            collidedObjects.Remove(other.transform);
    }
}
