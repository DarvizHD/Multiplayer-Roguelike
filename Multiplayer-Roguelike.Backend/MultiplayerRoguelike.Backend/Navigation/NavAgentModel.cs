using System.Collections.Generic;
using DotRecast.Detour;
using Shared.Primitives;

namespace Backend.Navigation
{
    public class NavAgentModel
    {
        public const int MaxPath = 256;

        public int Id { get; private set; }

        public Vector3 Position { get; private set; }
        public Vector3 Target { get; private set; }

        public float Speed { get; private set; } = 2.5f;

        public List<Vector3> Path { get; } = new();

        public long[] PolyPath { get; } = new long[MaxPath];
        public int PolyPathCount { get; set; }

        public DtStraightPath[] StraightPath { get; } = new DtStraightPath[MaxPath];
        public int StraightPathCount { get; set; }

        public bool HasPath => Path.Count > 0;

        public NavAgentModel(int id)
        {
            Id = id;
        }
    }
}
