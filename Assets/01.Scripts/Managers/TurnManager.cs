using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

using Photon.Realtime;

using NaughtyAttributes;
using Penwyn.Tools;

namespace Penwyn.Game
{
    public class TurnManager : SingletonMonoBehaviour<TurnManager>
    {
        [ReadOnly] public Player CurrentPlayer;

        protected Player[] _firstTeamPlayers;
        protected Player[] _secondTeamPlayers;
        protected PhotonTeam _firstTeam;
        protected PhotonTeam _secondTeam;

        protected Queue<Player> _turnQueue = new Queue<Player>();
        protected PhotonView _photonView;

        public event UnityAction TurnGenerated;
        public virtual void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        public virtual void StartGame()
        {
            PhotonTeamsManager.Instance.TryGetTeamByCode(1, out _firstTeam);
            PhotonTeamsManager.Instance.TryGetTeamByCode(2, out _secondTeam);

            PhotonTeamsManager.Instance.TryGetTeamMembers(1, out _firstTeamPlayers);
            PhotonTeamsManager.Instance.TryGetTeamMembers(2, out _secondTeamPlayers);

            for (int i = 0; i < 100; i++)
                AddNewTurnRotation();
            ConnectPlayerEvents();
            RPC_NextTurn();
            TurnGenerated?.Invoke();
        }

        public virtual void StartCurrentPlayerTurn()
        {
            if (IsLocalPlayerTurn)
            {
                PlayerManager.Instance.LocalPlayer.Energy.Set(1);
                InputReader.Instance.EnableGameplayInput();
            }
            else
            {
                InputReader.Instance.DisableGameplayInput();
            }
        }

        public virtual void RPC_NextTurn()
        {
            _photonView.RPC(nameof(NextTurn), RpcTarget.All);
        }

        [PunRPC]
        public virtual void NextTurn()
        {
            CurrentPlayer = _turnQueue.Dequeue();
            StartCurrentPlayerTurn();
        }

        public virtual void LocalPlayerEndTurn()
        {
            if (IsLocalPlayerTurn)
            {
                RPC_NextTurn();
            }
        }

        protected virtual void AddNewTurnRotation()
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    if (_firstTeamPlayers.Length > 0)
                        _turnQueue.Enqueue(_firstTeamPlayers[i]);
                    if (_secondTeamPlayers.Length > 0)
                        _turnQueue.Enqueue(_secondTeamPlayers[i]);
                }
                else if (i == 1)
                {
                    if (_firstTeamPlayers.Length > 1)
                        _turnQueue.Enqueue(_firstTeamPlayers[i]);
                    else if (_firstTeamPlayers.Length > 0)
                        _turnQueue.Enqueue(_firstTeamPlayers[0]);

                    if (_secondTeamPlayers.Length > 1)
                        _turnQueue.Enqueue(_secondTeamPlayers[i]);
                    else if (_secondTeamPlayers.Length > 0)
                        _turnQueue.Enqueue(_secondTeamPlayers[0]);
                }
            }
        }

        public virtual void ConnectPlayerEvents()
        {
            PlayerManager.Instance.LocalPlayer.Energy.OutOfEnergy += LocalPlayerEndTurn;
            PlayerManager.Instance.LocalPlayer.CharacterWeaponHandler.CurrentWeapon.WeaponUsed += LocalPlayerEndTurn;
        }

        public virtual void DisconnectEvents()
        {
            PlayerManager.Instance.LocalPlayer.Energy.OutOfEnergy -= LocalPlayerEndTurn;
            PlayerManager.Instance.LocalPlayer.CharacterWeaponHandler.CurrentWeapon.WeaponUsed -= LocalPlayerEndTurn;
        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }

        public Queue<Player> TurnQueue => _turnQueue;
        public bool IsLocalPlayerTurn => CurrentPlayer == PhotonNetwork.LocalPlayer;

    }

    public enum Turn
    {
        Self,
        Enemy,
        Teammate
    }
}

