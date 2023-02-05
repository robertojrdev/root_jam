using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class PongAI : MonoBehaviour
{
    public Pong pong;
    public List<Transform> bricks = new List<Transform>();
    public List<BrickVisuals> bricksVisuals = new List<BrickVisuals>();
    [Header("Brick Spawning")]
    public float brickSpawnTime;
    [Min(1)]
    public int bricksPerSpawn = 1;
    public bool spawnRandomly = false;
    [Header("Visuals")]
    public Gradient bricksColorsOverTime;
    public float onHitScaleDuration = 0.3f;
    public AnimationCurve onHitScaleCurve;

    private Rigidbody rigidbody;
    private Transform target;

    private List<Transform> hiddenBricks;
    private Transform ogBrick;
    private int spawnedBrickIndex;
    private bool canSpawnBrick;

    //private int currentSpawns;
    //private int TotalSpawns => Mathf.CeilToInt(bricks.Count / bricksPerSpawn);


    public Action OnAllBricksSpawned;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        hiddenBricks = new List<Transform>(bricks);

        StartCoroutine(SpawnBrickTimer());
    }

    private void OnEnable()
    {
        pong.OnStageUpdated += SpawnBricks;
    }

    private void OnDisable()
    {
        pong.OnStageUpdated -= SpawnBricks;
    }

    private void HideBricks()
    {
        foreach (Transform brick in bricks) brick.gameObject.SetActive(false);
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

        // Set the speed of the rigidbody


        var targetPos = transform.position;
        targetPos.z = target.position.z;

        // Calculate the direction vector towards the target position
        Vector3 dir = (targetPos - transform.position);
        dir = Vector3.ClampMagnitude(dir, 1);

        // Move the rigidbody in that direction with a constant speed
        rigidbody.MovePosition(transform.position + dir * Settings.Instance.pongPlayerMovementSpeed * 1.5f * Time.deltaTime);
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
        else pong.StageUpdate(); // THIS WILL BE MOVED TO SOME OTHER LOGIC THAT CHANGES THE STAGE

        //SpawnBricks();

    }

    private void SpawnBricks(int currentStage, int totalStages)
    {
        //Debug.Log("SPAWN BRICKS!");
        //Debug.Log($"stage: {currentStage} total stages: {totalStages} bricks per spawn: {bricksPerSpawn}");

        for (int i = 0; i < bricksPerSpawn; i++)
        {
            //Debug.Log("HIDDEN BRICKS COUNT = " + hiddenBricks.Count);
            if (hiddenBricks.Count == 0) break;

            // Spawn randomly or at index 0
            int index = spawnRandomly ? UnityEngine.Random.Range(0, hiddenBricks.Count) : 0;

            // SPAWN BRICK
            //hiddenBricks[rand].gameObject.SetActive(true);

            Transform currentBrick = hiddenBricks[index];

            currentBrick.transform.localScale = Vector3.zero;
            currentBrick.gameObject.SetActive(true);

            currentBrick.transform.DOScale(new Vector3(0.3f, 1f, 1f), onHitScaleDuration)
            .SetEase(onHitScaleCurve);

            //DOTween.Sequence()
            //    .Append(currentBrick.DOScale(0, 0))
            //    .AppendCallback(() => currentBrick.gameObject.SetActive(true))
            //    .Append(currentBrick.DOScale(new Vector3(0.5f, 1.2f, 1.2f), 0.2f))
            //    .Append(currentBrick.DOScale(new Vector3(0.3f, 1f, 1f), 0.3f));

            // What's better ?
            hiddenBricks.RemoveAt(index);
            //hiddenBricks.Remove(currentBrick);
        }

        Color currentBricksColor = bricksColorsOverTime.Evaluate((float)currentStage / totalStages);

        foreach (BrickVisuals brick in bricksVisuals)
        {
            // set the brick color
            brick.UpdateColor(currentBricksColor);
        }
    }
}