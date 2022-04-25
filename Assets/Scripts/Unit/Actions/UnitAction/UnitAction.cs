using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public abstract class UnitAction : IAction
    {
        public abstract string Name {get;}

        private Vector2 _pos;
        public Vector2 Position 
        {
            get 
            { 
                return _pos;
            }
            set 
            {
                _pos = value;
            }
        }

        private AbstractUnitController unit;
        public virtual AbstractUnitController Unit { get { return unit; }}

        public UnitAction(AbstractUnitController unit)
        {
            this.unit = unit;
        }

        public abstract void DoAction();
    }
}