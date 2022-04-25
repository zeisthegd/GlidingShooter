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
        [Header("Settings")]
        public bool ShouldCreateLevel = false;
        protected MapData _mapData;

        protected virtual void Start()
        {
            LoadLevel();
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
            if (ShouldCreateLevel)
            {

            }
        }


        /// <summary>
        /// Generate the level and spawn the enemies.
        /// </summary>
        protected virtual void LoadLevel()
        {
            if (ShouldCreateLevel)
            {
                ChangeToRandomData();
                LevelGenerator.GenerateLevel();
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

        }

        public virtual void MovePlayerTo(Vector2 position)
        {
            Characters.Player.transform.position = position;
        }
    }
}
