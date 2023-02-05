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
    public float camTransitionSpeed = 1f;
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

    public void Init()
    {
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
            /*
            print("transitionToComputer");

            camT.position = Vector3.Lerp(camT.position, camComputerTarget.position, camTransitionSpeed * Time.deltaTime);
            camT.rotation = Quaternion.Lerp(camT.rotation, camComputerTarget.rotation, camTransitionSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, computerFov, camTransitionSpeed * Time.deltaTime);

            if (Mathf.Abs(cam.fieldOfView - computerFov) < 0.01f)
                FinishedComputerTransition(); */
        }
        else if(transitionToPlayer)
        {
            /*
            print("transitionToPlayer");
            camT.position = Vector3.Lerp(camT.position, camPlayerTarget.position, camTransitionSpeed * Time.deltaTime);
            camT.rotation = Quaternion.Lerp(camT.rotation, camPlayerTarget.rotation, camTransitionSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, camTransitionSpeed * Time.deltaTime);

            if(Mathf.Abs(cam.fieldOfView - defaultFov) < 0.01f)
            {
                crosshairUI.SetActive(true);
                transitionToPlayer = false;
            } */
        }
        else
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
        transitionToComputer = true;

        StartCoroutine(TransitionToComputer());

        GetComponent<WalkerMovement>().DisableMovement();
        interactUI.text = "";
        crosshairUI.SetActive(false);
    }

    public void PCtoWalkingSimTransition()
    {
        transitionToComputer = false;
        transitionToPlayer = true;

        StartCoroutine(TransitionToPlayer());

        crosshairUI.SetActive(false);
        menuAnim.Play("close menu");
        GetComponent<WalkerMovement>().EnableMovememt();
    }

    private IEnumerator TransitionToComputer()
    {
        t = 0f;
        Vector3 camPos = camT.position;
        Quaternion camRot = camT.rotation;
        float fov = cam.fieldOfView;

        while (t < camTransitionSpeed)
        {
            t += Time.deltaTime;

            if (t > camTransitionSpeed) t = camTransitionSpeed;

            camT.position = Vector3.Lerp(camPos, camComputerTarget.position, t / camTransitionSpeed);
            camT.rotation = Quaternion.Lerp(camRot, camComputerTarget.rotation, t / camTransitionSpeed);
            cam.fieldOfView = Mathf.Lerp(fov, computerFov, t / camTransitionSpeed);

            //if (Mathf.Abs(cam.fieldOfView - computerFov) < 0.01f)

            yield return null;
        }

            //FinishedComputerTransition();
    }

    private IEnumerator TransitionToPlayer()
    {
        t = 0f;
        Vector3 camPos = camT.position;
        Quaternion camRot = camT.rotation;
        float fov = cam.fieldOfView;

        while(t < camTransitionSpeed)
        {
            camT.position = Vector3.Lerp(camPos, camPlayerTarget.position, t / camTransitionSpeed);
            camT.rotation = Quaternion.Lerp(camRot, camPlayerTarget.rotation, t / camTransitionSpeed);
            cam.fieldOfView = Mathf.Lerp(fov, defaultFov, t / camTransitionSpeed);

            if (Mathf.Abs(cam.fieldOfView - defaultFov) < 0.01f)
            {
                crosshairUI.SetActive(true);
                transitionToPlayer = false;
            }

            yield return null;
        }

        transitionToPlayer = false;
    }
}
