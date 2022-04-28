using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

using NaughtyAttributes;

using Penwyn.Tools;
using Penwyn.UI;

namespace Penwyn.Game
{

    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [Header("Managers")]
        public LevelManager LevelManager;
        [Expandable] public CameraManager CameraManager;
        [Expandable] public SceneManager SceneManager;
        [Expandable] public PlayerManager PlayerManager;
        [Expandable] public InputManager InputManager;
        [Expandable] public AudioPlayer AudioPlayer;


        [Expandable] public MatchSettings MatchSettings;


        void Awake()
        {
            CheckSingleton();
        }

        void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        }

        void CheckSingleton()
        {
            DontDestroyOnLoad(this);
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneManager.RoomSceenName)
            {
                CameraManager.CreatePlayerCamera();
                PlayerManager.CreateLocalPlayer();
                LevelManager.LoadLevel();
            }
        }
    }
}
