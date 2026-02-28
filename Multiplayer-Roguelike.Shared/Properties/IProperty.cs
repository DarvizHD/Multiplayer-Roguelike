using Shared.Protocol;

namespace Shared.Properties
{
    public interface IProperty
    {
        string Id { get; }
        bool IsDirty { get; }
        void Read(NetworkProtocol protocol);
        void Write(NetworkProtocol protocol);
        void UnsetDirty();
    }
}
