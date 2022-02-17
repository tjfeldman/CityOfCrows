using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuilder : MonoBehaviour
{
    public string UnitName;
    public float HitPoints;
    public float Strength;
    public float Precision;
    public float Speed;
    public float Armor;
    public float Movement;
    public float Detection;
    public Texture2D Texture;

    public override string ToString()
    {
        return "Name: " + UnitName + " Texture: " + Texture.ToString();
    }
}
