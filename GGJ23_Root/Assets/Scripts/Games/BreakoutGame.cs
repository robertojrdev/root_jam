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
    public ParticleSystem killerFlash;

    [Header("Settings")]
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

    float CompletePercentage => 1 - currentbricksAlive / initialBricksAlive;
    Vector2 minMaxTimeToMaxSpeed;
    Vector2 minMaxSlowSpeed;
    Vector2 minMaxFastSpeed;
    float currentBallSpeed;
    float elapsedTime = 0;
    float timeToMaxSpeed;

    #region Unity Lifecycle

    private void Start()
    {
        currentStage = 0;
        elapsedTime = 0;
    }

    private void FixedUpdate()
    {
        ball.Rigidbody.velocity = Vector3.zero;
        var movement = ballVelocity * Time.deltaTime;
        var position = ball.Rigidbody.position + movement;
        ball.Rigidbody.MovePosition(position);
    }

    private void Update()
    {
        if (GameManager.Instance.Mode == Mode.Debug && Input.GetKeyDown(KeyCode.N)) FinishGame();
        UpdateTimeBasedParameters();
    }

    #endregion

    protected override void SetupGame()
    {
        GameManager.GamePlaying = false;
        player.controller = new BreakoutController();
        ((BreakoutController)player.controller).SetMaxMovement(minMaxMovement);

        brickFirstRowPivot.position = GameManager.Instance.whiteboard.pong_BrickPos;
        ballPos = GameManager.Instance.whiteboard.pong_BallPosition;
        playerPos = GameManager.Instance.whiteboard.pong_PlayerPosition;

        ballDirection = Vector3.left + Vector3.forward;
        currentBallSpeed = GameManager.Instance.whiteboard.pong_BallSpeed;

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

        minMaxTimeToMaxSpeed = GameManager.Instance.settings.breakoutMinMaxTimeToReachMaxSpeed;
        timeToMaxSpeed = minMaxTimeToMaxSpeed.x;
        minMaxSlowSpeed = GameManager.Instance.settings.breakoutMinMaxSlowestSpeed;
        minMaxFastSpeed = GameManager.Instance.settings.breakoutMinMaxFastestSpeed;
        elapsedTime = 0;

        ball.onBallCollision += OnBallCollide;
    }

    protected override void OnStartGame()
    {
        StartCoroutine(StartGameTimer());
    }

    protected override void OnRestartGame()
    {
        ballPos = GameManager.Instance.whiteboard.pong_BallPosition;
        playerPos = GameManager.Instance.whiteboard.pong_PlayerPosition;
        int rand = Random.Range(0, 2);
        Vector3 horizontalDir = rand == 0 ? Vector3.left : Vector3.right;
        ballDirection = horizontalDir + Vector3.forward;
        currentBallSpeed = GameManager.Instance.whiteboard.pong_BallSpeed;
        GameManager.GamePlaying = false;
        //UIManager.Instance.ShowCountdown(true);
        StartCoroutine(StartGameTimer(false));
    }

    private IEnumerator StartGameTimer(bool playTransition = true)
    {
        if (playTransition)
        {
            DOTween.Sequence()
                .Append(brickFirstRowPivot.DOMoveZ(0, paddlesCenterTime))
                .Append(gameHolder.DORotate(new Vector3(0, gameTargetXRotationAngle, 0), rotationTimer))
                .Join(gameCam.transform.DOMoveY(camTargetYPos, camMoveTime));
        }

        float t = 0;
        Vector3 ballCurrentPos = ball.transform.localPosition;

        while (t < ballToCenterTime)
        {
            t += Time.deltaTime;

            ball.transform.localPosition = Vector3.Lerp(ballCurrentPos, ballInitPos, t / ballToCenterTime);

            yield return null;
        }

        if (playTransition)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < brickRows.Count; i++)
            {
                yield return StartCoroutine(RowDescendRoutine(i));
            }
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

    public void UpdateTimeBasedParameters()
    {
        timeToMaxSpeed = Mathf.Lerp(minMaxTimeToMaxSpeed.x, minMaxTimeToMaxSpeed.y, CompletePercentage);
        elapsedTime += Time.deltaTime;
        float percentTime = elapsedTime / timeToMaxSpeed;

        float slowestSpeed = Mathf.Lerp(minMaxSlowSpeed.x, minMaxSlowSpeed.y, percentTime);
        float fastestSpeed = Mathf.Lerp(minMaxSlowSpeed.x, minMaxSlowSpeed.y, percentTime);
        currentBallSpeed = Mathf.Lerp(slowestSpeed, fastestSpeed, percentTime);
    }

    public void SetBallDirection(Vector3 direction)
    {
        direction.y = 0;
        direction.Normalize();
        ballVelocity = direction * currentBallSpeed;
    }

    private void OnBallCollide(Collision other)
    {
        // Lose condition
        if (other.gameObject.CompareTag("Bounds"))
        {
            CamShake.Shake(gameCam, 0.2f, 0.8f, 10);
            killerFlash.Play();
            SFXManager.PlaySFX("breakout_fail");

            RestartGame();
            return;
        }

        var reflectDirection = Vector3.Reflect(ballVelocity, other.contacts[0].normal);
        var ySpeed = other.rigidbody.velocity.z;

        reflectDirection.z += ySpeed;
        SetBallDirection(reflectDirection);
        Debug.Log("Percentage = " + CompletePercentage);
        ShowChromaticAberration(CompletePercentage * 0.8f, 0.2f);

        if (other.transform.CompareTag("Wall"))
        {
            SFXManager.PlaySFX("pong_wall" + Random.Range(0, 4));
            CamShake.Shake(gameCam, 0.2f, 0.1f, 8);
            return;
        }
        if (other.transform.CompareTag("Player"))
        {
            SFXManager.PlaySFX("pong_hit_" + Random.Range(0, 4));
            CamShake.Shake(gameCam, 0.2f, 0.1f, 8);
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
        Debug.Log("COLLIDED WITH " + other.gameObject.name);
        BrickVisuals bv = other.transform.GetComponentInChildren<BrickVisuals>();

        Transform t = other.transform;
        bv.OnHit();
        bv.Show(false);
        bricksVfx.Remove(bv);
        activeBricks.Remove(t);

        SFXManager.PlaySFX("break_" + Random.Range(0, 8));
        CamShake.Shake(gameCam, 0.2f, 0.2f, 8);

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

        GameManager.Instance.whiteboard.breakout_LastBricksPos.Clear();
        GameManager.Instance.whiteboard.breakout_LastBricksRot.Clear();

        for (int i = 0; i < bricksLeftToWin; i++)
        {
            GameManager.Instance.whiteboard.breakout_LastBricksPos.Add(activeBricks[i].position);
            GameManager.Instance.whiteboard.breakout_LastBricksRot.Add(activeBricks[i].rotation);
        }

        GameManager.Instance.whiteboard.breakout_CameraPos = gameCam.transform.position;
        GameManager.Instance.whiteboard.breakout_CameraRot = gameCam.transform.rotation;
        GameManager.Instance.whiteboard.breakout_CameraFoV = gameCam.fieldOfView;
    }

}