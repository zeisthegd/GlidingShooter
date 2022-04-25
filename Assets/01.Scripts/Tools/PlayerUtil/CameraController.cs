using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using DG.Tweening;
using Penwyn.Game;

namespace Penwyn.Tools
{
    public class CameraController : MonoBehaviour
    {
        protected CinemachineVirtualCamera _virtualCamera;
        protected CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

        protected virtual void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _virtualCameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public virtual void FollowPlayer()
        {
            _virtualCamera.Follow = Characters.Player.transform;
            _virtualCamera.LookAt = Characters.Player.transform;
        }

        public virtual void StartShaking(float amplitude, float frequency)
        {
            if (_virtualCamera != null && _virtualCameraNoise != null)
            {
                _virtualCameraNoise.m_AmplitudeGain = amplitude;
                _virtualCameraNoise.m_FrequencyGain = frequency;
            }

        }

        public virtual void SetFOV(float newFOV)
        {
            if (_virtualCamera != null)
                _virtualCamera.m_Lens.FieldOfView = newFOV;
        }

        public virtual void ConnectEvents()
        {
            LevelManager.PlayerSpawned += FollowPlayer;
        }

        public virtual void DisconnectEvents()
        {
            LevelManager.PlayerSpawned -= FollowPlayer;

        }

        protected virtual void OnEnable()
        {
            ConnectEvents();
        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }
    }
}

