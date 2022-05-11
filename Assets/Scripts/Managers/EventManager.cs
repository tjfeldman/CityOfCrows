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

    /*
    * Display Information Events
    */

    //Event when a unit is clicked
    public event Action<AbstractUnitController> onUnitClicked;
    public void UnitClicked(AbstractUnitController unit)
    {
        if (onUnitClicked != null)
        {
            onUnitClicked(unit);
        }
    }

    //Event when a tile is clicked
    public event Action<AbstractTileController> onTileClicked;
    public void TileClicked(AbstractTileController tile)
    {
        if (onTileClicked != null)
        {
            onTileClicked(tile);
        }
    }

    public event Action<AbstractUnitController> onUnitHover;
    public void DisplayInformationForUnit(AbstractUnitController unit)
    {
        if (onUnitHover != null) 
        {
            onUnitHover(unit);
        }
    }

    public event Action<AbstractUnitController> onUnitExit;
    public void HideDisplayInformationForUnit(AbstractUnitController unit)
    {
        if (onUnitExit != null) 
        {
            onUnitExit(unit);
        }
    }

    /*
    * Unit Action Events
    */

    //Event to display attack options when attack action is selected
    public event Action<Actions.AttackAction, AbstractUnitController> onAttackAction;
    public void AttackActionForUnit(Actions.AttackAction action, AbstractUnitController unit)
    {
        if (onAttackAction != null)
        {
            onAttackAction(action, unit);
        }
    }

    //Event to engage in weapon attack
    public event Action<AbstractUnitController, Weapons.IWeapon> onWeaponAttack;
    public void WeaponAttackActionForUnitWeapon(AbstractUnitController unit, Weapons.IWeapon weapon)
    {
        if (onWeaponAttack != null)
        {
            onWeaponAttack(unit, weapon);
        }
    } 

    //Event to display movement options when movement action is selected
    public event Action<AbstractUnitController> onMovementAction;
    public void MovementActionForUnit(AbstractUnitController unit) 
    {
        if (onMovementAction != null)
        {
            onMovementAction(unit);
        }

    }

    //Event to move unit to a location
    public event Action<AbstractUnitController, AbstractTileController> onUnitMovement;
    public void MoveUnitToTile(AbstractUnitController unit, AbstractTileController tile) 
    {
        if (onUnitMovement != null)
        {
            onUnitMovement(unit, tile);
        }
    }

    //Event to undo a movement action
    public event Action<AbstractUnitController, AbstractTileController> onUndoMovement;
    public void UndoMovement(AbstractUnitController unit, AbstractTileController tile)
    {
        if (onUndoMovement != null)
        {
            onUndoMovement(unit, tile);
        }
    }

    //Event when a unit ends their turn
    public event Action<AbstractUnitController> onUnitEndTurn;
    public void UnitEndTurn(AbstractUnitController unit)
    {
        if (onUnitEndTurn != null)
        {
            onUnitEndTurn(unit);
        }
    }

    //Event to refresh a unit's actions
    public event Action<AbstractUnitController> onUnitRefresh;
    public void RefreshUnit(AbstractUnitController unit)
    {
        if (onUnitRefresh != null)
        {
            onUnitRefresh(unit);
        }
    }

    /*
    * Events for moving on to the next team's turn
    */

    //Event for initiating transition to the next team's turn
    public event Action<TeamType> onTurnTransitionOver;
    public void TurnTransitionOver(TeamType team)
    {
        if (onTurnTransitionOver != null)
        {
            onTurnTransitionOver(team);
        }
    }

    //Event to refresh an entire army
    public event Action<Manager.AbstractArmyManager> onRefreshArmy;
    public void RefreshArmy(Manager.AbstractArmyManager armyManager)
    {
        if (onRefreshArmy != null)
        {
            onRefreshArmy(armyManager);
        }
    }
}
