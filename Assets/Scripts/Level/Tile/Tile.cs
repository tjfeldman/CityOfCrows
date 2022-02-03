using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles {

    public abstract class Tile
    {
        public readonly int MovementCost;
        public readonly bool FlierOnly;

        public readonly GameObject Prefab;

        public Tile(int movementCost, bool flierOnly, string prefabName) 
        {
            MovementCost = movementCost;
            FlierOnly = flierOnly;
            Prefab = Resources.Load(prefabName) as GameObject;
        }
    }

}