using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

using TMPro;

namespace Penwyn.LevelEditor
{
    public class LevelDataManager : MonoBehaviour
    {
        [Header("--- Save and Load ---")]
        public TMP_InputField FileLoadName;
        public TMP_InputField FileSaveName;


        [Header("--- Scriptable Object Prefix ---")]//* These are the strings that indicate the type of the resouce. Used to manage files.
        public string LevelDataSfx;

        [Header("--- Game Settings ---")]
        public LevelDataList LevelDataList;
        protected LevelData _levelData;

        public const string SaveFolderPath = "Assets/Resources/LevelDatas/";


        /// <summary>
        /// Save Button. Can be used if the input text is not null or empty.
        /// First, get the save file path.
        /// Then for each component of the GameEditor, call the SaveData function on that component.
        /// If the save directory doesn't exists, create a new directory.
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrEmpty(FileSaveName.text))
            {
                Debug.Log("Null input");
                return;
            }

            string levelName = FileSaveName.text;
            string folderPath = GetFolderPath(FileSaveName.text);

            if (!FolderExists(folderPath))
                AssetDatabase.CreateFolder(SaveFolderPath.TrimEnd('/'), FileSaveName.text);

            _levelData = ScriptableObject.CreateInstance<LevelData>();

            _levelData.LevelName = FileSaveName.text;
            LevelEditor.Instance.SaveData(GetFilePath(folderPath, levelName, LevelDataSfx));

            LevelDataList.Add(_levelData);

            AssetDatabase.CreateAsset(_levelData, GetFilePath(folderPath, levelName, LevelDataSfx));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Saved");
        }

        /// <summary>
        /// Load Button. Can be used if the input text is not null or empty.
        /// First, get the load file path.
        /// Then for each component of the GameEditor, call the LoadData function on that component.
        /// </summary>
        public void Load()
        {
            if (string.IsNullOrEmpty(FileLoadName.text))
                return;
            FileSaveName.text = FileLoadName.text;
            string levelName = FileLoadName.text;
            string folderPath = GetFolderPath(FileLoadName.text);

            if (FolderExists(folderPath))
            {
                _levelData = (LevelData)AssetDatabase.LoadAssetAtPath(GetSettingsPath(folderPath, levelName), typeof(LevelData));
                LevelEditor.Instance.LoadData(GetFilePath(folderPath, levelName, LevelDataSfx));
            }
        }

        public string GetSettingsPath(string folderPath, string levelName)
        {
            return GetFilePath(folderPath, levelName, LevelDataSfx);
        }

        /// <summary>
        /// Get FolderPath.
        /// </summary>
        /// <param name="levelName">Level name</param>
        private string GetFolderPath(string levelName)
        {
            return $"{SaveFolderPath}{levelName}";
        }

        /// <summary>
        /// Get FilePath
        /// </summary>
        /// <param name="folderPath">Folder path</param>
        /// <param name="levelName">Level name</param>
        /// <param name="prefix">Type of resource.</param>
        /// <returns></returns>
        public string GetFilePath(string folderPath, string levelName, string suffix)
        {
            return $"{folderPath}/{levelName}_{suffix}.asset";
        }

        /// <summary>
        /// Check if folder directory exists.
        /// </summary>
        /// <param name="folderPath">Folder path</param>
        private bool FolderExists(string folderPath)
        {
            return System.IO.Directory.Exists(Application.dataPath + "/" + folderPath.Remove(0, 7));
        }
    }
}