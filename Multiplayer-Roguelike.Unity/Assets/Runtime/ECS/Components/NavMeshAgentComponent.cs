using UnityEngine;
using UnityEngine.AI;

namespace Runtime.ECS.Components
{
    public class NavMeshAgentComponent : IComponent
    {
        public readonly NavMeshAgent Agent;

        public NavMeshAgentComponent(NavMeshAgent agent, Vector3 startPosition,float speed)
        {
            Agent = agent;
            Agent.transform.position = startPosition;
            Agent.speed = speed;
        }
    }
}
