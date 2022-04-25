using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

//outside namespace so anyone can access team type
//might change later
public enum TeamType 
{
    None=-1,
    Player,
    Enemy,
    Ally,
    EndOfTeams,
}

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        private GridManager gridManager;
        private DisplayManager displayManager;
        private PlayerArmyManager playerArmyManager;
        private EnemyArmyManager enemyArmyManager;
        private bool noAllies = true;//ally manager

        private TeamType currentTurn = TeamType.None;

        [System.Serializable]
        private class TeamColor {
            public TeamType Team;
            public Color Color;

            public TeamColor(TeamType team, Color color)
            {
                this.Team = team;
                this.Color = color;
            }
        }
        [SerializeField]
        private List<TeamColor> teamColors;
        
        private AbstractUnitController activeUnit = null; 
        private bool attackActionActive = false; //prevent spawning multiple weapon buttons

        // Start is called before the first frame update
        void Start()
        {
            //set up display manager
            displayManager = this.GetComponentInChildren<DisplayManager>();
            if (displayManager != null) 
            {
                //do stuff here?
            } 
            else 
            {
                Debug.Log("Unable To Find Display Manager");
                return;
            }

            //set up player army manager
            playerArmyManager = this.GetComponentInChildren<PlayerArmyManager>();
            if (playerArmyManager == null) 
            {
                Debug.Log("Unable to Find Player Army Manager");
                return;
            }

            //set up enemy army manager
            enemyArmyManager = this.GetComponentInChildren<EnemyArmyManager>();
            if (enemyArmyManager == null) 
            {
                Debug.Log("Unable to Find Enemy Army Manager");
                return;
            }

            //set up grid manager
            gridManager = this.GetComponentInChildren<GridManager>();
            if (gridManager != null) 
            {
                gridManager.SpawnPlayerArmy(playerArmyManager.Army);
                gridManager.SpawnEnemyArmy(enemyArmyManager.Army);
            } else {
                Debug.LogError("Unable To Find the Grid Manager");
                return;
            }

            //set up event listeners
            EventManager.current.onUnitClicked += OnUnitClicked;
            EventManager.current.onTileClicked += OnTileClicked;
            EventManager.current.onAttackAction += AttackActionForUnit;
            EventManager.current.onWeaponAttack += WeaponActionForUnit;
            EventManager.current.onMovementAction += MovementActionForUnit;
            EventManager.current.onUnitEndTurn += EndUnitTurn;
            EventManager.current.onTurnTransitionOver += SetTurnToTeam;

            SetTurnToTeam(TeamType.Player);
        }

        // Update is called once per frame
        void Update()
        {
            if (currentTurn == TeamType.Player)
            {
                //Update during the player turn
                if (!playerArmyManager.CanArmyAct())
                {
                    //if the player army cannot act, time to change the turn team
                    changeTurnTeam();
                }
            }
            else if (currentTurn == TeamType.Enemy)
            {
                //TODO
                changeTurnTeam();
            }
        }

        private void OnDestroy() 
        {
            //remove event listeners on destroy
            EventManager.current.onUnitClicked -= OnUnitClicked;
            EventManager.current.onTileClicked -= OnTileClicked;
            EventManager.current.onAttackAction -= AttackActionForUnit;
            EventManager.current.onWeaponAttack -= WeaponActionForUnit;
            EventManager.current.onMovementAction -= MovementActionForUnit;
            EventManager.current.onUnitEndTurn -= EndUnitTurn;
            EventManager.current.onTurnTransitionOver -= SetTurnToTeam;
        }

        private void changeTurnTeam()
        {
            TeamType previousTurn = currentTurn;
            TeamType nextTurn = currentTurn + 1;
            currentTurn = TeamType.None;//set to none during transition
            //set back to player if there are no allies
            if (noAllies && nextTurn == TeamType.Ally) {
                nextTurn = TeamType.Player;
            }
            //set back to player if there are no more next teams
            else if (nextTurn == TeamType.EndOfTeams) {
                nextTurn = TeamType.Player;
            }

            //refresh army of the current team
            if (previousTurn == TeamType.Player) {
                //refresh player units
                EventManager.current.RefreshArmy(playerArmyManager);
            }

            Color color = Color.white;//default to white if no team color was set
            foreach (TeamColor teamColor in teamColors) {
                if (teamColor.Team == nextTurn) {
                    color = teamColor.Color;
                    break;
                }
            }

            displayManager.TurnTransitionForTeam(nextTurn, color);
        }

        private void SetTurnToTeam(TeamType team) 
        {
            currentTurn = team;
            Debug.Log("It is now the " + team.ToString() + " turn");
        }

        //Turns off displays for active unit is one exists
        private void ResetActiveUnit()
        {
            if (activeUnit) 
            {
                attackActionActive = false;
                displayManager.RemoveActionButtons();
                displayManager.CloseDisplay();
                gridManager.CloseAttackOptionsForUnit(activeUnit);
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
                Debug.Log(unit.DisplayInventoryContents()); //TODO: Display Equipment in HUD

                //if unit clicked is a player, we need to display the actions the player can do for that unit
                if (typeof(PlayerUnitController).IsInstanceOfType(unit))
                {
                    activeUnit = unit;//This unit is now the active unit
                    displayManager.DisplayActionsAt(unit.GetActions(), unit.GetPosition());
                }
            }
        }

        private void OnTileClicked(AbstractTileController tile)
        {
            if (currentTurn == TeamType.Player)
            {
                ResetActiveUnit();
                Debug.Log("Tile: " + tile);
                //to do show tile display
            }
        }

        private void AttackActionForUnit(Actions.AttackAction action, AbstractUnitController unit)
        {
            Debug.Log("Attack action for unit: " + unit);
            if (!attackActionActive && typeof(PlayerUnitController).IsInstanceOfType(unit) && unit == activeUnit)
            {
                //if unit is player, show attack options as a new submenu.
                displayManager.DisplayActionsAt(action.GetWeaponActions(), new Vector2(action.Position.x + 1.0f, action.Position.y));
                attackActionActive = true;
            }
        }

        private void WeaponActionForUnit(AbstractUnitController unit, Weapons.IWeapon weapon) 
        {
            Debug.Log("Doing Weapon Action for unit: " + unit + " with weapon: " + weapon);
            if (typeof(PlayerUnitController).IsInstanceOfType(unit) && unit == activeUnit)
            {
                displayManager.RemoveActionButtons();
                gridManager.ShowAttackRangeForPlayerUnitWithWeapon((PlayerUnitController)unit, weapon);
            }
        }

        //Move To Display Manager and Grid Manager?
        private void MovementActionForUnit(AbstractUnitController unit)
        {
            Debug.Log("Doing movement action for unit: " + unit);
            if (typeof(PlayerUnitController).IsInstanceOfType(unit) && unit == activeUnit)
            {
                displayManager.RemoveActionButtons();
                gridManager.ShowMovementOptionsForPlayerUnit((PlayerUnitController)unit);
            }
            //TO DO Enemy and Ally movement
        }

        private void EndUnitTurn(AbstractUnitController unit)
        {
            ResetActiveUnit();
        }
    }
}