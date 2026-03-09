namespace Runtime.ECS.Components.Battle.Weapon
{
    public class SwitchWeaponEventComponent : IComponent
    {
        public int TargetSlot { get; }

        public SwitchWeaponEventComponent(int targetSlot)
        {
            TargetSlot = targetSlot;
        }
    }
}
