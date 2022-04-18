using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class ArmyData
{
    [SerializeField]
    private List<UnitData> units;
    public virtual ReadOnlyCollection<UnitData> Units {
        get {
            return units.AsReadOnly();
        }
    }

    public ArmyData(List<UnitData> units) 
    {
        this.units = units;
    }
}
