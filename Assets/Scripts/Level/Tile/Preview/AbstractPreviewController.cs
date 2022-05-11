using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPreviewController : AbstractController
{
    public AbstractUnitController Unit;
    public TerrainTileController TerrainTile;

    void OnMouseDown()
    {
        DoAction();
    }

    protected abstract void DoAction();
}
