using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public abstract class UnitAction
    {
        private AbstractUnitController unit;
        public virtual AbstractUnitController Unit { get { return unit; }}

        public UnitAction(AbstractUnitController unit)
        {
            this.unit = unit;
        }

        public abstract void DoAction();
        public abstract Texture2D GetTexture();
    }
}