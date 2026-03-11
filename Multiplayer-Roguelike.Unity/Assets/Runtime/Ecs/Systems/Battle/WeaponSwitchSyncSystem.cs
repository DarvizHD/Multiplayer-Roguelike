using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchSyncSystem : BaseSystem
    {
        private QueryBuffer<CharacterNetworkSyncComponent, CharacterConnectionComponent,
                SwitchWeaponEventComponent, LocalControllableTag>
            _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var entityId = _buffer.EntityIds[i];
                var characterNetwork = _buffer.Components1[i];
                var characterConnection = _buffer.Components2[i];
                var switchWeaponEvent = _buffer.Components3[i];

                var switchWeaponCommand = new SwitchWeaponCommand(characterNetwork.CharacterSharedModel.Id,
                    (ushort)switchWeaponEvent.TargetSlot);

                switchWeaponCommand.Write(characterConnection.ServerConnectionModel.PlayerPeer);
            }
        }
    }
}
