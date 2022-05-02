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
        [ReadOnly] public PhotonTeam CurrentTeamTurn;
        [ReadOnly] public Player CurrentPlayerTurn;

        protected Player[] _firstTeamPlayers;
        protected Player[] _secondTeamPlayers;
        protected PhotonTeam _firstTeam;
        protected PhotonTeam _secondTeam;
        
        void Awake()
        {
            GameManager.Instance.GameStarted += GameStarted;
        }

        public virtual void GameStarted()
        {
            PhotonTeamsManager.Instance.TryGetTeamByCode(1, out _firstTeam);
            PhotonTeamsManager.Instance.TryGetTeamByCode(2, out _secondTeam);

            PhotonTeamsManager.Instance.TryGetTeamMembers(1, out _firstTeamPlayers);
            PhotonTeamsManager.Instance.TryGetTeamMembers(2, out _secondTeamPlayers);

            CurrentPlayerTurn = _firstTeamPlayers[0];
        }

        public virtual void DisconnectEvents()
        {

        }

        protected virtual void OnDisable()
        {
            DisconnectEvents();
        }

    }

    public enum Turn
    {
        Self,
        Enemy,
        Teammate
    }
}

