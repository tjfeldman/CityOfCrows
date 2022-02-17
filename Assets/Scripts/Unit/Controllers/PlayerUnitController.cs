using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

public class PlayerUnitController : AbstractUnitController
{
    public override List<UnitAction> GetActions()
    {
        List<UnitAction> actions = new List<UnitAction>();

        //TODO: Have generic action lists in Abstract Unit Controller?
        if (CanAct()) 
        {
            if (HasMove()) {
                actions.Add(new MoveAction(this));
            } else if (Movement > 0) {
                actions.Add(new UndoMoveAction(this, startTurnTile));
            }

            //wait is a default action. All player units can wait
            actions.Add(new WaitAction(this));
        }
        
        return actions;
    }
}
