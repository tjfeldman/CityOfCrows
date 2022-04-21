using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    string Name { get; }
    void DoAction();
}
