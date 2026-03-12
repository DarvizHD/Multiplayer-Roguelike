using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;

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
                    characterNetworkEventComponent.EventId = characterNetworkSyncComponent.CharacterSharedModel.EventId.Value;

                    if (characterNetworkSyncComponent.CharacterSharedModel.EventId.Value.Contains("damage"))
                    {
                        ComponentManager.AddComponent(entityId, new DamageAnimationEventComponent());
                    }
                    else
                    {
                        ComponentManager.AddComponent(entityId, new AttackAnimationEventComponent());
                    }
                }
            }
        }
    }
}
