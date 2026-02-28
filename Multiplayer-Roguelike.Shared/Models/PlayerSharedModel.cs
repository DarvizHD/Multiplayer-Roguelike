using Shared.Properties;

namespace Shared.Models
{
    public class PlayerSharedModel : SharedModel
    {
        public readonly Property<string> Nickname = new Property<string>("nickname", string.Empty);
        public readonly LobbySharedModel Lobby = new LobbySharedModel("lobby");
        public readonly CharacterSharedModel Character = new CharacterSharedModel("character");

        public PlayerSharedModel(string id) : base(id)
        {
            Properties.Add(Nickname.Id, Nickname);

            Models.Add(Lobby.Id, Lobby);
            Models.Add(Character.Id, Character);
        }
    }
}
