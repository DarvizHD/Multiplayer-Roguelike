using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

namespace Runtime.ECS.Systems.Particles
{
    public class ShootParticleSystem : BaseSystem
    {
        private QueryBuffer<ShootParticleEventComponent, ShootParticlePointComponent> _buffer = new();

        private readonly PinnedParticlePool _pool;

        public ShootParticleSystem(PinnedParticlePool pool)
        {
            _pool = pool;
        }

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var shootParticlePointComponent = _buffer.Components2[i];

                _pool.Get(shootParticlePointComponent.Point);

                ComponentManager.RemoveComponent<ShootParticleEventComponent>(entityId);
            }
        }
    }
}
