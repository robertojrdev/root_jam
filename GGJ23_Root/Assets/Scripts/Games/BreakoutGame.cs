using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System.Linq;

public class BreakoutGame : Game
{
    [Header("References")]
    public Player player;
    public Ball ball;
    public Transform gameHolder;
    public Transform brickFirstRowPivot;
    public List<Transform> brickRows;

    [Header("Settings")]
    public float ballSpeed;
    public float minMaxMovement;

    [Header("Initial Anim Properties")]
    public float gameTargetXRotationAngle;
    public float paddlesCenterTime;
    public float rotationTimer;
    public float camTargetYPos;
    public float camMoveTime;
    public float ballToCenterTime;
    public Vector3 ballInitPos;

    [Header("End Game Condition")]
    public float bricksLeftToWin;


    private List<float> bricksLocalX = new List<float>();
    private Vector3 ballDirection;
    private Vector3 ballPos;
    private Vector3 playerPos;
    private Vector3 ballVelocity;
    private Vector3 initialWallPos;
    private int initialBricksAlive;
    private int currentbricksAlive;
    private int bricksToTriggerNextStage;

    private List<Transform> activeBricks = new List<Transform>();
    private List<BrickVisuals> bricksVfx = new List<BrickVisuals>();


    protected override void SetupGame()
    {
        GameManager.GamePlaying = false;
        player.controller = new BreakoutController();
        ((BreakoutController)player.controller).SetMaxMovement(minMaxMovement);

        brickFirstRowPivot.position = Whiteboard.instance.pong_BrickPos;
        ballPos = Whiteboard.instance.pong_BallPosition;
        playerPos = Whiteboard.instance.pong_PlayerPosition;

        ballDirection = Whiteboard.instance.pong_BallDirection;

        foreach (Transform t in brickRows)
        {
            bricksLocalX.Add(t.localPosition.x);

            Rigidbody[] brickRbs = t.GetComponentsInChildren<Rigidbody>();
            BrickVisuals[] bricks = t.GetComponentsInChildren<BrickVisuals>();

            foreach (BrickVisuals bv in bricks)
            {
                bricksVfx.Add(bv);
            }

            foreach (Rigidbody rb in brickRbs)
            {
                activeBricks.Add(rb.GetComponent<Transform>());
            }
        }

        initialWallPos = brickRows[0].localPosition;

        initialBricksAlive = brickRows[0].childCount * brickRows.Count;
        currentbricksAlive = initialBricksAlive;
        bricksToTriggerNextStage = initialBricksAlive - (initialBricksAlive / stages);

        ball.onBallCollision += OnBallCollide;
    }

    protected override void OnStartGame()
    {
        StartCoroutine(StartGameTimer());
    }

    private IEnumerator StartGameTimer()
    {
        float t = 0;
        Vector3 ballCurrentPos = ball.transform.localPosition;

        DOTween.Sequence()
            .Append(brickFirstRowPivot.DOMoveZ(0, paddlesCenterTime))
            .Append(gameHolder.DORotate(new Vector3(0, gameTargetXRotationAngle, 0), rotationTimer))
            .Join(gameCam.transform.DOMoveY(camTargetYPos, camMoveTime));


        while (t < ballToCenterTime)
        {
            t += Time.deltaTime;

            ball.transform.localPosition = Vector3.Lerp(ballCurrentPos, ballInitPos, t / ballToCenterTime);

            yield return null;
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < brickRows.Count; i++)
        {
            yield return StartCoroutine(RowDescendRoutine(i));
        }


        // Start the game
        GameManager.GamePlaying = true;


        SetBallDirection(ballDirection);
    }

    private IEnumerator RowDescendRoutine(int index)
    {
        float t = 0;
        float wallDescendDuration = 0.1f;

        brickRows[index].localPosition = initialWallPos;
        brickRows[index].gameObject.SetActive(true);
        Vector3 initPos = brickRows[index].localPosition;
        Vector3 destinationPos = new Vector3(bricksLocalX[bricksLocalX.Count - 1 - index], 0, 0);

        while (t < wallDescendDuration)
        {
            t += Time.deltaTime;
            brickRows[index].localPosition = Vector3.Lerp(initPos, destinationPos, t / wallDescendDuration);
            yield return null;
        }

        CamShake.Shake(Camera.main, 0.1f, 0.2f, 8);
        brickRows[index].localPosition = destinationPos;
    }

    public void SetBallDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        ballVelocity = direction * ballSpeed;
    }

    private void FixedUpdate()
    {
        ball.Rigidbody.velocity = Vector3.zero;
        var movement = ballVelocity * Time.deltaTime;
        var position = ball.Rigidbody.position + movement;
        ball.Rigidbody.MovePosition(position);
    }

    private void OnBallCollide(Collision other)
    {
        var reflectDirection = Vector3.Reflect(ballVelocity, other.contacts[0].normal);
        var ySpeed = other.rigidbody.velocity.z;

        reflectDirection.z += ySpeed;
        SetBallDirection(reflectDirection);

        if (other.transform.CompareTag("Wall")) 
        {
            SFXManager.PlaySFX("pong_wall" + Random.Range(0, 4));
            return;
        }
        if (other.transform.CompareTag("Player"))
        {
            SFXManager.PlaySFX("pong_hit_" + Random.Range(0, 4));
            return;
        }

        DespawnBrick(other);
        currentbricksAlive--;

        if (currentbricksAlive <= bricksLeftToWin)
        {
            print("acabou crl!!");
            FinishGame();
            return;
        }

        if (currentbricksAlive == bricksToTriggerNextStage)
        {
            bricksToTriggerNextStage -= (initialBricksAlive / stages);
            StageUpdate();
        }
    }

    private void DespawnBrick(Collision other)
    {
        BrickVisuals bv = other.transform.GetComponentInChildren<BrickVisuals>();
        Transform t = other.transform;

        bv.OnHit();
        bricksVfx.Remove(bv);
        activeBricks.Remove(t);

        SFXManager.PlaySFX("break_" + Random.Range(0, 8));

        /*
        Renderer renderer = other.transform.GetChild(0).GetComponent<Renderer>();

        DOTween.Sequence()
            .Append(renderer.material.DOColor(Color.red, 0f))
            .Append(other.transform.DOScaleZ(0, 0.1f))
            .AppendCallback(() => other.gameObject.SetActive(false));*/

        //other.gameObject.SetActive(false);
    }

    protected override void OnUpdateStage()
    {
        // 

        print("next Stage");
    }

    protected override void OnFinishGame()
    {
        print("Finished game!");
        GameManager.GamePlaying = false;

        // AO FAZER DEBUG A LISTA DE ACTIVE BRICKS PODE TER MUITO MAIS DO QUE 6.
        // DESTA MANEIRA APENAS FICAM 6 PARA EFEITOS DE TRANSI�AO MESMO QD ESTAMOS EM DEBUG

        Whiteboard.instance.breakout_LastBricksPos.Clear();
        Whiteboard.instance.breakout_LastBricksRot.Clear();

        for (int i = 0; i < bricksLeftToWin; i++)
        {
            Whiteboard.instance.breakout_LastBricksPos.Add(activeBricks[i].position);
            Whiteboard.instance.breakout_LastBricksRot.Add(activeBricks[i].rotation);
        }

        Whiteboard.instance.breakout_CameraPos = gameCam.transform.position;
        Whiteboard.instance.breakout_CameraRot = gameCam.transform.rotation;
        Whiteboard.instance.breakout_CameraFoV = gameCam.fieldOfView;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) FinishGame();
    }
}