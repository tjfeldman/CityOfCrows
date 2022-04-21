using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Actions 
{
    public class ActionController : MonoBehaviour
    {
        private IAction action;

        public void SetAction(IAction action)
        {
            this.action = action;
            this.gameObject.GetComponentInChildren<TextMeshPro>().text = action.Name;
        }

        private void OnMouseDown() 
        {
            if (action != null) 
            {
                action.DoAction();
            }
        }
    }
}