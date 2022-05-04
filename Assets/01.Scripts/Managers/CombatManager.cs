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
    public class CombatManager : SingletonMonoBehaviour<CombatManager>
    {
        [ReadOnly] public Player CurrentPlayer;

        protected Queue<Player> _turnQueue = new Queue<Player>();
        protected PhotonView _photonView;


        protected TeamData _firstTeam;
        protected TeamData _secondTeam;

        public event UnityAction TurnChanged;
        public event UnityAction ScoreChanged;
        public event UnityAction ATeamWon;

        public virtual void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _firstTeam = new TeamData();
            _secondTeam = new TeamData();

            _firstTeam.CurrentDeath = 0;
            _secondTeam.CurrentDeath = 0;
        }

        public virtual void StartGame()
        {
            PhotonTeamsManager.Instance.TryGetTeamByCode(1, out _firstTeam.Team);
            PhotonTeamsManager.Instance.TryGetTeamByCode(2, out _secondTeam.Team);

            PhotonTeamsManager.Instance.TryGetTeamMembers(1, out _firstTeam.Players);
            PhotonTeamsManager.Instance.TryGetTeamMembers(2, out _secondTeam.Players);

            CreateNewTurnQueue();
            ConnectPlayerEvents();
        }

        public virtual void StartCurrentPlayerTurn()
        {
            if (IsLocalPlayerTurn)
            {
                InputReader.Instance.EnableGameplayInput();
                PlayerManager.Instance.LocalPlayer.Energy.Set(1);
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
            TurnChanged?.Invoke();
        }

        public virtual void LocalPlayerEndTurn()
        {
            if (IsLocalPlayerTurn)
            {
                RPC_NextTurn();
            }
        }

        public virtual void LocalPlayerDeath(Character player)
        {
            _photonView.RPC(nameof(PlayerDeath), RpcTarget.All, PhotonNetwork.LocalPlayer);
        }

        [PunRPC]
        public virtual void PlayerDeath(Player player)
        {
            if (player.GetPhotonTeam() == _firstTeam.Team)
            {
                if (_firstTeam.Score > 0)
                    _firstTeam.Score -= 1;
                _secondTeam.Score += 1;
                _firstTeam.CurrentDeath++;
            }
            else
            {
                _firstTeam.Score += 1;
                if (_secondTeam.Score > 0)
                    _secondTeam.Score -= 1;
                _secondTeam.CurrentDeath++;
            }
            HandleAllTeamMemberDeath();
            ScoreChanged?.Invoke();
        }

        protected virtual void HandleAllTeamMemberDeath()
        {
            if (_firstTeam.CurrentDeath == _firstTeam.Players.Length || _secondTeam.CurrentDeath == _secondTeam.Players.Length)
            {
                GameManager.Instance.LoadNextLevel();
            }
        }

        public virtual void ResetDeathCount()
        {
            _firstTeam.CurrentDeath = 0;
            _secondTeam.CurrentDeath = 0;
        }

        public virtual bool IsSameTeam(Player player)
        {
            if (player.GetPhotonTeam() == PhotonNetwork.LocalPlayer.GetPhotonTeam())
                return true;
            return false;
        }

        public virtual void CreateNewTurnQueue()
        {
            _turnQueue.Clear();
            for (int i = 0; i < 100; i++)
                AddNewTurnRotation();
        }

        protected virtual void AddNewTurnRotation()
        {
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    if (_firstTeam.Players.Length > 0)
                        _turnQueue.Enqueue(_firstTeam.Players[i]);
                    if (_secondTeam.Players.Length > 0)
                        _turnQueue.Enqueue(_secondTeam.Players[i]);
                }
                else if (i == 1)
                {
                    if (_firstTeam.Players.Length > 1)
                        _turnQueue.Enqueue(_firstTeam.Players[i]);
                    else if (_firstTeam.Players.Length > 0)
                        _turnQueue.Enqueue(_firstTeam.Players[0]);

                    if (_secondTeam.Players.Length > 1)
                        _turnQueue.Enqueue(_secondTeam.Players[i]);
                    else if (_secondTeam.Players.Length > 0)
                        _turnQueue.Enqueue(_secondTeam.Players[0]);
                }
            }
        }

        public virtual void ConnectPlayerEvents()
        {
            PlayerManager.Instance.LocalPlayer.Energy.OutOfEnergy += LocalPlayerEndTurn;
            PlayerManager.Instance.LocalPlayer.Health.OnDeath += LocalPlayerDeath;
            PlayerManager.Instance.LocalPlayer.CharacterWeaponHandler.CurrentWeapon.WeaponUsed += LocalPlayerEndTurn;
        }

        public virtual void DisconnectEvents()
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.LocalPlayer != null)
            {
                PlayerManager.Instance.LocalPlayer.Energy.OutOfEnergy -= LocalPlayerEndTurn;
                PlayerManager.Instance.LocalPlayer.Health.OnDeath -= LocalPlayerDeath;
                PlayerManager.Instance.LocalPlayer.CharacterWeaponHandler.CurrentWeapon.WeaponUsed -= LocalPlayerEndTurn;
            }
        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }

        public Queue<Player> TurnQueue => _turnQueue;
        public bool IsLocalPlayerTurn => CurrentPlayer == PhotonNetwork.LocalPlayer;
        public TeamData FirstTeam => _firstTeam;
        public TeamData SecondTeam => _secondTeam;

    }

    public enum Turn
    {
        Self,
        Enemy,
        Teammate
    }
}

