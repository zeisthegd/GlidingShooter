using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEditor;
using Penwyn.Game;

namespace Penwyn.LevelEditor
{
    public class LevelEditorUI : MonoBehaviour
    {
        public Button TileButtonPrefab;
        public Transform TileButtonsContainer;


        void Start()
        {
            LoadPlaceableObjects(LevelEditor.Instance.PlaceableObjects);
        }

        public void LoadPlaceableObjects(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObj in gameObjects)
            {
                Button newBtn = Instantiate(TileButtonPrefab, TileButtonsContainer.position, Quaternion.identity, TileButtonsContainer);
                newBtn.GetComponent<RawImage>().texture = AssetPreview.GetAssetPreview(gameObj);
                newBtn.onClick.AddListener(() =>
                {
                    LevelEditor.Instance.SetCurrentSelectedObject(gameObj);
                });
            }
        }
    }
}

