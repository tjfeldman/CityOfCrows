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
        private const float UNIT_Z = 0.5f;
        private const float DISPLAY_TILE_Z = 0.0f;

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
        private GameObject AttackTile;
        [SerializeField]
        private GameObject PlayerUnit;
       
        private LevelData levelData;
        private GameObject[,] gameGrid;
        private List<SpawnerData> playerSpawns;
        private List<SpawnerData> enemySpawns;
        private List<SpawnerData> allySpawns;
        private List<GameObject> attackTiles;
        // private List<GameObject> movementTiles;

        //Dictionaries to hold available movement information for a unit
        private Dictionary<AbstractUnitController, List<GameObject>> storedMovementOptions;//store calculated movement options that is cleared when any unit moves
        private Dictionary<AbstractUnitController, List<GameObject>> storedMovementTiles;//stores movement tiles for unit

        //Dictionaries to hold available attack information for a unit
        private Dictionary<AbstractUnitController, List<GameObject>> storedAttackOptions;//store calculated attack options that is cleared when any unit moves

        private void Start()
        {
            //define these lists to prevent future errors
            playerSpawns = new List<SpawnerData>();
            enemySpawns = new List<SpawnerData>();
            allySpawns = new List<SpawnerData>();
            attackTiles = new List<GameObject>();
            // movementTiles = new List<GameObject>();

            storedMovementOptions = new Dictionary<AbstractUnitController, List<GameObject>>();
            storedMovementTiles = new Dictionary<AbstractUnitController, List<GameObject>>();

            storedAttackOptions = new Dictionary<AbstractUnitController, List<GameObject>>();

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
                tile.GetComponentInChildren<TerrainTileController>().Unit = pc;
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
                tile.GetComponentInChildren<TerrainTileController>().Unit = enemy;
                unit.name = enemy.ToString();
            }
        }


        //TODO: Update with Tile System Update
        // public void ShowThreatRangeForUnit(AbstractUnitController unit)
        // {
        //     //if unit is player, then show where it can move and the attack range outside of their max movement
        //     if (typeof(PlayerUnitController).IsInstanceOfType(unit))
        //     {
        //         //First show movement for unit
        //         ShowMovementForUnit(unit);
        //         //Now show attack options that extend outside of movement tiles
        //         ShowAttackRangeOutsideUnitsMovement(unit);                
        //     }
            
        // }

        /*
        * Attack Handling
        */
        public void ShowAttackRangeForUnitWithWeapon(AbstractUnitController unit, Weapons.IWeapon weapon)
        {
            List<GameObject> validAttackOptions = new List<GameObject>();
            GetAttackRangeForWeaponFromPosition(validAttackOptions, weapon, unit.GetPosition());
            foreach (GameObject tile in validAttackOptions)
            {
                TerrainTileController terrainTile = tile.GetComponentInChildren<TerrainTileController>();
                Vector2Int tilePos = terrainTile.GetPosition();
                GameObject attackTile = Instantiate(AttackTile, new Vector3(tilePos.x, tilePos.y, DISPLAY_TILE_Z), Quaternion.identity);
                attackTile.GetComponentInChildren<PreviewAttackController>().SetPosition(tilePos.x, tilePos.y);
                attackTile.GetComponentInChildren<PreviewAttackController>().Unit = unit;
                attackTile.GetComponentInChildren<PreviewAttackController>().TerrainTile = terrainTile;
                attackTile.transform.parent = transform;
                attackTile.gameObject.name = "Attack Tile (X: " + tilePos.x.ToString() + ", Y: " + tilePos.y.ToString() + ")";
                attackTiles.Add(attackTile);
            }
        }

        // private void ShowAttackRangeOutsideUnitsMovement(AbstractUnitController unit) 
        // {
        //     List<GameObject> validMovementOptions = GetMovementOptionsForUnit(unit);
        //     List<GameObject> validAttackOptions = GetAttackOptionsForUnit(unit);
            
        //     foreach (GameObject tile in validAttackOptions)
        //     {
        //         if (!validMovementOptions.Contains(tile))
        //         {
        //             Vector2Int tilePos = tile.GetComponentInChildren<TerrainTileController>().GetPosition();
        //             GameObject attackTile = Instantiate(AttackTile, new Vector3(tilePos.x, tilePos.y, DISPLAY_TILE_Z), Quaternion.identity);
        //             attackTile.GetComponentInChildren<AttackTileController>().SetPosition(tilePos.x, tilePos.y);
        //             attackTile.GetComponentInChildren<AttackTileController>().Unit = unit;
        //             attackTile.transform.parent = transform;
        //             attackTile.gameObject.name = "Attack Tile (X: " + tilePos.x.ToString() + ", Y: " + tilePos.y.ToString() + ")";
        //             attackTiles.Add(attackTile);
        //         }
        //     }
        // }

        //Checks for all possible attack options for unit
        // private List<GameObject> GetAttackOptionsForUnit(AbstractUnitController unit)
        // {
        //     //If stored movement options does not have options for unit, calculate them
        //     if (!storedAttackOptions.ContainsKey(unit)) {
        //         List<GameObject> validMovementOptions = GetMovementOptionsForUnit(unit);
        //         Vector2Int position = unit.GetPosition();

        //         List<GameObject> validAttackOptions = new List<GameObject>();
        //         //check each weapon's attack range
        //         foreach (Weapons.IWeapon weapon in unit.GetWeapons())
        //         {
        //             //Get attack options from unit's location
        //             GetAttackRangeForWeaponFromPosition(validAttackOptions, unit, weapon, position);

        //             //Get attack options from unit's possible movement locations
        //             foreach(GameObject tile in validMovementOptions)
        //             {
        //                 Vector2Int tilePos = tile.GetComponentInChildren<TerrainTileController>().GetPosition();
        //                 GetAttackRangeForWeaponFromPosition(validAttackOptions, unit, weapon, tilePos);
        //             }
        //         }

        //         storedAttackOptions.Add(unit, validAttackOptions);
        //         Debug.Log("Stored Attack Options for Unit: " + unit);
        //     }

        //     return storedAttackOptions[unit];
        // }

        //Checks for all possible attack locations from position
        private void GetAttackRangeForWeaponFromPosition(List<GameObject> validAttackOptions, Weapons.IWeapon weapon, Vector2Int position)
        {
            int range = (int)weapon.Range;
            //TODO: Weapon Requirements such as LoS

            for (int x = -(int)range; x <= range; x++) 
            {
                for (int y = -(int)range; y <= range; y++) 
                {
                    if (x == 0 && y == 0) {
                        continue;//skip since unit can't attack itself
                    }

                    if (Math.Abs(x) + Math.Abs(y) > range) {
                        continue;//skip since unit can't attack pass their max range
                    }

                    Vector2Int tilePos = new Vector2Int(position.x + x, position.y + y);
                    if (isValidTile(tilePos)) {
                        GameObject tile = gameGrid[tilePos.x, tilePos.y];
                        validAttackOptions.Add(tile);
                    }
                }
            }
        }

        /*
        * Movement Handling
        */

        public void ShowMovementForUnit(AbstractUnitController unit)
        {
            if (!unit.HasMove()) 
            {
                return; //if unit can't move do not show valid movement options
            }
            List<GameObject> validMovementOptions = GetMovementOptionsForUnit(unit);

            //draw valid movement options
            List<GameObject> movementTiles = new List<GameObject>();
            foreach (GameObject tile in validMovementOptions) 
            {
                TerrainTileController terrainTile = tile.GetComponentInChildren<TerrainTileController>();
                Vector2Int tilePos = terrainTile.GetPosition();
                GameObject movementTile = Instantiate(MoveTile, new Vector3(tilePos.x, tilePos.y, DISPLAY_TILE_Z), Quaternion.identity);
                movementTile.GetComponentInChildren<PreviewMoveController>().Unit = unit;
                movementTile.GetComponentInChildren<PreviewMoveController>().TerrainTile = terrainTile;
                movementTile.GetComponentInChildren<PreviewMoveController>().SetPosition(tilePos.x, tilePos.y);
                movementTile.transform.parent = transform;
                movementTile.gameObject.name = "Movement Tile (X: " + tilePos.x.ToString() + ", Y: " + tilePos.y.ToString() + ")";
                movementTiles.Add(movementTile);
            }
            storedMovementTiles.Add(unit, movementTiles);
        }

        public void AllowMovementForUnit(AbstractUnitController unit)
        {
            if (storedMovementTiles.ContainsKey(unit))
            {
                List<GameObject> movementTiles = storedMovementTiles[unit];
                foreach (GameObject movementTile in movementTiles)
                {
                    movementTile.GetComponentInChildren<PreviewMoveController>().AllowMovement();
                }
            }
        }

        private void MoveUnitToTile(AbstractUnitController unit, AbstractTileController tile)
        {
            Debug.Log("Moving Unit: " + unit + " to Tile: " + tile);
            Vector2Int unitPos = unit.GetPosition();
            Vector2Int tilePos = tile.GetPosition();

            GameObject oldTerrainTile = gameGrid[unitPos.x,unitPos.y];
            GameObject newTerrainTile = gameGrid[tilePos.x,tilePos.y];
            

            unit.gameObject.transform.parent.position = new Vector3(tilePos.x, tilePos.y);
            unit.SetCurrentTile(newTerrainTile.GetComponentInChildren<AbstractTileController>());
            oldTerrainTile.GetComponentInChildren<TerrainTileController>().Unit = null;//remove unit being on old tile
            newTerrainTile.GetComponentInChildren<TerrainTileController>().Unit = unit;//add unit being on new tile
            CloseMovementForUnit(unit);

            //clear stored movement option dictionary so movement options have to be recalculated since a unit has moved locations
            storedMovementOptions.Clear();
        }

        //Checks for all possible movement options for unit
        private List<GameObject> GetMovementOptionsForUnit(AbstractUnitController unit)
        {
            //If stored movement options does not have options for unit, calculate them
            if (!storedMovementOptions.ContainsKey(unit)) {
                int movement = (int)unit.Movement;
                Vector2Int position = unit.GetPosition();

                List<GameObject> validMovementOptions = new List<GameObject>();
                for (int i = (int)Direction.UP; i <= (int)Direction.RIGHT; i++)
                {
                    checkValidMovementInDirection(unit.GetType(), validMovementOptions, (Direction)i, position, (Direction)i, movement);
                }

                storedMovementOptions.Add(unit, validMovementOptions);
                Debug.Log("Stored Movement Options for Unit: " + unit);
            }

            return storedMovementOptions[unit];
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

        private bool isValidTile(Vector2Int pos)
        {
            return isValidTile(pos.x, pos.y);
        }

        //return true if the direction is the opposite direction from starting direction
        private bool isNotOppositeDirectionFromStarting(Direction direction, Direction startingDirection)
        {
            Direction oppositeDirection = direction + 2; //opposite direction is +2 from direction
            if (oppositeDirection > Direction.RIGHT) { oppositeDirection -= Direction.RIGHT; }
            return startingDirection != oppositeDirection;
        }

        public void CloseAttackOptionsForUnit(AbstractUnitController unit)
        {
            foreach (GameObject attackTile in attackTiles)
            {
                Destroy(attackTile);
            }
            attackTiles.Clear();
        }

        public void CloseMovementForUnit(AbstractUnitController unit) 
        {
            //if able to successfully unit's stored movement tiles then destroy each tile
            if (storedMovementTiles.ContainsKey(unit))
            {
                Debug.Log("Removing Movement Tiles for Unit: " + unit);
                List<GameObject> movementTiles = storedMovementTiles[unit];
                foreach (GameObject movementTile in movementTiles) 
                {
                    Destroy(movementTile);
                }
                storedMovementTiles.Remove(unit);
            }
        }
    }
}