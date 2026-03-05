using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Tools
{
    public class MonoBehaviorProvider : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public Transform Transform { get; private set; }

        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    }
}
