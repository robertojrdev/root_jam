using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalkerCamera : MonoBehaviour
{
    public WalkerRunnerManager manager;

    public float defaultFov, computerFov;
    public Vector2 sens;
    public Transform camT;
    public Camera cam;
    public Transform camPlayerTarget, camComputerTarget;
    public float camTransitionDuration = 1f;
    public AnimationCurve camTransitionCurve;
    public Radio radio;

    [Header("UI")]
    public TextMeshProUGUI interactUI;
    public GameObject crosshairUI;
    public Animator menuAnim;

    private Vector2 rot;
    private float timer = 0.0f;
    private bool transitionToComputer = false;
    private bool transitionToPlayer = false;
    private bool transitionHappening = false;
    float t = 0;

    bool canMove = true;

    public void Init()
    {
        canMove = true;
        crosshairUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.walker) return;

        timer += Time.deltaTime;


        if (transitionToComputer)
        {
        
        }
        else if (transitionToPlayer)
        {

        }
        else if (canMove)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens.x;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens.y;

            rot.y += mouseX;
            rot.x -= mouseY;
            rot.x = Mathf.Clamp(rot.x, -90f, 90f);

            // rotate cam
            transform.rotation = Quaternion.Euler(0, rot.y, 0);
            camT.rotation = Quaternion.Euler(rot.x, rot.y, 0);

            // Look detection
            RaycastHit hit;
            Ray ray = new Ray(camT.position, camT.forward);
            interactUI.text = "";
            if (Physics.Raycast(ray, out hit, 2f))
            {
                if (hit.collider.isTrigger)
                {
                    if (hit.collider.CompareTag("Computer"))
                    {
                        interactUI.text = "[E] to Interact";
                        if (Input.GetKeyDown(KeyCode.E))
                            InteractComputer();
                    }
                    else if (hit.collider.CompareTag("Radio"))
                    {
                        interactUI.text = "[E] to Interact";
                        if (Input.GetKeyDown(KeyCode.E))
                            radio.Interact();
                    }
                }
            }
        }
    }

    public void FinishedComputerTransition()
    {
        // finished computer transition
        crosshairUI.SetActive(false);
        menuAnim.Play("start menu");
    }

    private void InteractComputer()
    {
        canMove = false;
        transitionToComputer = true;

        StartCoroutine(TransitionToComputer());

        GetComponent<WalkerMovement>().DisableMovement();
        interactUI.text = "";
        crosshairUI.SetActive(false);
    }

    public void PCtoWalkingSimTransition()
    {
        canMove = false;
        //transitionToComputer = false;
        transitionToPlayer = true;

        StartCoroutine(TransitionToPlayer());

        crosshairUI.SetActive(false);
        menuAnim.Play("close menu");
        GetComponent<WalkerMovement>().EnableMovememt();
    }

    private IEnumerator TransitionToComputer()
    {
        Vector3 camPos = camT.position;
        Quaternion camRot = camT.rotation;
        float fov = cam.fieldOfView;

        float elapsedTime = 0;

        while (elapsedTime < camTransitionDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > camTransitionDuration) elapsedTime = camTransitionDuration;

            float percentTime = elapsedTime / camTransitionDuration;
            float t = camTransitionCurve.Evaluate(percentTime);
            camT.position = Vector3.LerpUnclamped(camPos, camComputerTarget.position, t);
            camT.rotation = Quaternion.LerpUnclamped(camRot, camComputerTarget.rotation, t);
            cam.fieldOfView = Mathf.LerpUnclamped(fov, computerFov, t);

            //if (Mathf.Abs(cam.fieldOfView - computerFov) < 0.01f)

            yield return null;
        }

        FinishedComputerTransition();
    }

    private IEnumerator TransitionToPlayer()
    {
        Vector3 camPos = camT.position;
        Quaternion camRot = camT.rotation;
        float fov = cam.fieldOfView;

        float elapsedTime = 0;

        while (elapsedTime < camTransitionDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > camTransitionDuration) elapsedTime = camTransitionDuration;

            float percentTime = elapsedTime / camTransitionDuration;
            float t = camTransitionCurve.Evaluate(percentTime);
            camT.position = Vector3.LerpUnclamped(camPos, camPlayerTarget.position, t);
            camT.rotation = Quaternion.LerpUnclamped(camRot, camPlayerTarget.rotation, t);
            cam.fieldOfView = Mathf.LerpUnclamped(fov, defaultFov, t);

            //if (Mathf.Abs(cam.fieldOfView - computerFov) < 0.01f)

            yield return null;
        }

        transitionToPlayer = false;
    }
}
