using System;
using Runtime.Ecs.Components.Movement;
using Runtime.Ecs.Components.Network;
using Runtime.Ecs.Core;
using Runtime.Ecs.Systems.Core;
using Shared.Commands.Player;
using UnityEngine;

namespace Runtime.Ecs.Systems.Player.Network
{
    public class CharacterRotationSendSystem : BaseSystem
    {
        private const float _rotationThreshold = 1f;

        private QueryBuffer<CharacterConnectionComponent, CharacterNetworkSyncComponent, RotationComponent, LocalControllableTag> _buffer = new();

        public override void Update(float deltaTime)
        {
            ComponentManager.Filter.Query(ref _buffer);

            for (var i = 0; i < _buffer.Count; i++)
            {
                var characterConnectionComponent = _buffer.Components1[i];
                var characterNetworkSyncComponent = _buffer.Components2[i];
                var rotationComponent = _buffer.Components3[i];

                var angleDiff = Mathf.DeltaAngle(characterNetworkSyncComponent.CharacterSharedModel.Rotation.Value, rotationComponent.Angle);

                if (Math.Abs(angleDiff) <  _rotationThreshold)
                {
                    continue;
                }

                var rotateCommand = new RotateCommand
                (
                    characterNetworkSyncComponent.CharacterSharedModel.Id,
                    rotationComponent.Angle
                );

                rotateCommand.Write(characterConnectionComponent.ServerConnectionModel.PlayerPeer);
            }
        }
    }
}
