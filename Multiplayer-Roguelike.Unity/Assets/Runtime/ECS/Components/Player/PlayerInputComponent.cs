using UnityEngine;

namespace Runtime.ECS.Components.Player
{
    public class PlayerInputComponent : IComponent
    {
        public PlayerControls PlayerControls { get; private set; }

        public PlayerInputComponent(PlayerControls playerControls)
        {
            PlayerControls = playerControls;
        }
    }
}