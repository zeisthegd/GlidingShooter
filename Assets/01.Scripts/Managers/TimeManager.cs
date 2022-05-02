using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Tools;

namespace Penwyn.Game
{
    public class TimeManager : SingletonMonoBehaviour<TimeManager>
    {
        bool gameStarted = false;
        protected virtual void Awake()
        {
            PlayerManager.Instance.PlayerSpawned += OnPlayerSpawned;
        }

        void Update()
        {
            if (GameManager.Instance.State == GameState.Started)
            {
                if (InputReader.Instance.MoveInput.magnitude > 0)
                    ResetTime();
                else
                    StopTime();
            }
        }

        public virtual void ResetTime()
        {
            SetTimeScale(1);
        }

        public virtual void StopTime()
        {
            SetTimeScale(0);
        }

        public virtual void SetTimeScale(float newTimeScale)
        {
            Time.timeScale = newTimeScale;
        }

        public virtual void LocalPlayerIsActing()
        {

        }

        public virtual void OnPlayerSpawned()
        {
            gameStarted = true;
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
