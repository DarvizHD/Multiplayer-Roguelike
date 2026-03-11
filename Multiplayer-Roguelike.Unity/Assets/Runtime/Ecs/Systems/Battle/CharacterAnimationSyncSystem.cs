using Runtime.ECS.Components;
using Runtime.ECS.Components.Network;
using Runtime.ECS.Core;

namespace Runtime.ECS.Systems.Battle
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
                    ComponentManager.AddComponent(entityId, new AttackAnimationEventComponent());
                }
            }
        }
    }
}
