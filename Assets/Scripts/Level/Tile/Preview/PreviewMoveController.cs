using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewMoveController : AbstractPreviewController
{
    private bool active = false;

    public void AllowMovement()
    {
        active = true;
    }

    protected override void DoAction()
    {
        //only allow event to trigger if the move tile controller is active
        if (active) {
            EventManager.current.MoveUnitToTile(Unit, TerrainTile);
        }
    }
}
