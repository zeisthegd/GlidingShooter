using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using NaughtyAttributes;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class LevelBuilder : SingletonMonoBehaviour<LevelBuilder>
    {
        [Header("Spawn Settings")]
        public float DistanceBetweenIsland = 40;
        public float MinIslandDistanceOffset = 40;
        public float MaxIslandDistanceOffset = 120;
        public float MinIslandHeightOffset = 120;
        public float MaxIslandHeightOffset = 120;
        public Transform IslandsContainer;
        [ReadOnly] public List<GameObject> Islands = new List<GameObject>();
        protected LevelGenerator _generator;
        

        void Awake()
        {
            Islands = new List<GameObject>();
        }

        public virtual void BuildMap()
        {
            _generator = LevelGenerator.Instance;
            IslandsContainer = GameObject.FindGameObjectWithTag("IslandsContainer").transform;
            DestroyAllIslands();
            SpawnIslands();
        }

        public virtual void SpawnIslands()
        {
            int[,] islands = _generator.Map;
            for (int x = 0; x < _generator.MapData.Width; x++)
            {
                for (int z = 0; z < _generator.MapData.Height; z++)
                {
                    if (islands[x, z] == 1)
                        Islands.Add(Instantiate(_generator.MapData.IslandPrefab, GetRandomIslandPosition(x, z), Quaternion.identity, IslandsContainer));
                }
            }
        }

        public virtual void DestroyAllIslands()
        {
            for (int x = 0; x < Islands.Count; x++)
            {
                GameObject island = Islands[x];
                Destroy(island);
            }
            Islands.Clear();
        }

        public virtual Vector3 GetRandomIslandPosition(int x, int z)
        {
            return new Vector3((DistanceBetweenIsland + Randomizer.RandomNumber(MinIslandDistanceOffset, MaxIslandDistanceOffset)) * x,
                                    Randomizer.RandomNumber(MinIslandHeightOffset, MaxIslandHeightOffset),
                                        (DistanceBetweenIsland + Randomizer.RandomNumber(MinIslandDistanceOffset, MaxIslandDistanceOffset)) * z);
        }

        void OnEnable()
        {
            LevelGenerator.Instance.LevelDataGenerated += BuildMap;
        }

        void OnDisable()
        {
            LevelGenerator.Instance.LevelDataGenerated -= BuildMap;
        }
    }
}

