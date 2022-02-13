using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class WaitAction : UnitAction
    {
        public WaitAction(AbstractUnitController unit) : base(unit) {}

        public override void DoAction()
        {
            Debug.Log("Wait Action for " + Unit.ToString());
            EventManager.current.UnitEndTurn(Unit);
        }

        public override Texture2D GetTexture()
        {
            return Resources.Load("Sprites/Wait") as Texture2D;
        }
    }
}
