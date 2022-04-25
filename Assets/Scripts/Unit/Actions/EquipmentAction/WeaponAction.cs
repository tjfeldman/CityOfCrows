using System.Collections;
using System.Collections.Generic;
using Weapons;

namespace Actions {
    public class WeaponAction : IAction
    {
        public string Name => weapon.Name;

        private IWeapon weapon;
        private AbstractUnitController unit;

        public virtual IWeapon Weapon { get { return weapon; }}
        public virtual AbstractUnitController Unit { get { return unit; }}

        public WeaponAction(IWeapon weapon)
        {
            this.weapon = weapon;
        }

        public void SetWeaponOwner(AbstractUnitController unit) 
        {
            this.unit = unit;
        }

        public void DoAction()
        {
            EventManager.current.WeaponAttackActionForUnitWeapon(unit, weapon);
        }
    }
}