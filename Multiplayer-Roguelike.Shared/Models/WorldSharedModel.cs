using Shared.Properties;

namespace Shared.Models
{
    public class WorldSharedModel : SharedModel
    {
        public readonly SharedModelCollection<CharacterSharedModel> Characters =
            new SharedModelCollection<CharacterSharedModel>("characters", CharacterSharedModel.Create );

        public Property<bool> IsRun = new Property<bool>("is_run", false);

        public WorldSharedModel(string id) : base(id)
        {
            Children.Add(Characters.Id, Characters);
            Children.Add(IsRun.Id, IsRun);
        }
    }
}
