using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pong_ball : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody rb;
    private Vector3 direction;

    private void Start()
    {
        direction = new Vector3(1, 0, Random.Range(-0.2f, 0.2f)).normalized;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            transform.position = new Vector3(-2f, 0f, 0f);

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
            direction.z *= -1;

        if (collision.collider.CompareTag("Player"))
            direction.x *= -1;
    }
}
