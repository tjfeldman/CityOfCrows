using System.Collections;
using System.Collections.Generic;

namespace Weapons {

    public enum UnitTypeRestriction
    {
        ALL,
        ENEMY_ONLY,
    }

    public interface IWeapon
    {
        //UnitRestriction is a restriction requirement by the weapon on which unit can equip it
        //TODO: Make a WeaponRequirement class that have other requirements to equip a weapon such as a stat requirement
        UnitTypeRestriction UnitRestriction
        {
            get;
        }

        string Name
        {
            get;
        }

        //Damage is the dealt by the weapon
        float Attack
        {
            get;
        }

        //Armor Pen is the amount of armor this weapon ignores
        float ArmorPen
        {
            get;
        }

        //Speed Modifier is the modifier to the speed stat the weapon provides
        float SpeedModifier
        {
            get;
        }

        //Accuracy is the chance the weapon has to hit the target
        float Accuracy
        {
            get;
        }

        //Range is the distance the weapon can attack
        float Range
        {
            get;
        }

        //Calculates the weapon's attack power with the user
        float calculateAttackPower(AbstractUnitController user);
    }
}