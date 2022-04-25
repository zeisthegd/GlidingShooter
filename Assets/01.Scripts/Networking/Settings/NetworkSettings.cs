using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{

    [CreateAssetMenu(menuName = "Settings/Network")]
    public class NetworkSettings : ScriptableObject
    {
        [SerializeField] string gameVersion;
        [SerializeField] string nickName;
        [SerializeField] bool automaticallySyncScene;

        public string GameVersion { get => gameVersion; }
        public string NickName { get => nickName; set => nickName = value; }
        public bool AutomaticallySyncScene { get => automaticallySyncScene; }
    }
}
