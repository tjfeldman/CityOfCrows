using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class DisplayManager : MonoBehaviour
    {
        private float width;
        private float height;
        private Vector3 center;

        private GameObject StatDisplay;
        private GameObject TurnDisplay;

        private Text nameText;
        private Text healthText;
        private Text strengthText;
        private Text precisionText;
        private Text speedText;
        private Text armorText;
        private Text movementText;

        private Text turnText;

        [SerializeField]
        private GameObject ActionButtonPrefab;

        //List of Action Buttons being displayed
        private List<GameObject> actionButtons = new List<GameObject>();

        void Awake()
        {
            width = this.gameObject.GetComponent<Canvas>().pixelRect.width;
            height = this.gameObject.GetComponent<Canvas>().pixelRect.height;

            StatDisplay = GameObject.Find("StatDisplay");
            TurnDisplay = GameObject.Find("TurnDisplay");
            center = TurnDisplay.transform.position;
            // turnDisplayWidth = TurnDisplay.GetComponent<Image>().rectTransform.rect.width / this.gameObject.GetComponent<Canvas>().referencePixelsPerUnit;

            nameText = GameObject.Find("UnitName").GetComponent<Text>();
            healthText = GameObject.Find("HealthValue").GetComponent<Text>();
            strengthText = GameObject.Find("StrengthValue").GetComponent<Text>();
            precisionText = GameObject.Find("PrecisionValue").GetComponent<Text>();
            speedText = GameObject.Find("SpeedValue").GetComponent<Text>();
            armorText = GameObject.Find("ArmorValue").GetComponent<Text>();
            movementText = GameObject.Find("MovementValue").GetComponent<Text>();

            turnText = GameObject.Find("TurnText").GetComponent<Text>();

            StatDisplay.SetActive(false);
            TurnDisplay.SetActive(false);
        }

        private void CloseDisplay() {
            Debug.Log("Turning Display Off");
            StatDisplay.SetActive(false);
        }

        //update text for unit and turn display on
        public void DisplayStatForUnit(AbstractUnitController unit) 
        {   
            if (unit != null)
            {
                nameText.text = unit.UnitName;
                healthText.text = unit.CurrentHealth + "/" + unit.MaxHealth;
                strengthText.text = unit.Strength.ToString();
                precisionText.text = unit.Precision.ToString();
                speedText.text = unit.Speed.ToString();
                armorText.text = unit.Armor.ToString();
                movementText.text = unit.Movement.ToString();
                StatDisplay.SetActive(true);
            } else {
                CloseDisplay();//close display if no unit was given
            }
        }

        //display action buttons next to location
        public void DisplayActionsAt(List<Actions.IAction> actions, Vector2 position)
        {
            bool even = actions.Count % 2 == 0;
            //TODO: Buttons could appear off screen depending on where the player is. Should handle that.
            float offset =  even ? 0.75f : 0.0f;//For even numbers we want to add a 0.75 offset from the center. For odd numbers we can place the odd button in the center
            int above = (int)Math.Ceiling(actions.Count / 2.0f);//the number of buttons above the unit's y position should be half of them (rounded up)
            float top = (offset + ((above - 1) * 1.5f)) + position.y;//The top position is the offset + 1.5f for each button besides the first one plus the y position
            for (int x = 0; x < actions.Count; x++)
            {
                //we place the buttons starting from the top and going down 1.5f for each one after
                Vector2 buttonPos = new Vector2(position.x + 1.5f, top - (x * 1.5f));
                GameObject button = Instantiate(ActionButtonPrefab, new Vector3(buttonPos.x, buttonPos.y, -1), Quaternion.identity);
                button.gameObject.GetComponentInChildren<Actions.ActionController>().SetAction(actions[x]);
                actions[x].Position = buttonPos;
                actionButtons.Add(button);
            }
        }

        public void RemoveActionButtons() 
        {
            foreach (GameObject button in actionButtons) {
                Destroy(button);
            }
            actionButtons.Clear();
        }

        //Display Turn Display
        public void TurnTransitionForTeam(TeamType turn, Color color)
        {
            turnText.text = turn.ToString() + " Turn";
            turnText.color = color;
            Debug.Log("Now transitioning to " + turnText.text);

            //Using the World Vector set the Turn Display to just off the left side of the screen
            TurnDisplay.transform.position = new Vector3(-14, center.y, 0);//Need to figure out way to calculate this
            TurnDisplay.SetActive(true);

            StartCoroutine(MoveTurnDisplay(0.5f, turn));
        }

        private IEnumerator MoveTurnDisplay(float seconds, TeamType turn)
        {
            //move to center over x seconds
            float elapsedTime = 0;
            Vector3 startingPos = TurnDisplay.transform.position;
            while (elapsedTime < seconds) {
                TurnDisplay.transform.position = Vector3.Lerp(startingPos, center, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            TurnDisplay.transform.position = center;
            yield return new WaitForSeconds(seconds * 2f);

            //now move off screen over x seconds
            Vector3 endPos = new Vector3(30, center.y, 0);
            elapsedTime = 0;
            while (elapsedTime < seconds) {
                TurnDisplay.transform.position = Vector3.Lerp(center, endPos, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            TurnDisplay.transform.position = endPos;

            EventManager.current.TurnTransitionOver(turn);
        }
    }
}