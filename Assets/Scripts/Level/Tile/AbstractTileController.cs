using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public abstract class AbstractTileController : MonoBehaviour
{
    //Position variables
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
}
