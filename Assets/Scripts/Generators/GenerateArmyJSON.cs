using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GenerateArmyJSON : MonoBehaviour
{
    public string path = "Assets/Resources/Data/Unit/army.json";
    // Start is called before the first frame update
    void Start()
    {
        List<UnitData> army = new List<UnitData>();
        foreach (CharacterBuilder unit in this.GetComponentsInChildren<CharacterBuilder>())
        {
            army.Add(new UnitData(unit.UnitName, unit.HitPoints, unit.Strength, unit.Precision, unit.Speed, unit.Armor, unit.Movement, unit.Detection, unit.Texture.name));
        }

        ArmyData data = new ArmyData(army);
        string json = JsonUtility.ToJson(data);

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(json);
        writer.Close();

        Debug.Log("Level JSON Generated");
        Debug.Log(json);
        
    }
}
