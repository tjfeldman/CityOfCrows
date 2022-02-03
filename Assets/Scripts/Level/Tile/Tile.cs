using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Private Variables that should never be changed
    private int posX;
    private int posY;

    //Tile Specfic Variables
    private int movementCost = 1; //how much movement
    private bool flierOnly = false;//does the tile prevent non-flying units from passing 

    //Display Handler
    private UnitDisplayHandler displayHandler;
    public void setDisplayHandler(UnitDisplayHandler displayHandler) { this.displayHandler = displayHandler; }
    
    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    protected void SetMovementCost(int cost) 
    {
        movementCost = cost;
    }

    protected void SetFlierOnly(bool flierOnly) {
        this.flierOnly = flierOnly;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    public int GetMovementCost() 
    {
        return movementCost;
    }

    public bool IsFlierOnly() {
        return flierOnly;
    }

    void OnMouseDown() 
    {
        Debug.Log("You have clicked on a Tile");
        //Display Stats in Corner
        displayHandler.CloseAllDisplays();
    }
}
