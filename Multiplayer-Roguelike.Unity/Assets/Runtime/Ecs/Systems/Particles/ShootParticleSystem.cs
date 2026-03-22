using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Pools;

namespace Runtime.Ecs.Systems.Particles
{
    public class ShootParticleSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<ShootParticleEventComponent, ShootParticlePointComponent> _buffer = new();
        private readonly PinnedParticlePool _pool;

        public ShootParticleSystem(PinnedParticlePool pool)
        {
            _pool = pool;
        }

        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var entityId = _buffer.EntityIds[i];
            var shootParticlePointComponent = _buffer.Components2[i];

            _pool.Get(shootParticlePointComponent.Point);

            ComponentManager.RemoveComponent<ShootParticleEventComponent>(entityId);
        }
    }
}
