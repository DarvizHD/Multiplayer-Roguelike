using Runtime.Ecs.Components.Battle;
using Runtime.Ecs.Core;

namespace Runtime.Ecs.Systems.Battle
{
    public class AttackCooldownSystem : BaseSystem
    {
        private QueryBuffer<AttackCooldownComponent> _attackCooldownBuffer = new ();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _attackCooldownBuffer);

            for(var i = 0; i < _attackCooldownBuffer.Count; i++)
            {
                var attackCooldownComponent = _attackCooldownBuffer.Components[i];

                if (attackCooldownComponent.CurrentCooldown > 0f)
                {
                    attackCooldownComponent.CurrentCooldown -= deltaTime;
                }
            }
        }
    }
}
