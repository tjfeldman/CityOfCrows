using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tiles {

    //Not Sure if this will be how to handle flying vs ground units on terrain, but put it here for now.
    //Didn't feel like the boolean would be enough 
    public enum TileRestriction {
        None=1,
        Flier=100,
        All=1000,
    }
    public abstract class Tile
    {
        public readonly int MovementCost;

        public readonly GameObject Prefab;

        public Tile(int movementCost, string prefabName) 
        {
            MovementCost = movementCost;
            Prefab = Resources.Load(prefabName) as GameObject;
        }

        public override string ToString()
        {
            return this.GetType().ToString();
        }
    }

}