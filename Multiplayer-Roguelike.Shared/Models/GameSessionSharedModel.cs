using Shared.Models.Common;
using Shared.Properties;

namespace Shared.Models
{
    public class GameSessionSharedModel : SharedModel
    {
        public readonly CharacterSharedModelCollection Characters = new CharacterSharedModelCollection("characters");

        public readonly EnemySharedModelCollection Enemies = new EnemySharedModelCollection("enemies");

        public readonly Property<bool> IsRun = new Property<bool>("is_run", false);

        public GameSessionSharedModel(string id) : base(id)
        {
            Children.Add(Characters.Id, Characters);
            Children.Add(Enemies.Id, Enemies);
            Children.Add(IsRun.Id, IsRun);
        }
    }
}
