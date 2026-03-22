using UnityEngine;

namespace Runtime.Ecs.Components
{
    public class CursorWorldPositionComponent : IComponent
    {
        public Vector3 Position { get; set; }
    }
}
