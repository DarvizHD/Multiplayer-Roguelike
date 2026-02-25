using Runtime.ECS.Components.Battle;

namespace Runtime.ECS.Systems.Battle
{
    public class AttackCooldownSystem : BaseSystem
    {
        public AttackCooldownSystem()
        {
            RegisterRequiredComponent(typeof(AttackCooldownComponent));
        }

        protected override void Update(int id, object[] components, float deltaTime)
        {
            var attackCooldownComponent = components[0] as AttackCooldownComponent;

            attackCooldownComponent.CurrentCooldown -= deltaTime;
        }
    }
}
