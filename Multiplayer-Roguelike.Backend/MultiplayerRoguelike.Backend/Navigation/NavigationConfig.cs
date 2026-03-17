using DotRecast.Detour.Crowd;

namespace Backend.Navigation
{
    public class NavigationConfig
    {
        public DtCrowdAgentParams AgentParams { get; } = new()
        {
            radius = 0.6f,
            height = 2.0f,
            maxAcceleration = 8.0f,
            maxSpeed = 2.0f,
            collisionQueryRange = 1.5f,
            pathOptimizationRange = 1.0f,
            separationWeight = 1.5f,
            updateFlags = 0,
            obstacleAvoidanceType = 2,
            queryFilterType = 0,
            userData = null
        };

        public DtObstacleAvoidanceParams ObstacleParams { get; } = new()
        {
            velBias = 0.4f,
            weightDesVel = 2.0f,
            weightCurVel = 0.75f,
            weightSide = 0.75f,
            weightToi = 2.5f,
            horizTime = 2.5f,
            gridSize = 33,
            adaptiveDivs = 7,
            adaptiveRings = 2,
            adaptiveDepth = 5
        };

        public DtCrowdConfig CrowdConfig { get; } = new(0.6f)
        {
            pathQueueSize = 64,
            maxFindPathIterations = 50,
            maxTargetFindPathIterations = 10,
            topologyOptimizationTimeThreshold = 0.25f,
            checkLookAhead = 6,
            targetReplanDelay = 0.3f,
            maxTopologyOptimizationIterations = 16,
            collisionResolveFactor = 0.6f,
            maxObstacleAvoidanceCircles = 4,
            maxObstacleAvoidanceSegments = 6
        };
    }
}
