using Shared.Models.Common;
using Shared.Models.Enemy;
using Shared.Models.Player;
using Shared.Properties;

namespace Shared.Models.GameSession
{
    public class GameSessionSharedModel : SharedModel
    {
        public readonly CharacterSharedModelCollection Characters = new CharacterSharedModelCollection("characters");

        public readonly EnemySharedModelCollection Enemies = new EnemySharedModelCollection("enemies");

        public readonly Property<bool> IsRun = new Property<bool>("is_run", false);

        public readonly Property<bool> IsComplete = new Property<bool>("is_complete", false);

        public readonly Property<int> WaveNumber = new Property<int>("wave_number", 1);

        public readonly Property<string> SessionTime = new Property<string>("session_time", "00:00");

        public GameSessionSharedModel(string id) : base(id)
        {
            Children.Add(Characters.Id, Characters);
            Children.Add(Enemies.Id, Enemies);
            Children.Add(IsRun.Id, IsRun);
            Children.Add(IsComplete.Id, IsComplete);
            Children.Add(WaveNumber.Id, WaveNumber);
            Children.Add(SessionTime.Id, SessionTime);
        }
    }
}
