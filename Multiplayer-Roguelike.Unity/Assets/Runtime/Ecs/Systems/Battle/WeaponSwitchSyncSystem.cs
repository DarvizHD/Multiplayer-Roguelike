using Runtime.Ecs.Components.Battle.Weapon;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Commands.Player;

namespace Runtime.Ecs.Systems.Battle
{
    public class WeaponSwitchSyncSystem : BaseSystem
    {
        protected override IQueryBuffer Buffer => _buffer;

        private QueryBuffer<CharacterNetworkSyncComponent, CharacterConnectionComponent,
                SwitchWeaponEventComponent, LocalControllableTag>
            _buffer = new();


        protected override void Query()
        {
            ComponentManager.Filter.Query(ref _buffer);
        }

        protected override void Update(int i, float deltaTime)
        {
            var characterNetwork = _buffer.Components1[i];
            var characterConnection = _buffer.Components2[i];
            var switchWeaponEvent = _buffer.Components3[i];

            var switchWeaponCommand = new SwitchWeaponCommand(characterNetwork.CharacterSharedModel.Id,
                (ushort)switchWeaponEvent.TargetSlot);

            switchWeaponCommand.Write(characterConnection.ServerConnectionModel.PlayerPeer);
        }
    }
}
