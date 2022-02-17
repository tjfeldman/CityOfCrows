using System.Collections;
using System.Collections.Generic;
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
        private Text strengthText;
        private Text precisionText;
        private Text speedText;
        private Text armorText;
        private Text movementText;

        private Text turnText;

        void Start()
        {
            width = this.gameObject.GetComponent<Canvas>().pixelRect.width;
            height = this.gameObject.GetComponent<Canvas>().pixelRect.height;

            StatDisplay = GameObject.Find("StatDisplay");
            TurnDisplay = GameObject.Find("TurnDisplay");
            // turnDisplayWidth = TurnDisplay.GetComponent<Image>().rectTransform.rect.width / this.gameObject.GetComponent<Canvas>().referencePixelsPerUnit;

            nameText = GameObject.Find("UnitName").GetComponent<Text>();
            strengthText = GameObject.Find("StrengthValue").GetComponent<Text>();
            precisionText = GameObject.Find("PrecisionValue").GetComponent<Text>();
            speedText = GameObject.Find("SpeedValue").GetComponent<Text>();
            armorText = GameObject.Find("ArmorValue").GetComponent<Text>();
            movementText = GameObject.Find("MovementValue").GetComponent<Text>();

            turnText = GameObject.Find("TurnText").GetComponent<Text>();

            StatDisplay.SetActive(false);
            TurnDisplay.SetActive(false);
        }

        public void SetWorldCamera(Camera camera) 
        {
            this.gameObject.GetComponent<Canvas>().worldCamera = camera;
            //The Turn Display is set to be in the center by default, so once the worldCamera is set, we can figure out the center by getting it's position
            center = TurnDisplay.transform.position;
        }

        public void CloseDisplay() {
            StatDisplay.SetActive(false);
        }

        //update text for unit and turn display on
        public void DisplayStatForUnit(AbstractUnitController unit) 
        {       
            nameText.text = unit.UnitName;
            strengthText.text = unit.Strength.ToString();
            precisionText.text = unit.Precision.ToString();
            speedText.text = unit.Speed.ToString();
            armorText.text = unit.Armor.ToString();
            movementText.text = unit.Movement.ToString();
            StatDisplay.SetActive(true);
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