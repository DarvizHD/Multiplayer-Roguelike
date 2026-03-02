using Backend.CommandExecutors;
using Runtime.ECS.Components.Movement;
using Runtime.ECS.Components.Player;
using Runtime.ECS.Core;
using UnityEngine;

namespace Runtime.ECS.Systems.Rotation
{
    public class PlayerLookRotationSystem : BaseSystem
    {
        public override void Update(float deltaTime)
        {
            foreach (var (entityId, playerInputComponent, positionComponent, characterConnectionComponent, characterNetworkSyncComponent)
                     in ComponentManager.Query<PlayerInputComponent, PositionComponent, CharacterConnectionComponent, CharacterNetworkSyncComponent>())
            {

                var mouseScreenPosition = playerInputComponent.PlayerControls.Gameplay.Look.ReadValue<Vector2>();
                var mouseWorldPosition = Camera.main.ScreenPointToRay(mouseScreenPosition);

                if (Physics.Raycast(mouseWorldPosition, out var hit))
                {
                    var lookPoint = hit.point;

                    var direction = lookPoint - positionComponent.Position;
                    direction.y = 0f;

                    if (direction.sqrMagnitude > 0.01f)
                    {
                        var targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;

                        var rotateCommand = new RotateCommand(characterNetworkSyncComponent.CharacterSharedModel.Id, targetAngle);

                        rotateCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);
                    }
                }
            }
        }
    }
}
