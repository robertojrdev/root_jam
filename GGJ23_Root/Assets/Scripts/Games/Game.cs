using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public abstract void StartGame();
    public abstract void OnFinishGame();

    public void FinishGame()
    {
        OnFinishGame();
    }
}