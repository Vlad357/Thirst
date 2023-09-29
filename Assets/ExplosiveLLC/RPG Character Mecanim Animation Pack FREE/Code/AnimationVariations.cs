using RPGCharacterAnims.Lookups;

namespace RPGCharacterAnims
{
    /// <summary>
    /// This class contains all the variations for given action types.
    /// </summary>
    public class AnimationVariations
    {
        public static readonly KnockbackType[] Knockbacks = { KnockbackType.Knockback1, KnockbackType.Knockback2 };
        public static readonly KnockdownType[] Knockdowns = { KnockdownType.Knockdown1 };
        public static readonly HitType[] Hits = { HitType.Forward1, HitType.Forward2, HitType.Back1, HitType.Left1, HitType.Right1 };

        public static readonly TwoHandedSwordAttack[] TwoHandedSwordAttacks =
        {
            TwoHandedSwordAttack.Attack1
        };

        public static readonly RangeAttack[] rangeAttacks =
        {
            RangeAttack.Attack1
        };

        public static readonly UnarmedAttack[] UnarmedLeftAttacks =
        { UnarmedAttack.LeftAttack1, UnarmedAttack.LeftAttack2, UnarmedAttack.LeftAttack3 };

        public static readonly UnarmedAttack[] UnarmedRightAttacks =
        { UnarmedAttack.RightAttack1, UnarmedAttack.RightAttack2, UnarmedAttack.RightAttack3 };

    }
}