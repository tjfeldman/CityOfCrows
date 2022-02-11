using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}