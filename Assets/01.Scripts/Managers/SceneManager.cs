using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Photon;
using Photon.Pun;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        [Header("Scene names")]
        [SerializeField] string title;
        [SerializeField] string lobby;
        [SerializeField] string room;
        [SerializeField] string match;

        #region Load Scenes
        public void LoadTitleScene()
        {
            LoadScene(title);
        }

        public void LoadLobbyScene()
        {
            LoadScene(lobby);
        }

        public void LoadRoomScene()
        {
            Photon.Pun.PhotonNetwork.LoadLevel(2);
        }

        void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        #endregion

        #region Load Level

        public void LoadMatchScene()
        {
            PhotonNetwork.LoadLevel(match);
        }

        #endregion


        public void ExitGame()
        {
            NetworkManager.Instance.Disconnect();
            Application.Quit();
        }

        public string Title { get => title; }
        public string Lobby { get => lobby; }
        public string Room { get => room; }
        public string Match { get => match; }
    }
}
