using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    public event Action<UnitController> onUnitClicked;
    public void UnitClicked(UnitController unit)
    {
        if (onUnitClicked != null)
        {
            onUnitClicked(unit);
        }
    }

    public event Action<AbstractTileController> onTileClicked;
    public void TileClicked(AbstractTileController tile)
    {
        if (onTileClicked != null)
        {
            onTileClicked(tile);
        }
    }

    public event Action<UnitController> onShowMovement;
    public void ShowMovementForUnit(UnitController unit) 
    {
        if (onShowMovement != null)
        {
            onShowMovement(unit);
        }

    }

    public event Action<UnitController, MoveTileController> onUnitMovement;
    public void MoveUnitToTile(UnitController unit, MoveTileController tile) 
    {
        if (onUnitMovement != null)
        {
            onUnitMovement(unit, tile);
        }
    }
}
