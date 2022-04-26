using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public string PlayerPrefabPath;
        //Player Lists
        protected List<Character> _playersInRoom;
        [ReadOnly] public Character LocalPlayer;

        /// <summary>
        /// Instantiate the player model when they enter the room.
        /// </summary>
        public void CreateLocalPlayer()
        {
            if (PhotonNetwork.InRoom && LocalPlayer == null)
            {
                Debug.Log("Create player");
                Transform spawnPos = GameObject.Find("SpawnPos").transform;
                GameObject player = PhotonNetwork.Instantiate(PlayerPrefabPath, spawnPos.position, Quaternion.identity);
                player.name = PhotonNetwork.NickName;
                LocalPlayer = player.FindComponent<Character>();
                CameraManager.Instance.CurrenPlayerCam.FollowPlayer();
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

        #region Utilities

        /// <summary>
        /// Find a player by their photonview's owner actor number.
        /// </summary>
        public Character FindByOwnerActorNumber(int ownerActNr)
        {
            FindPlayersInRooms();
            return _playersInRoom.Find(player => player.photonView.OwnerActorNr == ownerActNr);
        }

        #endregion

        void OnEnable()
        {

        }

        public List<Character> PlayerInRoom { get => _playersInRoom; }
    }
}

