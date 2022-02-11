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
        //todo come up with better way to handle this
        public virtual DisplayManager DisplayManager {
            get {
                return displayManager;
            }
        }

        private TeamType currentTurn = TeamType.None;

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
                gridManager.SetGameManager(this);
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
            EventManager.current.onShowMovement += ShowMovementOptionsForUnit;
            EventManager.current.onUnitMovement += MoveUnitToTile;

            //set current turn to player
            SetTurnToTeam(TeamType.Player);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void SetTurnToTeam(TeamType team) 
        {
            //todo show animation of whose turn it is
            currentTurn = team;
        }


        //Event Handling
        private void OnUnitClicked(AbstractUnitController unit)
        {
            Debug.Log("You have clicked on Unit: " + unit.UnitName);
            displayManager.DisplayStatForUnit(unit);

            if (typeof(PlayerUnitController).IsInstanceOfType(unit))
            {
                Vector2Int position = unit.GetPosition();
                //create buttons
                GameObject movePrefab = Resources.Load("Buttons/MoveButton") as GameObject;
                GameObject waitPrefab = Resources.Load("Buttons/WaitButton") as GameObject;
                GameObject moveButton = Instantiate(movePrefab, new Vector3(position.x + 1.5f, position.y + 1.25f,-1), Quaternion.identity);
                GameObject waitButton = Instantiate(waitPrefab, new Vector3(position.x + 1.5f, position.y,-1), Quaternion.identity);

                //add unit controller to button action
                moveButton.GetComponentInChildren<Action>().Unit = (PlayerUnitController) unit;

                //add button to display handler
                displayManager.AddButtonToDisplay(moveButton);
                displayManager.AddButtonToDisplay(waitButton);
            }
        }

        private void OnTileClicked(AbstractTileController tile)
        {
            Debug.Log("Tile: " + tile);
            displayManager.CloseAllDisplays();
        }

        private void ShowMovementOptionsForUnit(AbstractUnitController unit)
        {
            Debug.Log("Displaying Movement Options for " + unit);
            displayManager.CloseButtonDisplays();
            displayManager.CloseAllDisplays();
            gridManager.ShowMovementOptionsForUnit(unit);
        }

        private void MoveUnitToTile(AbstractUnitController unit, MoveTileController tile)
        {
            gridManager.MoveUnitToTile(unit, tile);
        }
    }
}