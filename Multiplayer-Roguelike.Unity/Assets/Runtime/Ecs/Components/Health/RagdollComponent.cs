using Runtime.Tools;

namespace Runtime.ECS.Components.Health
{
    public class RagdollComponent : IComponent
    {
        public RagdollProvider RagdollProvider;

        public RagdollComponent(RagdollProvider ragdollProvider)
        {
            RagdollProvider = ragdollProvider;
        }
    }
}
