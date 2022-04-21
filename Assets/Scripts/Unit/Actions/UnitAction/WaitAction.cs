using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class WaitAction : UnitAction
    {
        public override string Name => "Wait";

        public WaitAction(AbstractUnitController unit) : base(unit) {}

        public override void DoAction()
        {
            Debug.Log("Wait Action for " + Unit.ToString());
            EventManager.current.UnitEndTurn(Unit);
        }
    }
}
