using System.Collections.Generic;

namespace Backend.Navigation
{
    public class NavigationWorldModel
    {
        public Dictionary<int, NavAgentModel> Agents { get; } = new();
    }
}
