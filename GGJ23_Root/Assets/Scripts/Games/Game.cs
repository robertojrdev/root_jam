using UnityEngine;

public abstract class Game : MonoBehaviour
{
    private void Awake()
    {
        
    }

    public abstract void StartGame();
    public abstract void Update();
    public abstract void OnFinishGame();

    public void FinishGame()
    {
        OnFinishGame();
    }
}