using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class AttackAction : UnitAction
    {
        public override string Name => "Attack";

        protected List<WeaponAction> attacks;

        public AttackAction(AbstractUnitController unit) : base(unit)
        {
            attacks = new List<WeaponAction>();
        }

        //Adds a Weapon Action to the list of attacks
        public void AddAttack(WeaponAction attack)
        {
            attack.SetWeaponOwner(Unit);
            attacks.Add(attack);
        }

        //returns true if AttackAction has at least 1 attack
        public bool HasAttacks()
        {
            return attacks.Count > 0;
        }

        public override void DoAction()
        {
            Debug.Log("Attack Action for " + Unit.ToString());
            EventManager.current.AttackActionForUnit(Unit, attacks);
        }
    }
}