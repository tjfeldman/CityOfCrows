using System.Collections;
using System.Collections.Generic;

namespace Weapons {

    public abstract class AbstractMeleeWeapon : AbstractWeapon
    {        
        public override UnitTypeRestriction UnitRestriction => UnitTypeRestriction.ALL;
        public override float Range => 1.0f;
        public override float Accuracy => 100.0f;

        public override float CalculateAttackPower(AbstractUnitController user) {

            float attackPower = Attack;
            if (user != null) {
                attackPower += user.Strength;
            }

            return attackPower;
        }
        
    }
}