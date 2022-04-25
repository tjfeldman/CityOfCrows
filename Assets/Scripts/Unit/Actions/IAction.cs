using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public interface IAction
    {
        string Name { get; }
        Vector2 Position { get; set; }
        void DoAction();
    }
}