using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;
using Photon.Pun;

namespace Penwyn.Game
{
    public class TimeManager : SingletonMonoBehaviour<TimeManager>
    {
        protected PhotonView _photonView;
        protected virtual void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            PlayerManager.Instance.PlayerSpawned += OnPlayerSpawned;
        }

        void Update()
        {
            if (GameManager.Instance.State == GameState.Started && CombatManager.Instance.IsLocalPlayerTurn)
            {
                if (InputReader.Instance.MoveInput.magnitude > 0)
                {
                    if (Time.timeScale == 0)
                        _photonView.RPC(nameof(ResetTime), RpcTarget.All);
                }
                else
                {
                    if (Time.timeScale == 1)
                        _photonView.RPC(nameof(StopTime), RpcTarget.All);
                }
            }
        }

        [PunRPC]
        public virtual void ResetTime()
        {
            SetTimeScale(1);
        }

        [PunRPC]
        public virtual void StopTime()
        {
            SetTimeScale(0);
        }

        public virtual void SetTimeScale(float newTimeScale)
        {
            Time.timeScale = newTimeScale;
            PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = newTimeScale == 0 ? 0 : -1f;
        }

        public virtual void LocalPlayerIsActing()
        {

        }

        public virtual void OnPlayerSpawned()
        {
            ConnectEvents();
        }

        public virtual void ConnectEvents()
        {

        }

        public virtual void DisconnectEvents()
        {
            PlayerManager.Instance.PlayerSpawned -= OnPlayerSpawned;
        }
    }
}
