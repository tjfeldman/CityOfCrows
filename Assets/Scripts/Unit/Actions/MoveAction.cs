using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class MoveAction : Action
    {
        protected override void OnMouseDown()
        {
            Debug.Log("Move Action for " + Unit.ToString() + " pressed");
            EventManager.current.ShowMovementForUnit(Unit);
        }
    }
}
