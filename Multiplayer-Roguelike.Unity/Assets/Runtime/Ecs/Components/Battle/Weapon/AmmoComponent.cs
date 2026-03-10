namespace Runtime.ECS.Components.Battle.Weapon
{
    public class AmmoComponent : IComponent
    {
        public int Current { get; set; }
        public int Magazine { get; }
        public int Reserve { get; set; }

        public AmmoComponent(int magazine, int reserve)
        {
            Magazine = magazine;
            Current = magazine;
            Reserve = reserve;
        }
    }
}
