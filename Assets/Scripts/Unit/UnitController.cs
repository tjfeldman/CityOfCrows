using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actions;

public class UnitController : MonoBehaviour
{
    private Unit unit;
    public virtual Unit Unit { get { return unit; } }
    public void SetUnit(Unit unit) {
        if (this.unit == null) {
            this.unit = unit;
        }
        else {
            Debug.Log("Unable to set unit as one already exists");
        }
    }

    //position variables
    private int posX;
    private int posY;
    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    private UnitDisplayHandler displayHandler;
    public void SetDisplayHandler(UnitDisplayHandler displayHandler) { this.displayHandler = displayHandler; }
    
    private void OnMouseDown() 
    {
        Debug.Log("You have clicked on Unit: " + unit.Name);
        //Display Stats in Corner
        displayHandler.DisplayStatForUnit(unit);

        if (typeof(PlayerUnit).IsInstanceOfType(unit))
        {
            Debug.Log("Player Unit clicked, spawning buttons");
            //create buttons
            GameObject movePrefab = Resources.Load("Buttons/MoveButton") as GameObject;
            GameObject waitPrefab = Resources.Load("Buttons/WaitButton") as GameObject;
            GameObject moveButton = Instantiate(movePrefab, new Vector3(posX + 1.5f, posY + 1.25f,-1), Quaternion.identity);
            GameObject waitButton = Instantiate(waitPrefab, new Vector3(posX + 1.5f, posY,-1), Quaternion.identity);

            //add unit controller to button action
            moveButton.GetComponentInChildren<Action>().Unit = this;

            //add button to display handler
            displayHandler.AddButtonToDisplay(moveButton);
            displayHandler.AddButtonToDisplay(waitButton);

            //Set Buttons this Unit for commands
        }
    }

    public void DisplayMovementOptions() 
    {
        Debug.Log("Displaying Movement Options");
        // displayHandler.CloseButtonDisplays();
        displayHandler.CloseAllDisplays();
        GameObject.Find("GridHandler").GetComponent<GridManager>().ShowMovementOptionsForUnit(this);
        
    }

    public override string ToString() 
    {
        return unit.Name + "(" + unit.GetType()  + ")";
    }

}
