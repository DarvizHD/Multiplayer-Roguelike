using Runtime.ECS.Components;
using UnityEngine;

namespace Runtime.Ecs.Components.Battle.Weapon
{
    public class MeleeAttackComponent : IComponent
    {
        public float Damage { get; set; }
        public float Range { get; set; }
        public float Angle { get; set; }
        public AudioClip AttackClip { get; }

        public MeleeAttackComponent(float damage, float range, AudioClip attackClip, float angle = 90f)
        {
            Damage = damage;
            Range = range;
            AttackClip = attackClip;
            Angle = angle;
        }
    }
}
