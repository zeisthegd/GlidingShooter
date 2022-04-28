using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;

using TMPro;

using Penwyn.Game;
using Penwyn.Tools;

public class TitleScreenUI : MonoBehaviourPunCallbacks
{
    public TMP_InputField NickNameTxt;
    public bool RandomNickname;
    public bool AutoConnect;

    void Start()
    {
        if (RandomNickname)
            NickNameTxt.text = Randomizer.RandomString(4);
        if (RandomNickname && AutoConnect)
            StartGame();
    }

    public void StartGame()
    {
        SceneManager.Instance.LoadLobbyScene();
    }


}
