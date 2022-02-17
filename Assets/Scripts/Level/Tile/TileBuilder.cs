using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[System.Serializable]
public class TileBuilder : MonoBehaviour
{
    public string TileName = "Tile";
    public int MovementCost = 1;
    public int DetectionPenalty = 0;
    public Vector2Int Size = new Vector2Int(1,1);
}
