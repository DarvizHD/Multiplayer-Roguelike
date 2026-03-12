using Shared.Models.Common;
using Shared.Primitives;
using Shared.Properties;

namespace Shared.Models.Enemy
{
    public class EnemySharedModel : SharedModel
    {
        public readonly Property<float> Health = new Property<float>("health", 100f);
        public readonly Property<Vector3> Position = new Property<Vector3>("last_position", new Vector3(0f, 0f, 0f));
        public readonly Property<string> TargetPlayerId = new Property<string>("target_id", null);
        public readonly Property<string> AnimationState = new Property<string>("animation_state", null);

        public EnemySharedModel(string id) : base(id)
        {
            Children.Add(Health.Id, Health);
            Children.Add(Position.Id, Position);
            Children.Add(TargetPlayerId.Id, TargetPlayerId);
            Children.Add(AnimationState.Id, AnimationState);
        }

        public static EnemySharedModel Create(string id)
        {
            return new EnemySharedModel(id);
        }
    }
}
