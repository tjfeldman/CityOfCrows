using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class MoveAction : UnitAction
    {
        public override string Name => "Move";

        public MoveAction(AbstractUnitController unit) : base(unit) {}

        public override void DoAction()
        {
            Debug.Log("Move Action for " + Unit.ToString());
            EventManager.current.MovementActionForUnit(Unit);
        }
    }
}
