using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractUnitController : MonoBehaviour
{
    private Unit unit;
    public virtual Unit Unit { get { return unit; } }
    public void SetUnit(Unit unit) {
        if (this.unit == null) {
            this.unit = unit;
        }
        else {
            Debug.Log("Unable to set unit as one already exists");
        }
    }

}
