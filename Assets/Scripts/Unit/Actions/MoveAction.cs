using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class MoveAction : UnitAction
    {
        public MoveAction(AbstractUnitController unit) : base(unit) {}

        public override void DoAction()
        {
            Debug.Log("Move Action for " + Unit.ToString());
            EventManager.current.MovementActionForUnit(Unit);
        }

        public override Texture2D GetTexture()
        {
            return Resources.Load("Sprites/Move") as Texture2D;
        }
    }
}
