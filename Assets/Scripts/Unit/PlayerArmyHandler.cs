using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

public class PlayerArmyHandler
{
    private List<Tuple<string, PlayerUnit>> playerArmy;//list of player units
    public readonly ReadOnlyCollection<Tuple<string, PlayerUnit>> PlayerArmy;

    public PlayerArmyHandler() 
    {
        playerArmy = new List<Tuple<string, PlayerUnit>>();
        PlayerArmy = playerArmy.AsReadOnly();

        //load default army
        string prefab = "MainCharacter";
        PlayerUnit mc = new PlayerUnit("MC", 15, 6, 6, 6, 0, 4, 4);
        playerArmy.Add(new Tuple<string, PlayerUnit>(prefab, mc));
    }
    
}
