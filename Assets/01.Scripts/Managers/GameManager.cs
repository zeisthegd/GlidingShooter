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

    public class GameManager : MonoBehaviourPun
    {
        [Header("Managers")]
        [Expandable] public NetworkManager NetworkManager;
        [Expandable] public LevelManager LevelManager;
        [Expandable] public CameraManager CameraManager;
        [Expandable] public SceneManager SceneManager;
        [Expandable] public PlayerManager PlayerManager;
        [Expandable] public InputManager InputManager;
        [Expandable] public AudioPlayer AudioPlayer;

        [Header("Utilities")]
        public CursorUtility CursorUtility;

        //[SerializeField] AudioPlayer audioPlayer;

        [Header("Level Stuff")]
        //[SerializeField] Level levelPref;
        public string LevelPath;
        [Expandable] public MatchSettings MatchSettings;

        //   Level currentLevel;

        public static GameManager Instance;


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
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }


        public virtual void OnRoomSceneLoaded()
        {
            CameraManager.CreatePlayerCamera();
            PlayerManager.CreateLocalPlayer();
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneManager.RoomSceenName)
            {
                OnRoomSceneLoaded();
                LevelManager.LoadLevel();
            }
        }
    }
}
