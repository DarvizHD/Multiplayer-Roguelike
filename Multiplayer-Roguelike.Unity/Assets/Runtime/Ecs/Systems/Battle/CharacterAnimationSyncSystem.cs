using Runtime.Ecs.Components;
using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Components.Particles;
using Runtime.Ecs.Components.Sound;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Constants;

namespace Runtime.Ecs.Systems.Battle
{
    public class CharacterAnimationSyncSystem : BaseSystem
    {
        private QueryBuffer<CharacterNetworkSyncComponent, CharacterNetworkEventComponent, WeaponSlotsComponent> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var characterNetworkSyncComponent = _buffer.Components1[i];
                var characterNetworkEventComponent = _buffer.Components2[i];
                var weaponSlotsComponent = _buffer.Components3[i];

                var hasDifferent = !string.Equals(characterNetworkSyncComponent.CharacterSharedModel.EventId.Value, characterNetworkEventComponent.EventId);

                if (hasDifferent)
                {
                    var evt = characterNetworkSyncComponent.CharacterSharedModel.EventId.Value;

                    var isPistolEvent = evt.Contains(WeaponConstants.Events[1]);

                    var meleeId = weaponSlotsComponent.SlotEntityIds[0];

                    ComponentManager.TryGetComponent<MeleeAttackComponent>(meleeId, out var melee);

                    PlaySoundEventComponent playSoundComponent = null;

                    if (melee != null)
                    {
                        playSoundComponent = new PlaySoundEventComponent(melee.AttackClip);
                    }

                    if (isPistolEvent)
                    {
                        ComponentManager.AddComponent(entityId, new ShootParticleEventComponent());

                        var pistolId = weaponSlotsComponent.SlotEntityIds[1];

                        if (ComponentManager.TryGetComponent<RangedWeaponComponent>(pistolId, out var ranged))
                        {
                            playSoundComponent = new PlaySoundEventComponent(ranged.ShootClip);
                        }
                    }

                    characterNetworkEventComponent.EventId = characterNetworkSyncComponent.CharacterSharedModel.EventId.Value;

                    if (characterNetworkSyncComponent.CharacterSharedModel.EventId.Value.Contains("damage"))
                    {
                        ComponentManager.AddComponent(entityId, new DamageAnimationEventComponent());
                    }
                    else
                    {
                        ComponentManager.AddComponent(entityId, new AttackAnimationEventComponent());
                        ComponentManager.AddComponent(entityId, playSoundComponent);
                    }
                }
            }
        }
    }
}
