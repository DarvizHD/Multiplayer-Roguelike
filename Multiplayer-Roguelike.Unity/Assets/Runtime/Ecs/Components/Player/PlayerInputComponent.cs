namespace Runtime.Ecs.Components.Player
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
