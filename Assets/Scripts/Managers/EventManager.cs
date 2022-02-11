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

    public event Action<AbstractUnitController> onUnitClicked;
    public void UnitClicked(AbstractUnitController unit)
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

    public event Action<AbstractUnitController> onShowMovement;
    public void ShowMovementForUnit(AbstractUnitController unit) 
    {
        if (onShowMovement != null)
        {
            onShowMovement(unit);
        }

    }

    public event Action<AbstractUnitController, MoveTileController> onUnitMovement;
    public void MoveUnitToTile(AbstractUnitController unit, MoveTileController tile) 
    {
        if (onUnitMovement != null)
        {
            onUnitMovement(unit, tile);
        }
    }
}
