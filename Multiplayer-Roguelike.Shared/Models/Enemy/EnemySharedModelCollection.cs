using Shared.Models.Common;

namespace Shared.Models.Enemy
{
    public class EnemySharedModelCollection : SharedModelCollection<EnemySharedModel>
    {
        public EnemySharedModelCollection(string id) : base(id)
        {
        }

        protected override EnemySharedModel CreateInstance(string id)
        {
            return new EnemySharedModel(id);
        }
    }
}
