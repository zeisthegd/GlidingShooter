using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using NaughtyAttributes;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
    {
        protected int[,] _map;
        protected string _seed;
        [Expandable] public MapData MapData;

        public event UnityAction LevelDataGenerated;

        [Button("Generate Level", EButtonEnableMode.Playmode)]
        public virtual void GenerateLevel()
        {
            _map = new int[MapData.Width, MapData.Height];
            GenerateIslandsArray();
            Debug.Log("Level Generated");
            LevelDataGenerated?.Invoke();
        }

        public virtual void GenerateIslandsArray()
        {
            _seed = MapData.Seed;
            if (MapData.UseRandomSeed)
                _seed = Randomizer.RandomString(10);
            System.Random rndNumber = new System.Random(_seed.GetHashCode());
            for (int x = 0; x < MapData.Width; x++)
            {
                for (int z = 0; z < MapData.Height; z++)
                {
                    if (x != MapData.Width / 2 && z != MapData.Height && _map[x, z] != 1)
                    {
                        _map[x, z] = (rndNumber.Next(0, 100) < 50) ? 1 : 0;
                    }
                    if (GetTotalIslandsCount() > MapData.MaxIslandCount)
                        return;
                }
            }
            if (GetTotalIslandsCount() < MapData.MinIslandCount)
                GenerateIslandsArray();
        }

        public virtual int GetNeighborWallsCount(int x, int z)
        {
            int neighborCount = 0;
            for (int neighborX = x - 1; neighborX <= x + 1; neighborX++)
            {
                for (int neighborZ = z - 1; neighborZ <= z + 1; neighborZ++)
                {
                    if (neighborX >= 0 && neighborZ >= 0 && neighborX < MapData.Width && neighborZ < MapData.Height)
                    {
                        if (neighborX != x || neighborZ != z)
                            neighborCount += _map[neighborX, neighborZ];
                    }
                    else
                        neighborCount++;
                }
            }
            return neighborCount;
        }

        public virtual int GetTotalIslandsCount()
        {
            int count = 0;
            for (int x = 0; x < MapData.Width; x++)
            {
                for (int z = 0; z < MapData.Height; z++)
                {
                    count += _map[x, z];
                }
            }
            return count;
        }


        // void OnDrawGizmos()
        // {
        //     if (_map != null)
        //     {
        //         for (int x = 0; x < MapData.Width; x++)
        //         {
        //             for (int z = 0; z < MapData.Height; z++)
        //             {
        //                 Gizmos.color = _map[x, y] == 1 ? Color.black : Color.white;
        //                 Vector3 pos = new Vector3(-MapData.Width / 2 + x + 0.5F, -MapData.Height / 2 + y + 0.5F);
        //                 Gizmos.DrawCube(pos, Vector3.one);
        //             }
        //         }
        //     }
        // }

        public int[,] Map { get => _map; }
    }
}

