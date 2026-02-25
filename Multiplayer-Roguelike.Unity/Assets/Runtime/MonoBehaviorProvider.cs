using UnityEngine;

namespace Runtime
{
    public class MonoBehaviorProvider : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        
        [field: SerializeField] public Transform Transform { get; private set; }
    }
}