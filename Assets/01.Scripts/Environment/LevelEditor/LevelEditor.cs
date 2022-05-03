using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Penwyn.Game;
using Penwyn.Tools;

using NaughtyAttributes;

namespace Penwyn.LevelEditor
{
    public class LevelEditor : SingletonMonoBehaviour<LevelEditor>
    {
#if UNITY_EDITOR

        [Header("Grid")]
        public int Width = 10;
        public int Height = 10;
        public int TileScale = 1;


        [Header("Blocks List")]
        public List<LevelBlock> PlaceableObjects;
        public List<LevelBlock> PlacedBlocks;
        public LevelBlock CurrentlySelectedBlock;


        protected Vector3 _currentMousePosition;
        protected Vector3 _mouseIndFromPos;
        protected LevelData _data;

        void Start()
        {
        }

        void Update()
        {
            _currentMousePosition = CursorManager.Instance.GetRayHitUnderMouse().point;
            UpdateCurrentObjectPosition();

        }

        /// <summary>
        /// Move the currently selected block using mouse input.
        /// </summary>
        public virtual void UpdateCurrentObjectPosition()
        {
            if (CurrentlySelectedBlock != null)
            {
                CurrentlySelectedBlock.transform.position = IndexToGridPosition((int)_mouseIndFromPos.x, (int)_mouseIndFromPos.z);
            }
        }

        /// <summary>
        /// From the UI, choose a block to place.
        /// </summary>
        public virtual void ChooseBlockToPlace(LevelBlock block)
        {
            if (CurrentlySelectedBlock != null)
                Destroy(CurrentlySelectedBlock);
            CurrentlySelectedBlock = Instantiate(block);
            InputReader.Instance.EnableGameplayInput();
        }

        /// <summary>
        /// Place the current block on the grid, add to the placed blocks list.
        /// </summary>
        public virtual void PlaceCurentBlock()
        {
            if (CurrentlySelectedBlock != null)
            {
                PlacedBlocks.Add(CurrentlySelectedBlock);
                Debug.Log($"Placed {CurrentlySelectedBlock.BlockID}");
                CurrentlySelectedBlock = null;
                InputReader.Instance.DisableGameplayInput();
            }
        }
        //*---------------------------------------------------------------------------------------------------------------------------------------------

        #region Data Serialization

        /// <summary>
        /// Get the layout and save it.
        /// </summary>
        public virtual void SaveData(string path)
        {
            _data = ScriptableObject.CreateInstance<LevelData>();
            GetLevelLayout();
            LevelDataManager.Instance.LevelData.BlockMap = _data.BlockMap;
        }

        /// <summary>
        /// Clone the data and load the layout.
        /// </summary>
        public virtual void LoadData(string path)
        {
            _data = Instantiate(LevelDataManager.Instance.LevelData);
            CleanAllBlocks();
            LoadLevelLayout();
        }

        /// <summary>
        /// Get the layout of currently placed blocks.
        /// </summary>
        public virtual void GetLevelLayout()
        {
            _data.BlockMap = new List<TileData>();
            foreach (LevelBlock block in PlacedBlocks)
            {
                _data.BlockMap.Add(new TileData(block.BlockID, block.transform.position, block.transform.eulerAngles.y));
            }
        }

        /// <summary>
        /// Load the layout of the blocks from the save data.
        /// Instantiate the blocks using the data and add the blocks to the placed blocks list.
        /// </summary>
        public virtual void LoadLevelLayout()
        {
            foreach (TileData data in _data.BlockMap)
            {
                LevelBlock block = GetBlockByID(data.BlockID);
                LevelBlock blockObj = Instantiate(block);
                blockObj.transform.position = data.Position;
                blockObj.transform.eulerAngles = new Vector3(0, data.RotationAngleY, 0);
                PlacedBlocks.Add(blockObj);
            }
        }

        #endregion

        //*---------------------------------------------------------------------------------------------------------------------------------------------

        #region Block Operations

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

        #endregion

        //*---------------------------------------------------------------------------------------------------------------------------------------------

        #region Position Helper
        /// <summary>
        /// Turns an array index to world position.
        /// </summary>
        public virtual Vector3 IndexToGridPosition(int x, int z)
        {
            Vector3 position = Vector3.zero;
            position.x = (x - (Width / 2.0F - 0.5F)) * TileScale;
            position.y = TileScale / 2.0F;
            position.z = (z - (Height / 2.0F - 0.5F)) * TileScale;
            return position;
        }

        /// <summary>
        /// Turns a world position into an array index.
        /// </summary>
        public virtual Vector3 PositionToIndex(float x, float z)
        {
            Vector3 position = Vector3.zero;
            position.x = Mathf.Round((x / TileScale) + (Width / 2.0F - 0.5F));
            position.z = Mathf.Round((z / TileScale) + (Height / 2.0F - 0.5F));
            return position;
        }
        #endregion

        void OnDrawGizmos()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Height; z++)
                {
                    _mouseIndFromPos = PositionToIndex(_currentMousePosition.x, _currentMousePosition.z);
                    Gizmos.color = Color.white;
                    if (_mouseIndFromPos.x == x && _mouseIndFromPos.z == z)
                        Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(IndexToGridPosition(x, z), Vector3.one * TileScale);
                }
            }
        }

        void OnEnable()
        {
            if (InputReader.Instance != null)
            {
                InputReader.Instance.NormalAttackPressed += PlaceCurentBlock;
            }
            else
            {
                Debug.LogWarning("InputReader missing.");
            }
        }

        void OnDisable()
        {
            if (InputReader.Instance != null)
            {
                InputReader.Instance.NormalAttackPressed -= PlaceCurentBlock;
            }
            else
            {
                Debug.LogWarning("InputReader missing.");
            }
        }
#endif
    }
}
