using UnityEngine;

namespace Runtime.ECS.Components
{
    public class CursorWorldPositionComponent : IComponent
    {
        public Vector3 Position { get; set; }
    }
}
