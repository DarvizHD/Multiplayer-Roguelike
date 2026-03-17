using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Runtime.Pools;

namespace Runtime.Ecs.Systems.Particles
{
    public class DamageParticleSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<DamageParticleEventComponent, PositionComponent> _buffer = new();

        private readonly PositionalParticlePool _pool;

        public DamageParticleSystem(PositionalParticlePool pool)
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
            var positionComponent = _buffer.Components2[i];

            _pool.Get(positionComponent.Position);

            ComponentManager.RemoveComponent<DamageParticleEventComponent>(entityId);
        }
    }
}
