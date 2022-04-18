using System.Collections;
using System.Collections.Generic;

namespace Weapons {
    public abstract class AbstractWeapon : IWeapon
    {
        //owner
        protected AbstractUnitController owner = null;//by default a weapon has no owner until it is equipped

        //restriction
        protected readonly UnitTypeRestriction restriction; //only units of this type can equip this melee weapon

        //name
        public readonly string WeaponName;

        //Stats
        protected readonly float baseDamage;//the base damage this weapon provides
        protected readonly float armorPen;//the armor pen this weapon provides
        protected readonly float speedModifier;//the modifier to the owner's speed this weapon provides
        protected readonly float accuracy;//the accuracy of this weapon
        protected readonly float range;//the attack range of this weapon
        
        public AbstractWeapon(string name, float baseDamage, float armorPen, float speedModifier, float accuracy, float range, UnitTypeRestriction restriction) 
        {
            this.WeaponName = name;
            
            this.baseDamage = baseDamage;
            this.armorPen = armorPen;
            this.speedModifier = speedModifier;
            this.accuracy = accuracy;
            this.range = range;

            this.restriction = restriction;
        }

        public UnitTypeRestriction UnitRestriction => restriction;
        public float Damage => calculateDamage();
        public float ArmorPen => armorPen;
        public float SpeedModifier => speedModifier;
        public float Range => range;
        public float Accuracy => accuracy;

        protected abstract float calculateDamage();

        public bool EquipTo(AbstractUnitController unit)
        {
            //TODO: Move check to Restriction Class object
            if (restriction == UnitTypeRestriction.ENEMY_ONLY) {
                if (!typeof(EnemyUnitController).IsInstanceOfType(unit)) {
                    return false;
                }
            }
            owner = unit;
            return true;
        }

        //attempts to unequip weapon from unit and returns True if successful or False if failed
        public bool UnequipFrom(AbstractUnitController unit) {
            if (owner == unit) {
                owner = null;
                return true;
            } else {
                return false;
            }
        }

    }
}