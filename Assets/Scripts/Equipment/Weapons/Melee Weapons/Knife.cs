using System.Collections;
using System.Collections.Generic;

namespace Weapons {
    
    public class Knife : AbstractMeleeWeapon
    {
        public const UnitTypeRestriction RESTRICTION = UnitTypeRestriction.ALL;//All units can equip the knife

        public const string NAME = "Knife";

        //Knife Stats
        public const float ATTACK = 1.0f;
        public const float ARMOR_PEN = 0.0f;
        public const float SPEED_MOD = 0.0f;
        public const float RANGE = 1.0f;

        public Knife () : base(NAME, ATTACK, ARMOR_PEN, SPEED_MOD, RANGE, RESTRICTION) {}
    }
}
