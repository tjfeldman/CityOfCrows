using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneratePlayerArmyJSON : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<PlayerUnitData> playerArmy = new List<PlayerUnitData>();
        foreach (CharacterBuilder unit in this.GetComponentsInChildren<CharacterBuilder>())
        {
            playerArmy.Add(new PlayerUnitData(unit.UnitName, unit.HitPoints, unit.Strength, unit.Precision, unit.Speed, unit.Armor, unit.Movement, unit.Detection, unit.Texture.name));
        }

        PlayerArmyData data = new PlayerArmyData(playerArmy);
        string json = JsonUtility.ToJson(data);
        string path = "Assets/Resources/Data/Unit/playerArmy.json";

        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(json);
        writer.Close();

        Debug.Log("Level JSON Generated");
        Debug.Log(json);
        
    }
}
