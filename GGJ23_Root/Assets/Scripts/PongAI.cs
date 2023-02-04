using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PongAI : MonoBehaviour
{
    public float brickSpawnTime;
    public int bricksPerSpawn;

    private Rigidbody rigidbody;
    private Transform target;

    private List<Transform> bricks = new List<Transform>();
    private List<Transform> hiddenBricks;
    private Transform ogBrick;
    private int spawnedBrickIndex;
    private bool canSpawnBrick;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;

        bricks = GetComponentsInChildren<Transform>(true).ToList();

        hiddenBricks = new List<Transform>(bricks);
        hiddenBricks.RemoveAt(0);
        //ogBrick = bricks[0];

        Shuffle(bricks);

        StartCoroutine(SpawnBrickTimer());
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        if (!target)
            return;

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

    private IEnumerator SpawnBrickTimer()
    {
        yield return new WaitForSeconds(brickSpawnTime);

        canSpawnBrick = true;
    }

    public void SpawnBrickOnCollision(Collision other)
    {
        if (other.transform != transform) 
            return;
        if (!canSpawnBrick) 
            return;

        SpawnBricks();

        if (hiddenBricks.Count == 0)
        {
            // NEXT GAME
        }
    }

    private void SpawnBricks()
    {
        for (int i = 0; i < bricksPerSpawn; i++)
        {
            if (hiddenBricks.Count == 0) break;

            int rand = Random.Range(0, hiddenBricks.Count);

            // SPAWN BRICK
            hiddenBricks[rand].gameObject.SetActive(true);

            hiddenBricks.RemoveAt(rand);
        }
    }

    public void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}