using System.Collections;
using System.Collections.Generic;

public interface ITerrain
{
    string Name { get; }
    int MovementCost { get; }
    AbstractUnitController Unit { get; set; }
}
