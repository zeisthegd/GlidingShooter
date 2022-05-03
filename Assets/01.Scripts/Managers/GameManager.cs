using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        [Expandable] public CameraManager CameraManager;
        [Expandable] public SceneManager SceneManager;
        [Expandable] public PlayerManager PlayerManager;
        [Expandable] public InputManager InputManager;

        public AudioPlayer AudioPlayer;
        public CombatManager CombatManager;
        public LevelManager LevelManager;

        [Header("Utilities")]
        public CursorUtility CursorUtility;

        //[SerializeField] AudioPlayer audioPlayer;

        [Header("Level Stuff")]
        //[SerializeField] Level levelPref;
        public string LevelPath;
        [Expandable] public MatchSettings MatchSettings;

        //   Level currentLevel;

        public static GameManager Instance;
        public event UnityAction GameStarted;
        protected GameState _gameState;


        void Awake()
        {
            CheckSingleton();
        }

        void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoad;
        }

        public virtual void RPC_StartGame()
        {
            photonView.RPC(nameof(StartGame), RpcTarget.All);
        }

        [PunRPC]
        public virtual void StartGame()
        {
            _gameState = GameState.Started;
            CombatManager.StartGame();
            LevelManager.LoadLevelByIndex(0);
            GameStarted?.Invoke();
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
            PlayerManager.CreateLocalPlayer();
            _gameState = GameState.TeamChoosing;
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SceneManager.RoomSceenName)
            {
                OnRoomSceneLoaded();
            }
        }

        public GameState State => _gameState;

    }

    public enum GameState
    {
        NotInRoom,
        TeamChoosing,
        Started
    }
}
