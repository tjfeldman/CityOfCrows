using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class AttackTileController : AbstractTileController
{
    public AbstractUnitController Unit;

    void OnMouseDown() 
    {
        // EventManager.current.MoveUnitToTile(Unit, this);
    }
}
