namespace Shared.Models
{
    public class WorldSharedModel : SharedModel
    {
        public readonly SharedModelCollection<CharacterSharedModel> Characters =
            new SharedModelCollection<CharacterSharedModel>("characters", CharacterSharedModel.Create );

        public WorldSharedModel(string id) : base(id)
        {
            Children.Add(Characters.Id, Characters);
        }
    }
}
