namespace Runtime.ECS.Components.Battle.Weapon
{
    public class AmmoComponent : IComponent
    {
        public int Current { get; set; }
        public int Max { get; }

        public AmmoComponent(int max)
        {
            Max = max;
            Current = max;
        }
    }
}
