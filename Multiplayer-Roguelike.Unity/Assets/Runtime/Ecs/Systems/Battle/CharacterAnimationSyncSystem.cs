using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Constants;

namespace Runtime.Ecs.Systems.Battle
{
    public class CharacterAnimationSyncSystem : BaseSystem
    {
        private QueryBuffer<CharacterNetworkSyncComponent, CharacterNetworkEventComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var characterNetworkSyncComponent = _buffer.Components1[i];
                var characterNetworkEventComponent = _buffer.Components2[i];

                var hasDifferent = !string.Equals(characterNetworkSyncComponent.CharacterSharedModel.EventId.Value, characterNetworkEventComponent.EventId);

                if (hasDifferent)
                {
                    var evt = characterNetworkSyncComponent.CharacterSharedModel.EventId.Value;

                    var isPistolEvent =
                        evt.Contains(WeaponConstants.Events[characterNetworkSyncComponent.CharacterSharedModel.EquippedWeaponSlotId.Value]);

                    if (isPistolEvent)
                    {
                        ComponentManager.AddComponent(entityId, new ShootParticleEventComponent());
                    }

                    characterNetworkEventComponent.EventId = characterNetworkSyncComponent.CharacterSharedModel.EventId.Value;
                    ComponentManager.AddComponent(entityId, new AttackAnimationEventComponent());
                }
            }
        }
    }
}
