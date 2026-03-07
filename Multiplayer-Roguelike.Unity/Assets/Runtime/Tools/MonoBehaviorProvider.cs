using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Runtime.Tools
{
    public class MonoBehaviorProvider : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public Transform Transform { get; private set; }

        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; set; }
    }
}
