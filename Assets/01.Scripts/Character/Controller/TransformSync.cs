using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Penwyn.Game
{
    public class TransformSync : MonoBehaviourPun, IPunObservable
    {
        public AnimationCurve VelocityMagnitudeToLerpSpeedCurve;
        protected Vector3 _remoteVelocity;
        protected Vector3 _remoteAngle;
        protected float _remoteVelocityMagnitude = 0;
        protected PhotonTransformViewClassic _photonViewTransformClassic;
        protected CharacterController _controller;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _photonViewTransformClassic = GetComponent<PhotonTransformViewClassic>();
        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                //_controller.SetVelocity(Vector3.Lerp(_controller.Velocity, _remoteVelocity, VelocityMagnitudeToLerpSpeedCurve.Evaluate(_remoteVelocityMagnitude) * Time.deltaTime));
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_controller.Velocity);
                stream.SendNext(transform.eulerAngles);
            }

            if (stream.IsReading)
            {
                _remoteVelocity = (Vector3)stream.ReceiveNext();
                _remoteAngle = (Vector3)stream.ReceiveNext();
                _remoteVelocityMagnitude = _remoteVelocity.magnitude;

                if (_photonViewTransformClassic)
                {
                    float velocityDifference = Mathf.Abs(_remoteVelocityMagnitude - _controller.Velocity.magnitude);
                    float angleDifference = Vector3.Angle(_remoteAngle, _controller.transform.eulerAngles);
                    _photonViewTransformClassic.m_PositionModel.TeleportIfDistanceGreaterThan = VelocityMagnitudeToLerpSpeedCurve.Evaluate(velocityDifference);
                    _photonViewTransformClassic.m_PositionModel.InterpolateLerpSpeed = VelocityMagnitudeToLerpSpeedCurve.Evaluate(velocityDifference);
                    _photonViewTransformClassic.m_RotationModel.InterpolateLerpSpeed = VelocityMagnitudeToLerpSpeedCurve.Evaluate(Mathf.Abs(angleDifference));
                }
            }
        }

    }

}
