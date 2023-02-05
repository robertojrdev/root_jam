using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Whiteboard", menuName = "Game/Whiteboard")]
/// <summary>
/// Use this scriptable object to feed variables between different games
/// </summary>
public class Whiteboard : ScriptableSingleton<Whiteboard>
{
    // NOMENCLATURE RULES:
    // 1 - Start with the corresponding game that feeds the 
    //     variable then underscore and the variable name, like so_ <gameName>_exampleVar


    // PONG
    public Vector3 pong_BrickPos;
    public Vector3 pong_BallPosition;
    public Vector3 pong_PlayerPosition;
    public float pong_BallSpeed;

    // BREAKOUT
    public List<Transform> breakout_LastBricks;

    // RUNNER
    public Vector3 runner_CameraPos;
    public Quaternion runner_CameraRot;
    public float runner_CameraFoV;

    // isto tem de ser 6 bricks
    public Vector3[] runner_BrickPositions =
    {
        new Vector3 (0f, 0f, -8.8f),
        new Vector3 (-1.4f, 3.2f),
        new Vector3 (-2f, 0f, 8.8f),
        new Vector3 (-3.15f, 0f, 6.8f),
        new Vector3 (-2f, 0f, -4.8f),
        new Vector3 (-2.7f, 0f, -2.8f),
    };
}
