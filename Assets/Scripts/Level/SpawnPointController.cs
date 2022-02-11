using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType 
{
    Player,
    Enemy,
}

public class SpawnPointController : MonoBehaviour
{
    public SpawnType Type;
}
