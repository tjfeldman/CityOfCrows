using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Manager
{
    public abstract class AbstractArmyManager : MonoBehaviour
    {
        [SerializeField]
        protected TextAsset ArmyJSON;

        [SerializeField]
        protected GameObject UnitObj;

        protected List<GameObject> army;//list of player units
        public virtual ReadOnlyCollection<GameObject> Army {
            get {
                return army.AsReadOnly();
            }
        }

        void Awake()
        {
            army = new List<GameObject>();
            if (UnitObj == null) {
                Debug.LogError("Unit not set in " + this.GetType() + ". Cannot load army");
                return;
            }
            if (ArmyJSON != null) 
            {
                LoadArmy();
            }
            else 
            {
                Debug.LogError("No Army Json was found. Could Not Load Army in " + this.GetType());
                return;
            }

            EventManager.current.onRefreshArmy += RefreshAllUnits;
        }

        void OnDestroy()
        {
            EventManager.current.onRefreshArmy -= RefreshAllUnits;
        }

        protected abstract void LoadArmy();

        protected void RefreshAllUnits(AbstractArmyManager armyManager)
        {
            if (armyManager == this)
            {
                Debug.Log("Refreshing All Units in " + this.GetType());
                foreach (GameObject unit in army)
                {
                    EventManager.current.RefreshUnit(unit.GetComponentInChildren<AbstractUnitController>());
                }
            }
        }

        public bool CanArmyAct()
        {
            foreach (GameObject unit in army)
            {
                AbstractUnitController unitController = unit.GetComponentInChildren<AbstractUnitController>();
                //if at least one unit can act, then the unit army can act
                if (unitController.CanAct()) {
                    return true;
                }
            }
            return false; //not a single unit was found to be able to act
        }
    }
}