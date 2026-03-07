using Backend.ServerSystems;
using DotRecast.Core.Numerics;
using DotRecast.Detour;
using Shared.Primitives;
using Shared.Properties;

namespace Backend.Navigation
{
    public class NavigationSystem : IServerSystem
    {
        private const float _pathRecalcInternal = 0.7f;
        private float _pathTimer;

        public string Id => "navigation";
        private readonly WorldModel _worldModel;

        public NavigationSystem(WorldModel worldModel)
        {
            _worldModel = worldModel;
        }


        public void Update(float deltaTime)
        {
            _pathTimer += deltaTime;

            if (!(_pathTimer < _pathRecalcInternal))
            {
                _pathTimer = 0;

                foreach (var session in _worldModel.Sessions.Models.Values)
                {
                    foreach (var enemy in session.Enemies.Models.Values)
                    {
                        if (session.GameSessionSharedModel.Characters.TryGet(enemy.TargetPlayerId, out var player))
                        {
                            RecalculatePath(enemy.Agent, player.Position, session.NavMesh);
                        }
                    }
                }
            }
        }

        private void RecalculatePath(NavAgentModel agent, Property<Vector3> target, NavMeshModel sessionNavMesh)
        {
            var query = sessionNavMesh.Query;

            if (query != null)
            {
                var filter = new DtQueryDefaultFilter();

                var start = new RcVec3f(agent.Position.X, agent.Position.Y, agent.Position.Z);
                var end = new RcVec3f(target.Value.X, target.Value.Y, target.Value.Z);

                var halfExtents = new RcVec3f(2, 4, 2);

                query.FindNearestPoly(start, halfExtents, filter, out var startRef, out var startPt, out _);

                query.FindNearestPoly(end, halfExtents, filter, out var endRef, out var endPt, out _);

                if (startRef != 0 && endRef != 0)
                {
                    query.FindPath(startRef, endRef, startPt, endPt, filter, agent.PolyPath, out var pathCount, NavAgentModel.MaxPath);

                    if (pathCount > 0)
                    {
                        agent.PolyPathCount = pathCount;
                        BuildStraightPath(agent, startPt, endPt, sessionNavMesh);
                    }
                }
            }
        }

        private void BuildStraightPath(NavAgentModel agent, RcVec3f startPt, RcVec3f endPt, NavMeshModel sessionNavMesh)
        {
            var query = sessionNavMesh.Query;

            query.FindStraightPath(startPt, endPt, agent.PolyPath, agent.PolyPathCount, agent.StraightPath, out var pathCount, NavAgentModel.MaxPath, 0);

            if (pathCount > 0)
            {
                agent.Path.Clear();

                agent.StraightPathCount = pathCount;

                for (var i = 0; i < agent.StraightPathCount; i++)
                {
                    var point = agent.StraightPath[i].pos;

                    agent.Path.Add(new Vector3(point.X, point.Y, point.Z));
                }
            }
        }
    }
}
