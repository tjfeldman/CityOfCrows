using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class MoveTileController : AbstractTileController
{
    public AbstractUnitController Unit;

    private bool active = false;

    public void AllowMovement()
    {
        active = true;
    }

    void OnMouseDown() 
    {
        //only allow event to trigger if the move tile controller is active
        if (active) {
            EventManager.current.MoveUnitToTile(Unit, this);
        }
    }
}
