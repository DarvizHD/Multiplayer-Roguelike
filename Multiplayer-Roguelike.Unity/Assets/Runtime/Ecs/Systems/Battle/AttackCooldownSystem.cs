using Runtime.Ecs.Components.Battle;

namespace Runtime.Ecs.Systems.Battle
{
    public class AttackCooldownSystem : BaseSystem
    {
        public AttackCooldownSystem()
        {
            RegisterRequiredComponent(typeof(AttackCooldownComponent));
        }

        public override void Update(float deltaTime)
        {
            foreach (var (entityId, attackCooldownComponent)
                     in ComponentManager.Query<AttackCooldownComponent>())
            {
                if (attackCooldownComponent.CurrentCooldown > 0f)
                {
                    attackCooldownComponent.CurrentCooldown -= deltaTime;
                }
            }
        }
    }
}
