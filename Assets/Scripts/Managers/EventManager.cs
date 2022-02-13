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

    public event Action<AbstractUnitController> onMovementAction;
    public void MovementActionForUnit(AbstractUnitController unit) 
    {
        if (onMovementAction != null)
        {
            onMovementAction(unit);
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

    public event Action<AbstractUnitController, AbstractTileController> onUndoMovement;
    public void UndoMovement(AbstractUnitController unit, AbstractTileController tile)
    {
        if (onUndoMovement != null)
        {
            onUndoMovement(unit, tile);
        }
    }

    public event Action<AbstractUnitController> onUnitEndTurn;
    public void UnitEndTurn(AbstractUnitController unit)
    {
        if (onUnitEndTurn != null)
        {
            onUnitEndTurn(unit);
        }
    }
}
