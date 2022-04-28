using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Penwyn.Tools;
using NaughtyAttributes;


namespace Penwyn.Game
{
    public class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        [Header("Map Datas")]
        public List<MapData> MapDatas;

        [Header("Sub-components")]
        public LevelGenerator LevelGenerator;
        public LootDropManager LootDropManager;
        public EnemySpawner EnemySpawner;
        [Header("Settings")]
        public bool ShouldCreateLevel = false;
        public float CurrentThreatLevel = 0;
        public float MaxThreatLevel = 0;


        protected MapData _mapData;
        protected bool _initialized = false;


        protected virtual void Start()
        {
            InputReader.Instance.EnableGameplayInput();
        }

        protected virtual void Update()
        {
            IncreaseThreatLevelAndProgress();
        }

        /// <summary>
        /// Increase the max threat level of enemies.
        /// Increase the level progress.
        /// </summary>
        public virtual void IncreaseThreatLevelAndProgress()
        {
            if (ShouldCreateLevel && _initialized)
            {
                MaxThreatLevel += _mapData.ThreatLevelIncrementPerSecond * Time.deltaTime;
            }
        }


        /// <summary>
        /// Generate the level and spawn the enemies.
        /// </summary>
        public virtual void LoadLevel()
        {
            if (ShouldCreateLevel)
            {
                ChangeToRandomData();
                LevelGenerator.GenerateLevel();
                StartCoroutine(EnemySpawner.SpawnRandomEnemies());
                _initialized = true;
            }
        }

        /// <summary>
        /// Change the level's data to a random one of the list.
        /// </summary>
        public virtual void ChangeToRandomData()
        {
            MapData randomData = MapDatas[Randomizer.RandomNumber(0, MapDatas.Count)];
            _mapData = Instantiate(randomData);

            LevelGenerator.MapData = _mapData;
            LootDropManager.MapData = _mapData;
            EnemySpawner.MapData = _mapData;

            MaxThreatLevel = _mapData.StartingThreatLevel;

            EnemySpawner.LoadData();
        }

        public virtual void MovePlayerTo(Vector2 position)
        {
            Characters.Player.transform.position = position;
        }
    }
}
