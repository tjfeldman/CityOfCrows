using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actions {
    public class AttackAction : UnitAction
    {
        public override string Name => "Attack";

        protected List<IAction> attacks;

        public AttackAction(AbstractUnitController unit) : base(unit)
        {
            attacks = new List<IAction>();
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

        //TODO: Protect the list from being editted?
        public List<IAction> GetWeaponActions()
        {
            return attacks;
        }

        public override void DoAction()
        {
            Debug.Log("Attack Action for " + Unit.ToString());
            EventManager.current.AttackActionForUnit(this, Unit);
        }
    }
}