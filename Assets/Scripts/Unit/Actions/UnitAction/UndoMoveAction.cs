using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class UndoMoveAction : UnitAction
    {
        public override string Name => "Undo Move";

        protected AbstractTileController PreviousTile;

        public UndoMoveAction(AbstractUnitController unit, AbstractTileController previousTile) : base(unit) 
        {
            this.PreviousTile = previousTile;
        }

        public override void DoAction()
        {
            Debug.Log("Undo Move Action for " + Unit.ToString());
            EventManager.current.UndoMovement(Unit, PreviousTile);
        }
    }
}