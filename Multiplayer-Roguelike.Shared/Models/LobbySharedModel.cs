using Shared.Properties;

namespace Shared.Models
{
    public class LobbySharedModel : SharedModel
    {
        public readonly Property<string> LobbyId = new Property<string>("lobby_id", string.Empty);
        public readonly Property<string> OwnerId = new Property<string>("owner_id", string.Empty);

        public readonly PropertyCollection<string> Members = new PropertyCollection<string>("members");

        public LobbySharedModel(string id) : base(id)
        {
            Children.Add(LobbyId.Id, LobbyId);
            Children.Add(OwnerId.Id, OwnerId);
            Children.Add(Members.Id, Members);
        }
    }
}
