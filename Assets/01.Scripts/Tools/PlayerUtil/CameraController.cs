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
            Follow(PlayerManager.Instance.LocalPlayer.transform, PlayerManager.Instance.LocalPlayer.transform);
        }

        public virtual void Follow(Transform followTrf, Transform lookAtTrf)
        {
            _virtualCamera.Follow = followTrf;
            _virtualCamera.LookAt = lookAtTrf;
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
        }

        public virtual void DisconnectEvents()
        {

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

