using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class MoveTileController : AbstractTileController
{
    public UnitController Unit;

    void OnMouseDown() 
    {
        EventManager.current.MoveUnitToTile(Unit, this);
    }
}
