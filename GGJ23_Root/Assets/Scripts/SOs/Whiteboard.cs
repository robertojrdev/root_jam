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
    public Vector3 pong_BallDirection;
    public Vector3 pong_BallPosition;
    public Vector3 pong_PlayerPosition;

    // BREAKOUT
    public List<Transform> breakout_LastBricks;
}
