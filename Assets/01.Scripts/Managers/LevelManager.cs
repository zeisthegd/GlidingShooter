using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Penwyn.Tools;
using NaughtyAttributes;

using Penwyn.LevelEditor;

namespace Penwyn.Game
{
    public class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        public List<LevelBlock> PlaceableObjects;
        public List<LevelBlock> PlacedBlocks;
        public LevelDataList LevelDataList;

        public int CurrentLevelIndex = 0;

        public virtual void LoadNextLevel()
        {
            LoadLevelByIndex(CurrentLevelIndex + 1);
        }

        public virtual void LoadLevelByIndex(int index)
        {
            LoadLevel(LevelDataList.List[index]);
            CurrentLevelIndex = index;
        }

        /// <summary>
        /// Generate the level and spawn the enemies.
        /// </summary>
        protected virtual void LoadLevel(LevelData data)
        {
            foreach (TileData tile in data.BlockMap)
            {
                LevelBlock block = GetBlockByID(tile.BlockID);
                LevelBlock blockObj = Instantiate(block);
                blockObj.transform.position = tile.Position;
                blockObj.transform.eulerAngles = new Vector3(0, tile.RotationAngleY, 0);
                PlacedBlocks.Add(blockObj);
            }
        }

        /// <summary>
        /// Get a block by its ID.
        /// </summary>
        public virtual LevelBlock GetBlockByID(string blockID)
        {
            for (int i = 0; i < PlaceableObjects.Count; i++)
            {
                if (PlaceableObjects[i].BlockID == blockID)
                    return PlaceableObjects[i];
            }
            return null;
        }


        /// <summary>
        /// Destroy all placed blocks
        /// </summary>
        [Button("Clean All Blocks", EButtonEnableMode.Playmode)]
        public virtual void CleanAllBlocks()
        {
            for (int i = 0; i < PlacedBlocks.Count; i++)
            {
                Destroy(PlacedBlocks[i].gameObject);
            }
            PlacedBlocks.Clear();
        }
    }
}
