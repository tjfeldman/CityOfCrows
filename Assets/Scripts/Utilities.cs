using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class of Static Utility functions to reuse code
public class Utilities
{
    public static Sprite GetSpriteByName(string name)
    {
        Texture2D tex = Resources.Load("Sprites/" + name) as Texture2D;
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public static IEnumerator MoveToPositionOverDuration(GameObject obj, Vector3 endPos, float seconds)
    {
        Debug.Log("Moving To Position over " + seconds + " seconds");
        float elapsedTime = 0;
        Vector3 startPos = obj.transform.position;
        while (elapsedTime < seconds) {
            Debug.Log(elapsedTime + " seconds");
            obj.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        obj.transform.position = endPos;
    }

    public static IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
