using Shared.Properties;

namespace Shared.Models
{
    public class GameSessionSharedModel : SharedModel
    {
        public readonly SharedModelCollection<CharacterSharedModel> Characters =
            new SharedModelCollection<CharacterSharedModel>("characters", CharacterSharedModel.Create);

        public readonly SharedModelCollection<NpcSharedModel> NPCs = new SharedModelCollection<NpcSharedModel>("npcs", NpcSharedModel.Create);

        public Property<bool> IsRun = new Property<bool>("is_run", false);

        public GameSessionSharedModel(string id) : base(id)
        {
            Children.Add(Characters.Id, Characters);
            Children.Add(NPCs.Id, NPCs);
            Children.Add(IsRun.Id, IsRun);
        }
    }
}
