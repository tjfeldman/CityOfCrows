using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

[System.Serializable]
public class TerrainTileController : AbstractTileController
{
    public string TileName = "Tile";
    public int MovementCost = 1;
    public int DetectionPenalty = 0;
    public string SpriteName;
    public float Size = 1f;
    public bool AutoSize = false;

    void Start() 
    {
        if (!string.IsNullOrEmpty(SpriteName)) {
            Texture2D tex = Resources.Load("Sprites/" + SpriteName) as Texture2D;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        if (AutoSize && Size > 1) {
            this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Size, Size);
            this.gameObject.transform.position += new Vector3(Size/2f - 0.5f, Size/2f - 0.5f, 0);
        }
    }

    private DisplayManager displayManager;
    public void setDisplayManager(DisplayManager displayManager) { this.displayManager = displayManager; }
    
    void OnMouseDown() 
    {
        Debug.Log("Tile: " + TileName);
        if (displayManager != null) 
        {
            displayManager.CloseAllDisplays();
        }
    }

}
