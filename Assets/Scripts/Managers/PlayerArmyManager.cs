using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Manager
{
    public class PlayerArmyManager : AbstractArmyManager
    {
        protected override void LoadArmy()
        {
            Debug.Log(ArmyJSON.text);
            ArmyData data = JsonUtility.FromJson<ArmyData>(ArmyJSON.text);

            foreach (UnitData d in data.Units) {
                Debug.Log("Creating Player Character \"" + d.Name + "\"");
                GameObject unit = Instantiate(UnitObj);
                unit.GetComponentInChildren<PlayerUnitController>().LoadUnitData(d);

                army.Add(unit);
            }
        }
    }
}