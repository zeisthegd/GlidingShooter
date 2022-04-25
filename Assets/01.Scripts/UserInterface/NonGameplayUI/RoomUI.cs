using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon;
using Photon.Pun;

using Penwyn.Game;

namespace Penwyn.UI
{
    public class RoomUI : MonoBehaviour
    {
        [SerializeField] TMP_Text passcodeTxt;
        [SerializeField] Button openSettingsBtn;
        [SerializeField] Button startMatchBtn;
        void Start()
        {
            if (PhotonNetwork.InRoom)
                passcodeTxt.text = (string)PhotonNetwork.CurrentRoom.CustomProperties["Passcode"];

            if (!PhotonNetwork.IsMasterClient)
            {
                openSettingsBtn.gameObject.SetActive(false);
                startMatchBtn.gameObject.SetActive(false);
            }
        }

        public void StartMatch()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                SceneManager.Instance.LoadMatchScene();
            }
        }
    }

}

