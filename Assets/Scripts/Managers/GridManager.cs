using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager
{
    public class GridManager : MonoBehaviour
    {
        //Z values for tiles and units
        private const float GRID_TILE_Z = 1.0f;
        private const float MOVE_TILE_Z = 0.5f;
        private const float UNIT_Z = 0.0f;

        private enum Direction
        {
            UP=1,
            LEFT=2,
            DOWN=3,
            RIGHT=4,
        }

        public virtual int Width { get { return levelData.Width; }}
        public virtual int Height { get { return levelData.Height; }}

        [SerializeField]
        private TextAsset LevelJSON;
        [SerializeField]
        private GameObject Tile;
        [SerializeField]
        private GameObject MoveTile;
        [SerializeField]
        private GameObject PlayerUnit;
       
        private LevelData levelData;
        private GameObject[,] gameGrid;
        private List<SpawnerData> playerSpawns;
        private List<SpawnerData> enemySpawns;
        private List<SpawnerData> allySpawns;
        private List<GameObject> movementTiles;

        private void Start()
        {
            //define these lists to prevent future errors
            playerSpawns = new List<SpawnerData>();
            enemySpawns = new List<SpawnerData>();
            allySpawns = new List<SpawnerData>();
            movementTiles = new List<GameObject>();

            if (LevelJSON != null) 
            {
                Debug.Log(LevelJSON.text);
                //find grid manager and start generating grid
                levelData = JsonUtility.FromJson<LevelData>(LevelJSON.text);
                CreateLevel();
            } 
            else 
            {
                Debug.LogError("No Level Json was loaded. Cannot Load Level");
                return;
            }

            EventManager.current.onUnitMovement += MoveUnitToTile;
            EventManager.current.onUndoMovement += MoveUnitToTile;
        }

        private void OnDestroy()
        {
            EventManager.current.onUnitMovement -= MoveUnitToTile;
            EventManager.current.onUndoMovement -= MoveUnitToTile;
            
        }

        private void CreateLevel()
        {
            int width = levelData.Width;
            int height = levelData.Height;
            gameGrid = new GameObject[width, height];

            foreach (TileData tile in levelData.Tiles)
            {
                int x = tile.Position.x;
                int y = tile.Position.y;
                Vector2Int size = tile.Size;

                GameObject newTile = Instantiate(Tile, new Vector3(x, y, GRID_TILE_Z), Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.gameObject.name = tile.TileName + " (X: " + x.ToString() + ", Y: " + y.ToString() + ")";

                //for tiles larger than 1x1 we need to add extra data to the gameGrid
                if (size.x > 1 || size.y > 1) {
                    for (int additionalX = 0; additionalX < size.x; additionalX++) {
                        for (int additionalY = 0; additionalY < size.y; additionalY++) {
                            int newX = x + additionalX;
                            int newY = y + additionalY;
                            gameGrid[newX,newY] = newTile;
                        }
                    }
                } else {
                    //otherwise we just add the tile at location
                    gameGrid[x,y] = newTile;
                }

                TerrainTileController tileController = newTile.GetComponentInChildren<TerrainTileController>();
                tileController.LoadTileData(tile);
                tileController.SetPosition(x,y);
            }

            foreach (SpawnerData spawner in levelData.Spawners) {
                if (spawner.Type == TeamType.Player) {
                    playerSpawns.Add(spawner);
                } else if (spawner.Type == TeamType.Enemy) {
                    enemySpawns.Add(spawner);
                }
            }
        }

        public void SpawnPlayerArmy(ReadOnlyCollection<GameObject> army) 
        {
            //if army is larger than number of player spawns output warning
            if (army.Count > playerSpawns.Count) {
                Debug.LogWarning("Army is bigger than the number of spawns");
            }

            for (int i = 0; i < army.Count && i < playerSpawns.Count; i++) {
                GameObject unit = army[i];
                SpawnerData spawner = playerSpawns[i];
                int x = spawner.Position.x;
                int y = spawner.Position.y;
                GameObject tile = gameGrid[x,y];

                unit.transform.position = new Vector3(x,y,UNIT_Z);
                unit.transform.parent = transform;
                PlayerUnitController pc = unit.GetComponentInChildren<PlayerUnitController>();                
                pc.SetStartingTile(tile.GetComponentInChildren<TerrainTileController>());
                tile.GetComponentInChildren<TerrainTileController>().SetUnitOnTile(pc);
                unit.name = pc.ToString();
            }
        }

        public void SpawnEnemyArmy(ReadOnlyCollection<GameObject> army)
        {
            //if army is larger than number of enemy spawns output warning
            if (army.Count > enemySpawns.Count) {
                Debug.LogWarning("Army is bigger than the number of spawns");
            }

            for (int i = 0; i < army.Count && i < playerSpawns.Count; i++) {
                GameObject unit = army[i];
                SpawnerData spawner = enemySpawns[i];
                int x = spawner.Position.x;
                int y = spawner.Position.y;
                GameObject tile = gameGrid[x,y];

                unit.transform.position = new Vector3(x,y,UNIT_Z);
                unit.transform.parent = transform;
                EnemyUnitController enemy = unit.GetComponentInChildren<EnemyUnitController>();                
                enemy.SetStartingTile(tile.GetComponentInChildren<TerrainTileController>());
                tile.GetComponentInChildren<TerrainTileController>().SetUnitOnTile(enemy);
                unit.name = enemy.ToString();
            }
        }

        public void ShowMovementOptionsForPlayerUnit(PlayerUnitController unit)
        {
            List<GameObject> validMovementOptions = GetMovementOptionsForUnit(unit);
            //draw valid movement options
            foreach (GameObject tile in validMovementOptions) 
            {
                Vector2Int tilePos = tile.GetComponentInChildren<TerrainTileController>().GetPosition();
                GameObject movementTile = Instantiate(MoveTile, new Vector3(tilePos.x, tilePos.y, MOVE_TILE_Z), Quaternion.identity);
                movementTile.GetComponentInChildren<MoveTileController>().Unit = unit;
                movementTile.GetComponentInChildren<MoveTileController>().SetPosition(tilePos.x, tilePos.y);
                movementTile.transform.parent = transform;
                movementTile.gameObject.name = "Movement Tile (X: " + tilePos.x.ToString() + ", Y: " + tilePos.y.ToString() + ")";
                movementTiles.Add(movementTile);
            }
        }

        /*
        FUTURE FUNCTION
        public void GetMovementOptionsForEnemyUnit(EnemyUnitController unit)
        {

        }
        */

        private void MoveUnitToTile(AbstractUnitController unit, AbstractTileController tile)
        {
            Vector2Int tilePos = tile.GetPosition();
            GameObject terrainTile = gameGrid[tilePos.x,tilePos.y];

            unit.gameObject.transform.parent.position = new Vector3(tilePos.x, tilePos.y);
            unit.SetCurrentTile(terrainTile.GetComponentInChildren<AbstractTileController>());
            terrainTile.GetComponentInChildren<TerrainTileController>().SetUnitOnTile(unit);
            CloseMovementOptionsForUnit(unit);
        }

        //Checks for all possible movement options for unit
        private List<GameObject> GetMovementOptionsForUnit(AbstractUnitController unit)
        {
            int movement = (int)unit.Movement;
            Vector2Int position = unit.GetPosition();

            List<GameObject> validMovementOptions = new List<GameObject>();
            for (int i = (int)Direction.UP; i <= (int)Direction.RIGHT; i++)
            {
                checkValidMovementInDirection(unit.GetType(), validMovementOptions, (Direction)i, position, (Direction)i, movement);
            }

            return validMovementOptions;
        }

        //Recursive function that checks step by step in a single direction all possible movements in that direction from starting position
        private void checkValidMovementInDirection(Type unitType, List<GameObject> validMovementOptions, Direction startingDirection, Vector2Int position, Direction currentDirection, int movementLeft)
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
                    AbstractUnitController unit = tile.GetComponentInChildren<TerrainTileController>().Unit;
                    if (unit == null || unitType == unit.GetType()) //only allow movement through units of the same type
                    {
                        int movementCost = tile.GetComponentInChildren<TerrainTileController>().MovementCost;
                        if (movementCost > 0) 
                        {
                            movementLeft -= movementCost;
                            //if unit has the movement to move on to the tile, then we can continue
                            if (movementLeft >= 0) {
                                //add tile to valid movement options if tile does not contain a unit and tile is not already in valid movement options
                                if (unit == null && !validMovementOptions.Contains(tile)) 
                                {
                                    validMovementOptions.Add(tile);
                                }
                                //now continue to check the next tiles in the current direction
                                checkValidMovementInDirection(unitType, validMovementOptions, startingDirection, new Vector2Int(x,y), currentDirection, movementLeft);

                                //now check the directions to the side of the tile based on current direction as long as you don't go in the opposite direction
                                //adjacent directions are +1 and +3 from current direction
                                Direction adjacentDirection = currentDirection + 1;
                                if (adjacentDirection > Direction.RIGHT) { adjacentDirection -= Direction.RIGHT; }
                                if (isNotOppositeDirectionFromStarting(adjacentDirection, startingDirection)) {
                                    checkValidMovementInDirection(unitType, validMovementOptions, startingDirection, new Vector2Int(x,y), adjacentDirection, movementLeft);
                                }

                                adjacentDirection = currentDirection + 3;
                                if (adjacentDirection > Direction.RIGHT) { adjacentDirection -= Direction.RIGHT; }
                                if (isNotOppositeDirectionFromStarting(adjacentDirection, startingDirection)) {
                                    checkValidMovementInDirection(unitType, validMovementOptions, startingDirection, new Vector2Int(x,y), adjacentDirection, movementLeft);
                                }
                            }
                        }
                    }
                }
            }
        }

        //returns true if x and y exist within the grid
        private bool isValidTile(int x, int y)
        {
            return (x >= 0 && x < levelData.Width) && (y >= 0 && y < levelData.Height);
        }

        //return true if the direction is the opposite direction from starting direction
        private bool isNotOppositeDirectionFromStarting(Direction direction, Direction startingDirection)
        {
            Direction oppositeDirection = direction + 2; //opposite direction is +2 from direction
            if (oppositeDirection > Direction.RIGHT) { oppositeDirection -= Direction.RIGHT; }
            return startingDirection != oppositeDirection;
        }

        public void CloseMovementOptionsForUnit(AbstractUnitController unit) 
        {
            foreach (GameObject movementTile in movementTiles) 
            {
                Destroy(movementTile);
            }
            movementTiles.Clear();
        }
    }
}