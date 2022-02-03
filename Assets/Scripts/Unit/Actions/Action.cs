using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public abstract class Action : MonoBehaviour
    {
        public UnitController Unit;

        protected abstract void OnMouseDown();
    }
}