using System.Collections.Generic;

namespace Backend.ModelCollections
{
    public interface ISerializable
    {
        Dictionary<string, object> Serialize();
    }
}
