using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTileController : AbstractTileController
{
    public UnitController Unit;

    void OnMouseDown() 
    {
        GameObject.Find("GridHandler").GetComponent<GridManager>().MoveUnitToTile(Unit, this);
    }
}
