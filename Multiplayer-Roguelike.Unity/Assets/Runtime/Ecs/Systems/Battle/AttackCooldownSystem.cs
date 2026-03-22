using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class AttackCooldownSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<AttackCooldownComponent> _buffer = new();

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var attackCooldownComponent = _buffer.Components[i];

            if (attackCooldownComponent.CurrentCooldown > 0f)
            {
                attackCooldownComponent.CurrentCooldown -= deltaTime;
            }
        }
    }
}
