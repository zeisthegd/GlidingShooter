using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Map Data")]
        public MapData MapData;

        [Header("Pooling Settings")]
        public ObjectPooler EnemyPoolPrefab;
        [ReadOnly] public List<ObjectPooler> ObjectPoolers;

        [HorizontalLine]
        [Header("Spawn Settings")]
        public float MinDistanceToPlayer;
        public float MaxDistanceToPlayer;
        public float TimeTillSpawnNewEnemies = 2;


        protected float _waitToSpawnTime = 0;
        protected bool _isSpawning = false;

        public virtual void LoadData()
        {
            CreateEnemyPools();
        }

        /// <summary>
        /// Create enemies pools.
        /// Connect the enemies' death events to the handler method.
        /// </summary>
        protected virtual void CreateEnemyPools()
        {
            ObjectPoolers = new List<ObjectPooler>();
            foreach (EnemySpawnSettings spawnSettings in MapData.SpawnSettings)
            {
                ObjectPooler enemyPooler = Instantiate(EnemyPoolPrefab);
                enemyPooler.ObjectToPool = spawnSettings.Prefab;
                enemyPooler.Init();
                ObjectPoolers.Add(enemyPooler);
                ConnectEnemyInPoolWithDeathEvent(enemyPooler);
            }
        }

        /// <summary>
        /// Spawn new enemies until the 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator SpawnRandomEnemies()
        {
            if (MapData.SpawnSettings.Length > 0)
            {
                _isSpawning = true;
                while (LevelManager.Instance.CurrentThreatLevel < LevelManager.Instance.MaxThreatLevel)
                {
                    Debug.Log("_isSpawning");
                    EnemySpawnSettings settings = MapData.GetRandomEnemySpawnSettings();
                    EnemyData randomEnemyData = settings.GetRandomEnemyData();
                    foreach (ObjectPooler pooler in ObjectPoolers)
                    {
                        if (pooler.ObjectToPool.gameObject == settings.Prefab.gameObject)
                        {
                            GameObject pooledObject = pooler.PullOneObject();
                            SpawnOneEnemy(pooledObject, randomEnemyData);
                            break;
                        }
                    }
                    yield return null;
                }
                Debug.Log("Max threat reached");
            }
            else
                Debug.LogWarning("Not enemy spawn settings inserted");
            _isSpawning = false;
        }

        /// <summary>
        /// Spawn a new enemy, load data if needed.
        /// Spawn near the player.
        /// </summary>
        public virtual void SpawnOneEnemy(GameObject pooledObject, EnemyData data)
        {
            Enemy enemy = pooledObject.GetComponent<Enemy>();
            enemy.AIBrain.Enabled = true;
            enemy.LoadEnemy(data);
            enemy.transform.position = GetPositionNearPlayer();
            LevelManager.Instance.CurrentThreatLevel += data.ThreatLevel;
            enemy.gameObject.SetActive(true);
            Debug.Log(data.name);
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void ConnectEnemyInPoolWithDeathEvent(ObjectPooler pooler)
        {
            foreach (GameObject pooledbject in pooler.ObjectPool.PooledObjects)
            {
                pooledbject.GetComponent<Enemy>().Health.OnDeath += HandleEnemyDeath;
            }
        }

        /// <summary>
        /// Get an empty position near the player.
        /// </summary>
        protected virtual Vector3 GetPositionNearPlayer()
        {
            Vector3 randomPos;
            randomPos.x = Randomizer.RandomNumber(MinDistanceToPlayer, MaxDistanceToPlayer);
            randomPos.y = Randomizer.RandomNumber(MinDistanceToPlayer, MaxDistanceToPlayer);
            randomPos.z = Randomizer.RandomNumber(MinDistanceToPlayer, MaxDistanceToPlayer);
            return randomPos;
        }


        /// <summary>
        /// When an enemy dies, delay a bit before spawning new ones.
        /// </summary>
        public virtual void HandleEnemyDeath(Character character)
        {
            Enemy enemy = character.GetComponent<Enemy>();
            LevelManager.Instance.CurrentThreatLevel -= enemy.Data.ThreatLevel;
            StartWaitToSpawnEnemyCounter();
        }

        public virtual void HandleMaxLevelThreatIncreased()
        {
            if (LevelManager.Instance.MaxThreatLevel - LevelManager.Instance.CurrentThreatLevel > 1
                    && _isSpawning == false && _waitToSpawnTime <= 0)
                StartWaitToSpawnEnemyCounter();
        }

        public virtual void StartWaitToSpawnEnemyCounter()
        {
            _waitToSpawnTime = TimeTillSpawnNewEnemies;
        }

        protected virtual void WaitToSpawnEnemy()
        {
            if (_waitToSpawnTime > 0)
                _waitToSpawnTime -= Time.deltaTime;
            if (_waitToSpawnTime < 0)
            {
                _waitToSpawnTime = 0;
                StartCoroutine(SpawnRandomEnemies());
            }
        }
    }
}

