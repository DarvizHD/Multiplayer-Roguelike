using System.Collections.Generic;
using Shared.Models;

namespace Runtime.TestServer
{
    public class TestWorldModel
    {
        public Dictionary<string, TestCharacterModel> Characters { get; private set; } = new();

        public WorldSharedModel World { get; private set; } = new("world");
    }
}
