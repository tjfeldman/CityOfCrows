using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class PlayerArmyHandler
{
    private List<Tuple<string, PlayerUnitData>> playerArmy;//list of player units
    public readonly ReadOnlyCollection<Tuple<string, PlayerUnitData>> PlayerArmy;

    public PlayerArmyHandler() 
    {
        playerArmy = new List<Tuple<string, PlayerUnitData>>();
        PlayerArmy = playerArmy.AsReadOnly();

        //load default army
        string prefab = "MainCharacter";
        PlayerUnitData mc = new PlayerUnitData("MC", 15, 6, 6, 6, 0, 5, 5);
        playerArmy.Add(new Tuple<string, PlayerUnitData>(prefab, mc));
    }
    
}
