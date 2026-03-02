using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using Shared.Commands;
using UnityEngine;

namespace Runtime.ECS.Systems
{
    public class PlayerInputMovementSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, playerInputComponent, characterConnectionComponent, characterSyncComponent, positionComponent)
                     in ComponentManager.Query<PlayerInputComponent, CharacterConnectionComponent, CharacterNetworkSyncComponent, PositionComponent>())
            {
                var moveInput = playerInputComponent.PlayerControls.Gameplay.Move.ReadValue<Vector2>();

                var moveCommand = new MoveCommand(characterSyncComponent.CharacterSharedModel.Id, moveInput.ToSharedVector2(), positionComponent.Position.ToSharedVector3());

                moveCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);
            }
        }
    }
}
