using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool fakeVersion = false;
    public Transform[] buttons;
    public Transform selector;
    private int option = 0;

    public void Update()
    {
        if (GetComponent<CanvasGroup>().alpha < 1f) return;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            option = 1;
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            option = 0;

        if (Input.GetKeyDown(KeyCode.Space))
            ActivateOption();

        selector.parent = buttons[option];
        selector.localPosition = Vector3.zero;
    }

    // isto é chamada na anim "close menu" no animator do menu
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ActivateOption()
    {
        if(option == 0)
        {
            if (fakeVersion)
                SceneManager.LoadScene("MainScene");
            else
                GetComponent<Animator>().Play("close menu");
        }
        else if(option == 1)
        {
            // quit
            //if (fakeVersion)
            //else
        }
    }
}
