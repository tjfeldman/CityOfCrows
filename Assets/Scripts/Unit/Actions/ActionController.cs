using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions 
{
    public class ActionController : MonoBehaviour
    {
        private UnitAction action;

        public void SetAction(UnitAction action)
        {
            this.action = action;
            //set texture
            Texture2D tex = action.GetTexture();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
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