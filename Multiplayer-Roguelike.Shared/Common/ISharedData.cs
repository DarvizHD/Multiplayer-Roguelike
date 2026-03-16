using Shared.Protocol;

namespace Shared.Common
{
    public interface ISharedData
    {
        string Id { get; }
        bool IsDirty { get; }
        void Read(NetworkProtocol protocol);
        void ReadData(NetworkProtocol protocol);
        void Write(NetworkProtocol protocol);
        void WriteAll(NetworkProtocol protocol);
        void ClearDirty();
    }
}
