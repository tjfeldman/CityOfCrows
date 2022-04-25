using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Actions {
    public class WeaponAction : IAction
    {
        public string Name => weapon.Name;

        private IWeapon weapon;
        private AbstractUnitController unit;

        private Vector2 _pos;
        public Vector2 Position 
        {
            get 
            { 
                return _pos;
            }
            set 
            {
                _pos = value;
            }
        }

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