using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Actions;

//outside namespace so anyone can access team type
//might change later
public enum TeamType 
{
    None=-1,
    Player=0,
    Enemy=2,
    Ally=3,
}

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public TextAsset LevelJSON;

        private GridManager gridManager;
        private DisplayManager displayManager;

        private TeamType currentTurn = TeamType.None;
        
        private AbstractUnitController activeUnit = null; 

        // Start is called before the first frame update
        void Start()
        {
            //set up display manager
            displayManager = this.GetComponentInChildren<DisplayManager>();
            if (displayManager != null) 
            {
                this.GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                displayManager.CloseAllDisplays();
            } 
            else 
            {
                Debug.Log("Unable To Find Display Manager");
                return;
            }

            //set up grid manager
            gridManager = this.GetComponentInChildren<GridManager>();
            if (gridManager != null) 
            {
                //todo
            } else {
                Debug.LogError("Unable To Find the Grid Manager");
                return;
            }

            if (LevelJSON != null) 
            {
                Debug.Log(LevelJSON.text);
                //find grid manager and start generating grid
                LevelData data = JsonUtility.FromJson<LevelData>(LevelJSON.text);
                gridManager.CreateLevel(data);
            } 
            else 
            {
                Debug.LogError("No Level Json was loaded. Cannot Load Level");
                return;
            }

            //set up event listeners
            EventManager.current.onUnitClicked += OnUnitClicked;
            EventManager.current.onTileClicked += OnTileClicked;
            EventManager.current.onMovementAction += MovementActionForUnit;
            EventManager.current.onUnitEndTurn += EndUnitTurn;

            //set current turn to player
            SetTurnToTeam(TeamType.Player);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnDestroy() 
        {
            //remove event listeners on destroy
            EventManager.current.onUnitClicked -= OnUnitClicked;
            EventManager.current.onTileClicked -= OnTileClicked;
            EventManager.current.onMovementAction -= MovementActionForUnit;
            EventManager.current.onUnitEndTurn -= EndUnitTurn;
        }

        private void SetTurnToTeam(TeamType team) 
        {
            //todo show animation of whose turn it is
            currentTurn = team;
        }

        //Turns off displays for active unit is one exists
        private void ResetActiveUnit()
        {
            if (activeUnit) 
            {
                displayManager.CloseButtonDisplays();
                gridManager.CloseMovementOptionsForUnit(activeUnit);
                activeUnit = null;
            }
        }

        //Event Handling
        private void OnUnitClicked(AbstractUnitController unit)
        {
            //Only do things when a unit is clicked on player's turn
            if (currentTurn == TeamType.Player) 
            {
                ResetActiveUnit();
                displayManager.DisplayStatForUnit(unit);

                //if unit clicked is a player, we need to display the actions the player can do for that unit
                if (typeof(PlayerUnitController).IsInstanceOfType(unit))
                {
                    activeUnit = unit;//This unit is now the active unit

                    Vector2Int position = unit.GetPosition();
                    List<UnitAction> actions = unit.GetActions();
                    bool even = actions.Count % 2 == 0;
                    float offset =  even ? 0.75f : 0.0f;//For even numbers we want to add a 0.75 offset from the center. For odd numbers we can place the odd button in the center
                    int above = (int)Math.Ceiling(actions.Count / 2.0f);//the number of buttons above the unit's y position should be half of them (rounded up)
                    float top = (offset + ((above - 1) * 1.5f)) + position.y;//The top position is the offset + 1.5f for each button besides the first one plus the y position
                    for (int x = 0; x < actions.Count; x++)
                    {
                        GameObject buttonPrefab = Resources.Load("Buttons/ActionButton") as GameObject;
                        //we place the buttons starting from the top and going down 1.5f for each one after
                        GameObject button = Instantiate(buttonPrefab, new Vector3(position.x + 1.5f, top - (x * 1.5f), -1), Quaternion.identity);
                        button.gameObject.GetComponentInChildren<ActionController>().SetAction(actions[x]);
                        displayManager.AddButtonToDisplay(button);
                    }
                }
            }
        }

        private void OnTileClicked(AbstractTileController tile)
        {
            if (currentTurn == TeamType.Player)
            {
                ResetActiveUnit();
                Debug.Log("Tile: " + tile);
                displayManager.CloseAllDisplays();
            }
        }

        //Move To Display Manager and Grid Manager?
        private void MovementActionForUnit(AbstractUnitController unit)
        {
            Debug.Log("Doing movement action for unit: " + unit);
            if (typeof(PlayerUnitController).IsInstanceOfType(unit))
            {
                displayManager.CloseButtonDisplays();
                displayManager.CloseAllDisplays();
                gridManager.ShowMovementOptionsForUnit(unit);
            }
            //TO DO Enemy and Ally movement
        }

        private void EndUnitTurn(AbstractUnitController unit)
        {
            ResetActiveUnit();
            displayManager.CloseAllDisplays();
        }
    }
}