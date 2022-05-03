using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Game;
using Penwyn.Tools;

using NaughtyAttributes;

namespace Penwyn.LevelEditor
{
    public class LevelEditor : SingletonMonoBehaviour<LevelEditor>
    {
        [Header("Grid")]
        public int Width = 10;
        public int Height = 10;
        public int TileScale = 1;


        public List<GameObject> PlaceableObjects;



        public GameObject CurrentlySelectedObject;
        protected Vector3 _currentMousePosition;
        protected Vector3 _mouseIndFromPos;

        void Start()
        {
        }

        void Update()
        {
            _currentMousePosition = CursorManager.Instance.GetRayHitUnderMouse().point;
            UpdateCurrentObjectPosition();

        }

        public virtual void UpdateCurrentObjectPosition()
        {
            if (CurrentlySelectedObject != null)
            {
                CurrentlySelectedObject.transform.position = IndexToGridPosition((int)_mouseIndFromPos.x, (int)_mouseIndFromPos.z);
            }
        }

        public virtual void SetCurrentSelectedObject(GameObject gameObject)
        {
            if (CurrentlySelectedObject != null)
                Destroy(CurrentlySelectedObject);
            CurrentlySelectedObject = Instantiate(gameObject);
        }

        #region Data Serialization
        public virtual void SaveData(string path)
        {

        }

        public virtual void LoadData(string path)
        {

        }


        #endregion


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
    }
}
