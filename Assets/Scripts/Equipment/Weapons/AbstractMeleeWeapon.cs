using System.Collections;
using System.Collections.Generic;

namespace Weapons {

    public abstract class AbstractMeleeWeapon : AbstractWeapon
    {
        //constants
        public const float ACCURACY = 100.0f;//Melee Weapons always hit
        
        public AbstractMeleeWeapon(string name, float baseDamage, float armorPen, float speedModifier, float range, UnitTypeRestriction restriction) : base(name, baseDamage, armorPen, speedModifier, ACCURACY, range, restriction) {}

        protected override float calculateDamage() {
            float damage = baseDamage;
            if (owner != null) {
                damage += owner.Strength;
            }

            return damage;
        }
        
    }
}