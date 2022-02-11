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
        private enum Direction
        {
            UP=1,
            LEFT=2,
            DOWN=3,
            RIGHT=4,
        }

        public GameObject Tile;
        public GameObject MoveTile;

        private int width;
        private int height;
        private int tileZ = 1; //set to 1 so every character is shown over it.

        private GameManager gameManager;
        private GameObject[,] gameGrid;
        private List<GameObject> validMovementOptions;
        private List<GameObject> movementTiles;
        private PlayerArmyHandler playerArmyHandler;

        public void SetGameManager(GameManager gameManager) {
            this.gameManager = gameManager;
        }

        public void CreateLevel(LevelData level)
        {
            width = level.Width;
            height = level.Height;
            gameGrid = new GameObject[width, height];

            foreach (TileData tile in level.Tiles)
            {
                int x = tile.Position.x;
                int y = tile.Position.y;
                float size = tile.Size;

                GameObject newTile = Instantiate(Tile, new Vector3(x, y, tileZ), Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.gameObject.name = tile.TileName + " (X: " + x.ToString() + ", Y: " + y.ToString() + ")";

                if (size > 1) {
                    //for larger tiles we need to add extra data to the game grid
                    for (int additionalX = 0; additionalX < size; additionalX++) {
                        for (int additionalY = 0; additionalY < size; additionalY++) {
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
                tileController.TileName = tile.TileName;
                tileController.MovementCost = tile.MovementCost;
                tileController.DetectionPenalty = tile.DetectionPenalty;
                tileController.SpriteName = tile.SpriteName;
                tileController.Size = size;
                tileController.SetPosition(x,y);
            }

            //spawn player and enemies
            foreach (SpawnerData spawner in level.Spawners) 
            {
                if (spawner.Type == TeamType.Player) 
                {
                    SpawnPlayerAt(spawner.Position);
                }
            }
        }

        public void ShowMovementOptionsForUnit(AbstractUnitController unit)
        {
            int movement = (int)unit.Movement;
            Vector2Int position = unit.GetPosition();

            validMovementOptions = new List<GameObject>();
            for (int i = (int)Direction.UP; i <= (int)Direction.RIGHT; i++)
            {
                checkValidMovementInDirection(validMovementOptions, (Direction)i, position, (Direction)i, movement);
            }

            //draw valid movement options
            movementTiles = new List<GameObject>();
            foreach (GameObject tile in validMovementOptions) 
            {
                Vector2Int tilePos = tile.GetComponentInChildren<TerrainTileController>().GetPosition();
                GameObject movementTile = Instantiate(MoveTile, new Vector3(tilePos.x, tilePos.y, tileZ-1), Quaternion.identity);
                movementTile.GetComponentInChildren<MoveTileController>().Unit = unit;
                movementTile.GetComponentInChildren<MoveTileController>().SetPosition(tilePos.x, tilePos.y);
                movementTile.transform.parent = transform;
                movementTile.gameObject.name = "Movement Tile (X: " + tilePos.x.ToString() + ", Y: " + tilePos.y.ToString() + ")";
                movementTiles.Add(movementTile);
            }

            validMovementOptions.Clear();//empty data so it's not being stored for no reason
        }

        public void MoveUnitToTile(AbstractUnitController unit, AbstractTileController tile)
        {
            Vector2Int tilePos = tile.GetPosition();
            unit.gameObject.transform.parent.position = new Vector3(tilePos.x, tilePos.y);
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
                    int movementCost = tile.GetComponentInChildren<TerrainTileController>().MovementCost;
                    if (movementCost > 0) 
                    {
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

        private void CloseMovementOptionsForUnit(AbstractUnitController unit) 
        {
            foreach (GameObject movementTile in movementTiles) 
            {
                Destroy(movementTile);
            }
            movementTiles.Clear();
        }

        private void SpawnPlayerAt(Vector2Int position)
        {
            int x = position.x;
            int y = position.y;

            playerArmyHandler = new PlayerArmyHandler();
            Tuple<string, PlayerUnitData> playerInfo = playerArmyHandler.PlayerArmy[0]; 
            GameObject prefab = Resources.Load(playerInfo.Item1) as GameObject;
            GameObject unit = Instantiate(prefab, new Vector3(x,y,0), Quaternion.identity); //set Z to 1 so it shows over tile
            unit.transform.parent = transform;
            PlayerUnitController pc = unit.GetComponentInChildren<PlayerUnitController>();
            PlayerUnitData data = playerInfo.Item2;

            Debug.Log("Data = " + data);

            pc.setStrength(data.Strength);
            pc.setPrecision(data.Precision);
            pc.setSpeed(data.Speed);
            pc.setArmor(data.Armor);
            pc.setMovement(data.Movement);
            pc.setDetection(data.Precision);
            pc.SetPosition(x,y);
        }
    }
}