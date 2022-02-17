using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField]
    private int width;
    public virtual int Width {
        get {
            return width;
        }
    }

    [SerializeField]
    private int height;
    public virtual int Height {
        get {
            return height;
        }
    }

    [SerializeField]
    private List<TileData> tiles;
    public virtual ReadOnlyCollection<TileData> Tiles {
        get {
            return tiles.AsReadOnly();
        }
    }

    [SerializeField]
    private List<SpawnerData> spawners;
    public virtual ReadOnlyCollection<SpawnerData> Spawners {
        get {
            return spawners.AsReadOnly();
        }
    }

    public LevelData(int width, int height, List<TileData> tiles, List<SpawnerData> spawners)
    {
        this.width = width;
        this.height = height;
        this.tiles = tiles;
        this.spawners = spawners;
    }

    public override string ToString()
    {
        return "Width: " + width + "\nHeight: " + height + "\nTiles: " + tiles.Count;
    }

}

[System.Serializable]
public class TileData
{
    [SerializeField]
    private string tileName = "Tile";
    public virtual string TileName {
        get {
            return tileName;
        }
    }

    [SerializeField]
    private int movementCost = 1;
    public virtual int MovementCost {
        get {
            return movementCost;
        }
    }


    [SerializeField]
    private int detectionPenalty = 0;
    public virtual int DetectionPenalty {
        get {
            return detectionPenalty;
        }
    }


    [SerializeField]
    private string spriteName;
    public virtual string SpriteName {
        get {
            return spriteName;
        }
    }


    [SerializeField]
    private Vector2Int position;
    public virtual Vector2Int Position {
        get {
            return position;
        }
    }

    [SerializeField]
    private Vector2Int size;
    public virtual Vector2Int Size {
        get {
            return size;
        }
    }


    public TileData(string tileName, int movementCost, int detectionPenalty, string spriteName, Vector2Int position, Vector2Int size)
    {
        this.tileName = tileName;
        this.movementCost = movementCost;
        this.detectionPenalty = detectionPenalty;
        this.spriteName = spriteName;
        this.position = position;
        this.size = size;
    }

}

[System.Serializable]
public class SpawnerData {

    [SerializeField]
    private TeamType type;
    public virtual TeamType Type {
        get {
            return type;
        }
    }

    [SerializeField]
    private Vector2Int position;
    public virtual Vector2Int Position {
        get {
            return position;
        }
    }

    public SpawnerData(TeamType type, Vector2Int position) {
        this.type = type;
        this.position = position;
    }

}