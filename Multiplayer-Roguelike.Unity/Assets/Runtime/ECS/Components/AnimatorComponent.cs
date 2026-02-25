using UnityEngine;

namespace Runtime.ECS.Components
{
    public class AnimatorComponent  : IComponent
    {
        public int X => Animator.StringToHash("X");
        public int Z => Animator.StringToHash("Z");
        public int IsRun => Animator.StringToHash("IsRun");
        
        
        public readonly Animator Animator;

        public AnimatorComponent(Animator animator)
        {
            Animator = animator;
        }
    }
}