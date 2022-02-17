using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitStats;
using Actions;

public abstract class AbstractUnitController : MonoBehaviour
{
    //Unit Name
    protected string unitName;
    public virtual string UnitName { get { return unitName; }}

    //Stats
    protected UnitStat strength;
    protected UnitStat precision;
    protected UnitStat speed;
    protected UnitStat armor;
    protected UnitStat movement;
    protected UnitStat detection;

    //stat getters
    public virtual float Strength { get { return strength.Value; }}
    public virtual float Precision { get { return precision.Value; }}
    public virtual float Speed { get { return speed.Value; }}
    public virtual float Armor { get { return armor.Value; }}
    public virtual float Movement { get { return movement.Value; }}
    public virtual float Detection { get { return detection.Value; }}

    //Actions
    protected bool action = true;
    protected bool move = true;
    protected bool free = true;

    public bool HasMove() { return move && Movement > 0; } //if unit has no movement then they can't move.
    public bool HasAction() { return action; }
    public bool HasFreeAction() { return free; }
    public bool CanAct() {
        return move || action || free;
    }

    //position variables
    protected AbstractTileController startTurnTile;
    protected AbstractTileController currentTile;

    public Vector2Int GetPosition()
    {
        return currentTile.GetPosition();
    }

    //TODO Better initilaze and manage these variables
    public void SetStartingTile(AbstractTileController tile)
    {
        startTurnTile = tile;
        currentTile = tile;
    }

    public void SetCurrentTile(AbstractTileController tile)
    {
        currentTile = tile;
    }

    public void LoadPlayerData(PlayerUnitData data)
    {
        unitName = data.Name;
        strength = new UnitStat(data.Strength);
        precision = new UnitStat(data.Precision);
        speed = new UnitStat(data.Speed);
        armor = new UnitStat(data.Armor);
        movement = new UnitStat(data.Movement);
        detection = new UnitStat(data.Detection);

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Utilities.GetSpriteByName(data.Sprite);

        Debug.Log("Done Loading \"" + unitName + "\"");
    }

    private void Start() 
    {
        //set up event listeners
        EventManager.current.onUnitMovement += Moved;
        EventManager.current.onUndoMovement += UndoMove;
        EventManager.current.onUnitEndTurn += EndTurn;
        EventManager.current.onUnitRefresh += Refresh;
    }

    private void OnDestroy() 
    {
        //remove event listeners
        EventManager.current.onUnitMovement -= Moved;
        EventManager.current.onUndoMovement -= UndoMove;
        EventManager.current.onUnitEndTurn -= EndTurn;
        EventManager.current.onUnitRefresh -= Refresh;
    }

    private void OnMouseDown() 
    {
        Debug.Log("You have clicked on Unit: " + ToString());
        EventManager.current.UnitClicked(this);
    }

    public override string ToString() 
    {
        return unitName + "(" + this.GetType()  + ")";
    }

    public abstract List<UnitAction> GetActions();

    private void Moved(AbstractUnitController unit, MoveTileController tile)
    {
        if (unit == this) {
            move = false;
            EventManager.current.UnitClicked(this);
        }
    }

    private void UndoMove(AbstractUnitController unit, AbstractTileController tile)
    {
        if (unit == this) {
            move = true;
            EventManager.current.UnitClicked(this);
        }
    }

    protected void Refresh(AbstractUnitController unit) 
    {
        if (unit == this) {
            move = true;
            action = true;
            free = true;
            this.startTurnTile = this.currentTile;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    protected void EndTurn(AbstractUnitController unit) 
    {
        if (unit == this) {
            move = false;
            action = false;
            free = false;
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }
}
