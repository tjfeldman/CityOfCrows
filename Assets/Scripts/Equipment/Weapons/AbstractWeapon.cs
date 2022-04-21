using System.Collections;
using System.Collections.Generic;
using Actions;

namespace Weapons {

    public abstract class AbstractWeapon : IWeapon
    {
        public abstract UnitTypeRestriction UnitRestriction {get;}
        public abstract string Name {get;}
        public abstract float Attack {get;}
        public abstract float ArmorPen {get;}
        public abstract float SpeedModifier {get;}
        public abstract float Range {get;}
        public abstract float Accuracy {get;}

        public abstract float CalculateAttackPower(AbstractUnitController user);

        public List<IAction> GetActions() 
        {
            return new List<IAction>() { new WeaponAction(this) };//by default all weapons have a weapon action AKA an attack
        }

    }
}