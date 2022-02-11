using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitStats;

public abstract class AbstractUnitController : MonoBehaviour
{
    //Unit Name
    protected string unitName;
    public void setName(string name) { unitName = name; }
    public virtual string UnitName { get { return unitName; }}

    //Stats
    protected UnitStat strength;
    protected UnitStat precision;
    protected UnitStat speed;
    protected UnitStat armor;
    protected UnitStat movement;
    protected UnitStat detection;

    //stat setters
    public void setStrength(float value) { strength = new UnitStat(value); }
    public void setPrecision(float value) { precision = new UnitStat(value); }
    public void setSpeed(float value) { speed = new UnitStat(value); }
    public void setArmor(float value) { armor = new UnitStat(value); }
    public void setMovement(float value) { movement = new UnitStat(value); }
    public void setDetection(float value) { detection = new UnitStat(value); }

    //stat getters
    public virtual float Strength { get { return strength.Value; }}
    public virtual float Precision { get { return precision.Value; }}
    public virtual float Speed { get { return speed.Value; }}
    public virtual float Armor { get { return armor.Value; }}
    public virtual float Movement { get { return movement.Value; }}
    public virtual float Detection { get { return detection.Value; }}

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
        return unitName + "(" + this.GetType()  + ")";
    }

}
