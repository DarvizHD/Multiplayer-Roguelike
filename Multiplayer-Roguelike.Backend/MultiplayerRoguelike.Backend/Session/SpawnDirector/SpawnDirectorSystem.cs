using System;
using Backend.Enemies;
using Backend.ServerSystems;
using Shared.Commands.Player;
using Shared.Models.GameSession;

namespace Backend.Session.SpawnDirector
{
    public class SpawnDirectorSystem : IServerSystem
    {
        private float _spawnTimer;
        private readonly SpawnDirectorModel _model;
        private readonly SessionModel _sessionModel;
        private int _lastSpawnPointIndex;
        private int _lastEnemyId = 10;
        public string Id { get; }

        public SpawnDirectorSystem(string id, SpawnDirectorModel model, SessionModel sessionModel)
        {
            Id = id;
            _model = model;
            _sessionModel = sessionModel;
        }

        public void Update(float deltaTime)
        {
            if (_sessionModel.GameSessionWaveModel.WaveActive)
            {
                _spawnTimer += deltaTime;
                if (_spawnTimer >= _model.SpawnInterval)
                {
                    var maxToSpawn = _sessionModel.GameSessionWaveModel.EnemiesTarget - _sessionModel.GameSessionWaveModel.EnemiesSpawned;
                    if (maxToSpawn > 0)
                    {
                        var spawnPointCount = _model.SpawnPoints.Count;
                        for (var i = 0; i < spawnPointCount && maxToSpawn > 0; i++)
                        {
                            var index = (_lastSpawnPointIndex + i) % spawnPointCount;
                            var spawnPoint = _model.SpawnPoints[index];
                            if (CanSpawnEnemy(spawnPoint, _spawnTimer))
                            {
                                SpawnEnemy(spawnPoint);
                                maxToSpawn--;
                                _sessionModel.GameSessionWaveModel.EnemiesSpawned++;
                                _lastSpawnPointIndex = (index + 1) % spawnPointCount;
                            }
                        }
                    }
                    _spawnTimer = 0;
                }
            }
        }

        private void SpawnEnemy(SpawnPointModel spawnPoint)
        {
            var enemy = new EnemyModel(_lastEnemyId + 1, new EnemyConfig());
            enemy.Shared.Position.Value = spawnPoint.Position;
            _sessionModel.GameSessionSharedModel.Enemies.Add(enemy.Shared);
            _sessionModel.Enemies.Add(_lastEnemyId + 1, enemy);
            _lastEnemyId++;
        }

        private bool CanSpawnEnemy(SpawnPointModel spawnPoint, float deltaTime)
        {
            spawnPoint.Timer += deltaTime;
            if (spawnPoint.Timer >= spawnPoint.Interval)
            {
                spawnPoint.Timer = 0;
                return true;
            }
            return false;
        }
    }
}
