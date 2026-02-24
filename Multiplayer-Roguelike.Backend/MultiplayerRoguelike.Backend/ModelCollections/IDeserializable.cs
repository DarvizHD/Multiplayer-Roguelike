using System.Collections.Generic;

namespace Backend.ModelCollections
{
    public interface IDeserializable
    {
        void Deserialize(Dictionary<string, object> data);
    }
}