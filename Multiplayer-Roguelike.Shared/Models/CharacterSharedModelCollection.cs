using Shared.Models.Common;

namespace Shared.Models
{
    public class CharacterSharedModelCollection : SharedModelCollection<CharacterSharedModel>
    {
        public CharacterSharedModelCollection(string id) : base(id)
        {
        }

        protected override CharacterSharedModel CreateInstance(string id)
        {
            return new CharacterSharedModel(id);
        }
    }
}
