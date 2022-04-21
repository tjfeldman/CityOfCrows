using System.Collections;
using System.Collections.Generic;

namespace Weapons {
    
    public class Knife : AbstractMeleeWeapon
    {
        public override string Name => "Knife";
        public override float Attack => 1.0f;
        public override float ArmorPen => 0.0f;
        public override float SpeedModifier => 0.0f;
    }
}
