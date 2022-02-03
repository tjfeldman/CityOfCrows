using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiles;

public class TileController : MonoBehaviour
{
    //Position variables
    private int posX;
    private int posY;

    //Tile Type
    private Tile tile; //private, because a tile shouldn't be able to change the tile type
    public virtual Tile Tile {
        get {
            return tile;
        }
    }
    public void SetTileType(Tile tile) { this.tile = tile; }
    
    //Unit On Tile
    public Unit Unit;

    //Display Handler
    private UnitDisplayHandler displayHandler;
    public void setDisplayHandler(UnitDisplayHandler displayHandler) { this.displayHandler = displayHandler; }
    
    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    void OnMouseDown() 
    {
        Debug.Log("Tile: " + tile.GetType());
        //Display Stats in Corner
        displayHandler.CloseAllDisplays();
    }
}
