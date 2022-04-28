using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon;
using Photon.Pun;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Managers/Player Manager")]
    public class PlayerManager : SingletonScriptableObject<PlayerManager>
    {
        [Header("Player")]
        public Character PlayerPrefab;
        //Player Lists
        protected List<Character> _playersInRoom;
        [ReadOnly] public Character LocalPlayer;

        public event UnityAction PlayerSpawned;

        /// <summary>
        /// Instantiate the player model when they enter the room.
        /// </summary>
        public void CreateLocalPlayer()
        {
            if (LocalPlayer == null)
            {
                Transform spawnPos = GameObject.Find("SpawnPos").transform;
                LocalPlayer = Instantiate(PlayerPrefab, spawnPos.position, Quaternion.identity);
                CameraManager.Instance.CurrenPlayerCam.FollowPlayer();
                PlayerSpawned?.Invoke();
                Debug.Log("Created player");
            }
        }

        public void FindPlayersInRooms()
        {
            var playerObjects = GameObject.FindGameObjectsWithTag("Player");
            _playersInRoom = new List<Character>();
            foreach (GameObject playerObj in playerObjects)
            {
                _playersInRoom.Add(playerObj.GetComponent<Character>());
            }
        }

        void OnEnable()
        {

        }

        public List<Character> PlayerInRoom { get => _playersInRoom; }
    }
}

