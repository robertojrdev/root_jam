using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerCamera : MonoBehaviour
{
    public WalkerRunnerManager manager;

    public float defaultFov, computerFov;
    public Vector2 sens;
    public Transform camT;
    public Camera cam;
    public Transform camPlayerTarget, camComputerTarget;
    public float camTransitionSpeed = 1f;

    [Header("UI")]
    public GameObject interactUI;
    public GameObject crosshairUI;
    
    private Vector2 rot;
    private float timer = 0.0f;
    private bool transitionToComputer = false;
    private bool transitionToPlayer = false;

    public void Init()
    {
        crosshairUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.walker) return;

        timer += Time.deltaTime;

        if (transitionToComputer)
        {
            camT.position = Vector3.Lerp(camT.position, camComputerTarget.position, camTransitionSpeed * Time.deltaTime);
            camT.rotation = Quaternion.Lerp(camT.rotation, camComputerTarget.rotation, camTransitionSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, computerFov, camTransitionSpeed * Time.deltaTime);

            if (Mathf.Abs(cam.fieldOfView - computerFov) < 0.01f)
                FinishedComputerTransition();

            if (Input.GetKeyDown(KeyCode.E))
            {
                transitionToComputer = false;
                transitionToPlayer = true;
            }
        }
        else if(transitionToPlayer)
        {
            camT.position = Vector3.Lerp(camT.position, camPlayerTarget.position, camTransitionSpeed * Time.deltaTime);
            camT.rotation = Quaternion.Lerp(camT.rotation, camPlayerTarget.rotation, camTransitionSpeed * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, camTransitionSpeed * Time.deltaTime);

            if(Mathf.Abs(cam.fieldOfView - defaultFov) < 0.01f)
            {
                crosshairUI.SetActive(true);
                transitionToPlayer = false;
            }
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
            interactUI.SetActive(false);
            if (Physics.Raycast(ray, out hit, 2f))
            {
                if (hit.collider.isTrigger)
                {
                    if (hit.collider.CompareTag("Computer"))
                    {
                        interactUI.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            InteractComputer();
                        }
                    }
                }
            }
        }
    }

    public void FinishedComputerTransition()
    {
        crosshairUI.SetActive(true);
        // finished computer transition
    }

    private void InteractComputer()
    {
        transitionToComputer = true;
        GetComponent<WalkerMovement>().DisableMovement();
        interactUI.SetActive(false);
    }
}
