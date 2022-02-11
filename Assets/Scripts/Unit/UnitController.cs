using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitController : MonoBehaviour
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

    //position variables
    private int posX;
    private int posY;
    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }
  
    private void OnMouseDown() 
    {
        EventManager.current.UnitClicked(this);
    }

    public override string ToString() 
    {
        return unit.Name + "(" + unit.GetType()  + ")";
    }

}
