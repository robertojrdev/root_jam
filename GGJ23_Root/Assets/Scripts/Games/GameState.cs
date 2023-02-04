using UnityEngine;

public class GameState
{
    public ObjectState cameraState;
    public ObjectState playerState;
}

[System.Serializable]
public class ObjectState
{
    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;

    public ObjectState(GameObject gameObject)
    {
        position = gameObject.transform.position;
        scale = gameObject.transform.localScale;
        rotation = gameObject.transform.rotation;
    }

    public void ApplyState(GameObject gameObject)
    {
        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
        gameObject.transform.rotation = rotation;
    }
}
