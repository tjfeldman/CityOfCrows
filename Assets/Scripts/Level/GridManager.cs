using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Tiles;

public class GridManager : MonoBehaviour
{
    private enum Direction
    {
        UP=1,
        LEFT=2,
        DOWN=3,
        RIGHT=4,
    }

    private int width = 12;
    private int height = 6;
    private int tileZ = 1; //set to 1 so every character is shown over it.
    private float tileSize = 1f;

    private GameObject[,] gameGrid;
    private List<GameObject> validMovementOptions;
    private List<GameObject> movementTiles;
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

        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++) 
            {
                //create tile at cell
                Tile tile = new GrassTile();
                if (x % 2 == 0 || y % 2 == 0) {
                    tile = new ForestTile();
                }
                if (y == 0 || y == 5) {
                    tile = new MountainTile();
                }
                if (x > 3 && x < 6 && y < 3) {
                    tile = new DesertTile();
                }
                gameGrid[x, y] = Instantiate(tile.Prefab, new Vector3(x * tileSize, y * tileSize, tileZ), Quaternion.identity);
                gameGrid[x, y].GetComponentInChildren<TileController>().SetTileType(tile);
                gameGrid[x, y].GetComponentInChildren<TileController>().SetPosition(x, y);
                gameGrid[x, y].GetComponentInChildren<TileController>().setDisplayHandler(displayHandler);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = tile + " (X: " + x.ToString() + ", Y: " + y.ToString() + ")";
            }
        }

        //Center Grid
        // float gridW = (width * tileSize) / -2f;
        // float girdH = (height * tileSize) / -2f;
        // transform.position = new Vector2(gridW, girdH);
    }

    public void ShowMovementOptionsForUnit(UnitController unit)
    {
        int movement = (int)unit.Unit.Movement;
        Vector2Int position = unit.GetPosition();

        validMovementOptions = new List<GameObject>();
        for (int i = (int)Direction.UP; i <= (int)Direction.RIGHT; i++)
        {
            checkValidMovementInDirection(validMovementOptions, (Direction)i, position, (Direction)i, movement);
        }

        //draw valid movement options
        GameObject prefab = Resources.Load("ValidMovementTile") as GameObject;
        movementTiles = new List<GameObject>();
        foreach (GameObject tile in validMovementOptions) 
        {
            Vector2Int tilePos = tile.GetComponentInChildren<TileController>().GetPosition();
            GameObject movementTile = Instantiate(prefab, new Vector3(tilePos.x * tileSize, tilePos.y * tileSize, tileZ-1), Quaternion.identity);
            movementTile.GetComponentInChildren<MoveTileController>().Unit = unit;
            movementTile.GetComponentInChildren<MoveTileController>().SetPosition(tilePos.x, tilePos.y);
            movementTile.transform.parent = transform;
            movementTile.gameObject.name = "Movement Tile (X: " + tilePos.x.ToString() + ", Y: " + tilePos.y.ToString() + ")";
            movementTiles.Add(movementTile);
        }

        validMovementOptions.Clear();//empty data so it's not being stored for no reason
    }

    public void MoveUnitToTile(UnitController unit, AbstractTileController tile)
    {
        Vector2Int tilePos = tile.GetPosition();
        unit.gameObject.transform.position = new Vector3(tilePos.x * tileSize, tilePos.y * tileSize);
        unit.SetPosition(tilePos.x, tilePos.y);
        CloseMovementOptionsForUnit(unit);
    }

    private void checkValidMovementInDirection(List<GameObject> validMovementOptions, Direction startingDirection, Vector2Int position, Direction currentDirection, int movementLeft)
    {
        int x = position.x;
        int y = position.y;

        if (movementLeft > 0) {
            if (currentDirection == Direction.UP)
            {
                y++;
            }
            else if (currentDirection == Direction.LEFT)
            {
                x++;
            }
            else if (currentDirection == Direction.DOWN)
            {
                y--;
            }
            else if (currentDirection == Direction.RIGHT)
            {
                x--;
            }

            if (isValidTile(x, y)) {
                GameObject tile = gameGrid[x, y];
                int movementCost = tile.GetComponentInChildren<TileController>().Tile.MovementCost;
                movementLeft -= movementCost;
                //if unit has the movement to move on to the tile, then we can continue
                if (movementLeft >= 0) {
                    //add tile to valid movement options if tile is not already in valid movement options
                    if (!validMovementOptions.Contains(tile)) 
                    {
                        validMovementOptions.Add(tile);
                    }
                    //now continue to check the next tiles in the current direction
                    checkValidMovementInDirection(validMovementOptions, startingDirection, new Vector2Int(x,y), currentDirection, movementLeft);

                    //now check the directions to the side of the tile based on current direction as long as you don't go in the opposite direction
                    //adjacent directions are +1 and +3 from current direction
                    Direction adjacentDirection = currentDirection + 1;
                    if (adjacentDirection > Direction.RIGHT) { adjacentDirection -= Direction.RIGHT; }
                    if (isNotOppositeDirectionFromStarting(adjacentDirection, startingDirection)) {
                        checkValidMovementInDirection(validMovementOptions, startingDirection, new Vector2Int(x,y), adjacentDirection, movementLeft);
                    }

                    adjacentDirection = currentDirection + 3;
                    if (adjacentDirection > Direction.RIGHT) { adjacentDirection -= Direction.RIGHT; }
                    if (isNotOppositeDirectionFromStarting(adjacentDirection, startingDirection)) {
                        checkValidMovementInDirection(validMovementOptions, startingDirection, new Vector2Int(x,y), adjacentDirection, movementLeft);
                    }
                }

            }
        }
    }

    private bool isValidTile(int x, int y)
    {
        return (x >= 0 && x < width) && (y >= 0 && y < height);
    }

    //return true if the direction is the opposite direction from starting direction
    private bool isNotOppositeDirectionFromStarting(Direction direction, Direction startingDirection)
    {
        Direction oppositeDirection = direction + 2; //opposite direction is +2 from direction
        if (oppositeDirection > Direction.RIGHT) { oppositeDirection -= Direction.RIGHT; }
        return startingDirection != oppositeDirection;
    }

    private void CloseMovementOptionsForUnit(UnitController unit) 
    {
        foreach (GameObject movementTile in movementTiles) 
        {
            Destroy(movementTile);
        }
        movementTiles.Clear();
    }

    private void SpawnPlayerArmy() 
    {
        //TODO: Temp For Spawning Unit, but control of player army and spawning should be controlled elsewhere
        int x = (int)Math.Round(width/2f, 4);
        int y = (int)Math.Round(height/2f, 4);
        playerArmyHandler = new PlayerArmyHandler();
        Tuple<string, PlayerUnit> playerInfo = playerArmyHandler.PlayerArmy[0]; 
        GameObject prefab = Resources.Load(playerInfo.Item1) as GameObject;
        GameObject unit = Instantiate(prefab, new Vector3(x,y), Quaternion.identity); //set Z to 1 so it shows over tile
        PlayerUnit pc = playerInfo.Item2;
        unit.GetComponent<UnitController>().SetPosition(x,y);
        unit.GetComponent<UnitController>().SetUnit(pc);
        unit.GetComponent<UnitController>().SetDisplayHandler(displayHandler);
        // gameGrid[x,y].GetComponentInChildren<TileController>().Unit = pc;
            
        //Json Utility Test
        // string json = JsonUtility.ToJson(pc);
        // Debug.Log(json);
    }

    //TODO: Control of Display should be moved elsewhere
    private void setUpDisplayHandler()
    {
        displayHandler = GameObject.Find("UnitDisplay").GetComponent<UnitDisplayHandler>();
        displayHandler.CloseAllDisplays();
    }
}
