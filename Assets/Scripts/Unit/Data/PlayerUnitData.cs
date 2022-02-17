using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class PlayerUnitData : UnitData
{
    public PlayerUnitData(string name, float hp, float str, float prc, float spd, float arm, float mov, float detect, string sprite) : base(name, hp, str, prc, spd, arm, mov, detect, sprite) { }
}


[System.Serializable]
public class PlayerArmyData
{
    [SerializeField]
    private List<PlayerUnitData> playerUnits;
    public virtual ReadOnlyCollection<PlayerUnitData> PlayerUnits {
        get {
            return playerUnits.AsReadOnly();
        }
    }

    public PlayerArmyData(List<PlayerUnitData> playerUnits) 
    {
        this.playerUnits = playerUnits;
    }
}
