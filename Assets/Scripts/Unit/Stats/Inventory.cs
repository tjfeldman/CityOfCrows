using System;
using System.Collections;
using System.Collections.Generic;
using Actions;
using Weapons;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public readonly AbstractUnitController Owner;

    private IWeapon Weapon;

    [SerializeField]
    private string weapon;
    public virtual string weaponType => weapon;

    public Inventory(AbstractUnitController owner)
    {
        this.Owner = owner;
    }

    public Inventory(AbstractUnitController owner, Inventory inventory)
    {
        this.Owner = owner;

        if (inventory.weaponType != null) {
            Type type = Type.GetType(inventory.weaponType);
            if (type != null) {
                Weapon = (IWeapon) Activator.CreateInstance(type);
            }
        }
    }

    public void EquipWeapon(IWeapon weapon)
    {
        this.Weapon = weapon;
        this.weapon = Weapon.GetType().ToString();
    }

    public List<UnitAction> GetActions()
    {
        return new List<UnitAction>();
    }

    public override string ToString()
    {
        string str = "Weapon: ";
        if (Weapon == null) {
            str += "None";
        } else {
            str += Weapon.Name;
        }
        return str;
    }
}
