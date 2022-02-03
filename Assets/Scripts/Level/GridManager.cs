using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int width = 12;
    private int height = 6;
    private int tileZ = 1; //set to 1 so every character is shown over it.
    private float tileSize = 1f;

    private GameObject[,] gameGrid;
    private PlayerArmyHandler playerArmyHandler;
    private UnitDisplayHandler displayHandler;

    // Start is called before the first frame update
    void Start()
    {
        //Set Up Stat Display Handler
        setUpDisplayHandler();
        //import the level information and send it off to create grid
        CreateGrid();
        //import player army and spawn them in
        SpawnPlayerArmy();
    }

    private void CreateGrid() 
    {
        gameGrid = new GameObject[width, height];
        Debug.Log("Creating Grid of Size: " + width + " x " + height);
        GameObject tilePrefab = Resources.Load("GrassTile") as GameObject;

        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++) 
            {
                //create tile at cell
                gameGrid[x, y] = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize, tileZ), Quaternion.identity);
                gameGrid[x, y].GetComponent<Tile>().SetPosition(x, y);
                gameGrid[x, y].GetComponent<Tile>().setDisplayHandler(displayHandler);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = "Grass Tile (X: " + x.ToString() + ", Y: " + y.ToString() + ")";
            }
        }

        //Center Grid
        // float gridW = (width * tileSize) / -2f;
        // float girdH = (height * tileSize) / -2f;
        // transform.position = new Vector2(gridW, girdH);
    }

    private void SpawnPlayerArmy() 
    {
        playerArmyHandler = new PlayerArmyHandler();
        for (int i = 0; i < playerArmyHandler.PlayerArmy.Count; i++) 
        {
            Tuple<string, PlayerUnit> playerInfo = playerArmyHandler.PlayerArmy[i];
            GameObject prefab = Resources.Load(playerInfo.Item1) as GameObject;
            GameObject unit = Instantiate(prefab, new Vector3(6,1), Quaternion.identity); //set Z to 1 so it shows over tile
            PlayerUnit pc = playerInfo.Item2;
            unit.GetComponent<UnitController>().SetPosition(6,1);
            unit.GetComponent<UnitController>().SetUnit(pc);
            unit.GetComponent<UnitController>().SetDisplayHandler(displayHandler);
            
            //Json Utility Test
            // string json = JsonUtility.ToJson(pc);
            // Debug.Log(json);
        }
    }

    private void setUpDisplayHandler()
    {
        displayHandler = GameObject.Find("UnitDisplay").GetComponent<UnitDisplayHandler>();
        displayHandler.CloseAllDisplays();
    }
}
