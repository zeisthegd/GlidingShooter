using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;
using Photon.Realtime;

using Penwyn.Tools;
using Penwyn.UI;

namespace Penwyn.Game
{

    public class GameManager : MonoBehaviourPun
    {
        [Header("Managers")]
        [SerializeField] NetworkManager networkManager;
       // [SerializeField] CameraManager cameraManager;
        [SerializeField] SceneManager sceneManager;
       // [SerializeField] PlayerManager playerManager;
        //[SerializeField] UIManager uiManager;

        [Header("Utilities")]
        [SerializeField] CursorUtility cursorUtility;
        [SerializeField] InputReader inputReader;

        //[SerializeField] AudioPlayer audioPlayer;
        [SerializeField] Announcer announcer;

        [Header("Level Stuff")]
        //[SerializeField] Level levelPref;
        [SerializeField] string levelPath;
        [SerializeField] MatchSettings matchSettings;

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


        [PunRPC]
        /// <summary>
        /// Assign the local player to a random position.
        /// </summary>
        public void SetUpPlayersOnClients()
        {
        }


        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {

        }

        public MatchSettings MatchSettings { get => matchSettings; }

        public Announcer Announcer { get => announcer; }
        
    
        public InputReader InputReader { get => inputReader; }
    }
}
