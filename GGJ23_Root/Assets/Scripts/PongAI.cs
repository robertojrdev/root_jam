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
    public Player player;
    public BrickVisuals mainBrick;
    public List<BrickVisuals> bricks = new List<BrickVisuals>();
    [Header("Brick Spawning")]
    public float brickSpawnTime;
    [Min(1)]
    public int bricksPerSpawn = 1;
    public bool spawnRandomly = false;
    [Header("Visuals")]
    public bool updateBricksColorsOverTime;
    public Gradient bricksColorsOverTime;
    public float onHitScaleDuration = 0.3f;
    public AnimationCurve onHitScaleCurve;

    private Rigidbody rigidbody;
    private Transform target;

    private List<BrickVisuals> hiddenBricks;
    private int spawnedBrickIndex;
    private bool canSpawnBrick;

    //private int currentSpawns;
    //private int TotalSpawns => Mathf.CeilToInt(bricks.Count / bricksPerSpawn);


    public Action OnAllBricksSpawned;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        hiddenBricks = new List<BrickVisuals>(bricks);
        hiddenBricks.RemoveAt(0);
        HideBricks();

        StartCoroutine(SpawnBrickTimer());
    }

    private void OnEnable()
    {
        pong.OnStageUpdated += OnStageUpdate;
    }

    private void OnDisable()
    {
        pong.OnStageUpdated -= OnStageUpdate;
    }

    private void HideBricks()
    {
        foreach (BrickVisuals brick in hiddenBricks) brick.Show(false);
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
        rigidbody.MovePosition(transform.position + dir * GameManager.Instance.settings.pongPlayerMovementSpeed * 1.5f * Time.deltaTime);
    }

    private IEnumerator SpawnBrickTimer()
    {
        yield return new WaitForSeconds(brickSpawnTime);

        canSpawnBrick = true;
    }

    public void OnBallCollision(Collision other)
    {
        Debug.Log("");
        if (other.transform == transform)
        {
            mainBrick.OnHit(); // Play enemy hit effect
            
            SFXManager.PlaySFX("pong_hit_original");
            // SFXManager.PlaySFX("pong_hit_" + UnityEngine.Random.Range(0, 4));
        }
        else if (other.transform.CompareTag("Player"))
        {
            SFXManager.PlaySFX("pong_hit_original");
            // SFXManager.PlaySFX("pong_hit_" + UnityEngine.Random.Range(0, 4));
            
            // play player hit effects
            Debug.Log("PLAYER = null " + player == null);
            player.PlayHitVFX();
        }
        else
        {
            //PLAY SFX DE QUANDO SE ACERTA A PAREDE
            SFXManager.PlaySFX("pong_wall_original");
            // SFXManager.PlaySFX("pong_wall");
        }

        //else quando bate na parede e play qquando quiser
    }

    public void OnStageUpdate(int stage, int totalStages)
    {
        Debug.Log("Should");

        SpawnBricks(stage, totalStages);
    }

    private void SpawnBricks(int currentStage, int totalStages)
    {
        //PLAY SFX DE popup de um brick novo: popupClones
        SFXManager.PlaySFX("pong_popup_clones");

        if (hiddenBricks.Count == 0)
        {
            OnAllBricksSpawned?.Invoke();
        }
        Debug.Log("SPAWN BRICKS!");
        Debug.Log($"stage: {currentStage} total stages: {totalStages} bricks per spawn: {bricksPerSpawn}");

        for (int i = 0; i < bricksPerSpawn; i++)
        {
            //Debug.Log("HIDDEN BRICKS COUNT = " + hiddenBricks.Count);
            if (hiddenBricks.Count == 0) break;

            // Spawn randomly or at index 0
            int index = spawnRandomly ? UnityEngine.Random.Range(0, hiddenBricks.Count) : 0;

            // SPAWN BRICK
            //hiddenBricks[rand].gameObject.SetActive(true);

            BrickVisuals currentBrick = hiddenBricks[index];

            currentBrick.transform.localScale = Vector3.zero;
            currentBrick.Show(true);

            currentBrick.transform.DOScale(Vector3.one, onHitScaleDuration)
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

        transform.DOScale(Vector3.one, onHitScaleDuration)
            .SetEase(onHitScaleCurve);

        Color currentBricksColor = bricksColorsOverTime.Evaluate((float)currentStage / totalStages);

        if (updateBricksColorsOverTime)
            foreach (BrickVisuals brick in bricks)
            {
                // set the brick color
                brick.UpdateColor(currentBricksColor);
            }
    }
}