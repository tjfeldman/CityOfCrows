using System.Collections;
using System.Collections.Generic;

namespace Tiles {

    public class MountainTile : Tile
    {
        public MountainTile() : base((int)TileRestriction.Flier, "MountainTile") { }
    }

}