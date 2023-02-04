using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PongAI : MonoBehaviour
{
    public List<Transform> bricks;
    public float brickSpawnTime;
    public int bricksPerSpawn;

    private Rigidbody rigidbody;
    private Transform target;

    private List<Transform> hiddenBricks;
    private Transform ogBrick;
    private int spawnedBrickIndex;
    private bool canSpawnBrick;

    public Action OnAllBricksSpawned;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;

        hiddenBricks = new List<Transform>(bricks);

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
        if (hiddenBricks.Count == 0)
        {
            OnAllBricksSpawned?.Invoke();
        }

        SpawnBricks();

    }

    private void SpawnBricks()
    {
        for (int i = 0; i < bricksPerSpawn; i++)
        {
            if (hiddenBricks.Count == 0) break;

            int rand = UnityEngine.Random.Range(0, hiddenBricks.Count);

            // SPAWN BRICK
            //hiddenBricks[rand].gameObject.SetActive(true);

            Transform currentBrick = hiddenBricks[rand];

            DOTween.Sequence()
                .Append(currentBrick.DOScale(0, 0))
                .AppendCallback(() => currentBrick.gameObject.SetActive(true))
                .Append(currentBrick.DOScale(new Vector3(0.5f, 1.2f, 1.2f), 0.2f))
                .Append(currentBrick.DOScale(new Vector3(0.3f, 1f, 1f), 0.3f));

            hiddenBricks.RemoveAt(rand);
        }
    }
}