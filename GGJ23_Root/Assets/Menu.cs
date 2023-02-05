using UnityEngine;

public class Menu : MonoBehaviour
{
    public Transform[] buttons;
    public Transform selector;
    private int option = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            option = 1;
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            option = 0;

        if (Input.GetKeyDown(KeyCode.Space))
            ActivateOption();

        selector.parent = buttons[option];
        selector.localPosition = Vector3.zero;
    }

    public void ActivateOption()
    {
        if(option == 0)
        {
            // play
            print("play");
        }
        else if(option == 1)
        {
            // quit
            print("quit");
        }
    }
}
