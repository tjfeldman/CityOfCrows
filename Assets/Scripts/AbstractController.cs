using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract Controller Class to contain default positional functions and fields
public abstract class AbstractController : MonoBehaviour
{
    //Position variables
    private int posX;
    private int posY;

    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public void SetPosition(Vector2Int position)
    {
        posX = position.x;
        posY = position.y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }
}
