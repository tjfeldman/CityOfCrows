using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainTileController : AbstractTileController, ITerrain
{

    //tile values
    private string tileName = "Tile";
    private int movementCost = 1;
    private int detectionPenalty = 0;
    private AbstractUnitController unit = null;

    //getters
    public string Name => tileName;
    public int MovementCost => movementCost;
    public AbstractUnitController Unit {
        get {
            return unit;
        }
        set {
            unit = value;
        }
    }
    
    public void LoadTileData(TileData data)
    {
        tileName = data.TileName;
        movementCost = data.MovementCost;
        detectionPenalty = data.DetectionPenalty;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Utilities.GetSpriteByName(data.SpriteName);

        int sizeX = data.Size.x;
        int sizeY = data.Size.y;
        if (sizeX > 1 || sizeY > 1) {
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(sizeX, sizeY);
            this.gameObject.transform.position += new Vector3(sizeX/2f - 0.5f, sizeY/2f - 0.5f, 0);
        }
    }

    void OnMouseDown() 
    {
        EventManager.current.TileClicked(this);
    }

    public override string ToString()
    {
        return Name;
    }

}
