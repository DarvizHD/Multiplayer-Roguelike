using System.Collections.Generic;
using Runtime.Ecs.Components;

namespace Runtime.Ecs.Systems.UI
{
    public class UIComponent : IComponent
    {
        public readonly List<string> Elements = new();
    }
}
