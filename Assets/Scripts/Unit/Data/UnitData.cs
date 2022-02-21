using System.Collections;
using System.Collections.Generic;
using UnitStats;
using UnityEngine;

[System.Serializable]
public abstract class UnitData
{
    //Unit Name
    [SerializeField]
    protected string name;

    //Stats
    [SerializeField]
    protected HealthSystem health;
    [SerializeField]
    protected UnitStat strength;
    [SerializeField]
    protected UnitStat precision;
    [SerializeField]
    protected UnitStat speed;
    [SerializeField]
    protected UnitStat armor;
    [SerializeField]
    protected UnitStat movement;
    [SerializeField]
    protected UnitStat detection;

    [SerializeField]
    protected string sprite;
    public virtual string Sprite { get { return sprite; }}

    //Stat Getters
    public virtual string Name { get { return name; } }
    public virtual float Health { get { return health.MaxHealthValue; }}
    public virtual float Strength { get { return strength.Value; } }
    public virtual float Precision { get { return precision.Value; } }
    public virtual float Speed { get { return speed.Value; } }
    public virtual float Armor { get { return armor.Value; } }
    public virtual float Movement { get { return movement.Value; } }
    public virtual float Detection { get { return detection.Value; } }

    public UnitData(string name, float hp, float str, float prc, float spd, float arm, float mov, float detect, string sprite) 
    {
        this.name = name;
        this.health = new HealthSystem(hp);
        this.strength = new UnitStat(str);
        this.precision = new UnitStat(prc);
        this.speed = new UnitStat(spd);
        this.armor = new UnitStat(arm);
        this.movement = new UnitStat(mov);
        this.detection = new UnitStat(detect);
        this.sprite = sprite;
    }   

}
