using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Generator 
{
    public class GenerateLevelJSON : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            int height = 0;
            int width = 0;
            List<TileData> tiles = new List<TileData>();
            List<SpawnerData> spawners = new List<SpawnerData>();
            
            foreach (TileBuilder tile in this.GetComponentsInChildren<TileBuilder>())
            {
                string tileName = tile.TileName;
                int movementCost = tile.MovementCost;
                int detectionPenalty = tile.DetectionPenalty;
                Vector2Int size = tile.Size;

                GameObject child = tile.gameObject;
                string spriteName = child.GetComponentInChildren<SpriteRenderer>().sprite.name;
                //need to remove half the size from the x and y since the bottom left corner is the real position of the tile
                float x = child.transform.position.x - size.x/2f;
                float y = child.transform.position.y - size.y/2f;
                Vector2Int position = new Vector2Int((int)x, (int)y);

                tiles.Add(new TileData(tileName, movementCost, detectionPenalty, spriteName, position, size));

                if (position.x > width) {
                    width = (int)position.x;
                }

                if (position.y > height) {
                    height = (int)position.y;
                }
            }

            height++;
            width++;

            foreach (SpawnPointController spawner in this.GetComponentsInChildren<SpawnPointController>()) 
            {
                GameObject child = spawner.gameObject;

                TeamType type = spawner.Type;
                Vector2Int position = new Vector2Int((int)child.transform.position.x, (int)child.transform.position.y);

                spawners.Add(new SpawnerData(type, position));
            }

            LevelData data = new LevelData(width, height, tiles, spawners);
            string json = JsonUtility.ToJson(data);

            string path = "Assets/Data/Levels/level.json";
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(json);
            writer.Close();

            Debug.Log("Level JSON Generated");
            Debug.Log(json);
        }
    }
}
