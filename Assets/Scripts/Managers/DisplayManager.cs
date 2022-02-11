using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class DisplayManager : MonoBehaviour
    {
        private Text nameText;
        private Text strengthText;
        private Text precisionText;
        private Text speedText;
        private Text armorText;
        private Text movementText;

        //List of Buttons to Close when Displays are to be closed
        private readonly List<GameObject> buttons = new List<GameObject>();
        public void AddButtonToDisplay(GameObject button) { buttons.Add(button); }

        void Awake()
        {
            nameText = GameObject.Find("UnitName").GetComponent<Text>();
            strengthText = GameObject.Find("StrengthValue").GetComponent<Text>();
            precisionText = GameObject.Find("PrecisionValue").GetComponent<Text>();
            speedText = GameObject.Find("SpeedValue").GetComponent<Text>();
            armorText = GameObject.Find("ArmorValue").GetComponent<Text>();
            movementText = GameObject.Find("MovementValue").GetComponent<Text>();
        }

        //update text for unit and turn display on
        public void DisplayStatForUnit(Unit unit) 
        {       
            nameText.text = unit.Name;
            strengthText.text = unit.Strength.ToString();
            precisionText.text = unit.Precision.ToString();
            speedText.text = unit.Speed.ToString();
            armorText.text = unit.Armor.ToString();
            movementText.text = unit.Movement.ToString();
            gameObject.SetActive(true);
        }

        public void CloseAllDisplays() 
        {
            CloseButtonDisplays();
            CloseStatDisplay();
        }

        public void CloseStatDisplay()
        {
            gameObject.SetActive(false);
        }

        public void CloseButtonDisplays() 
        {
            foreach (GameObject button in buttons) {
                Destroy(button);
            }
            buttons.Clear();
        }
    }
}