using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Settings/Match")]
    public class MatchSettings : ScriptableObject
    {
        [SerializeField] int thiefCount;
        [SerializeField] int treasureCount;

        [SerializeField] int guardViewRadius;
        [SerializeField] int thiefViewRadius;

        public void SetThiefCountFromText(string text)
        {
            thiefCount = Convert.ToInt32(text);
        }

        public void SetTreasureCountFromText(string text)
        {
            treasureCount = Convert.ToInt32(text);
        }

        public void SetGuardViewRadiusFromText(string text)
        {
            guardViewRadius = Convert.ToInt32(text);
        }

        public void SetThiefViewRadiusFromText(string text)
        {
            thiefViewRadius = Convert.ToInt32(text);
        }

        public int ThiefCount { get => thiefCount; }
        public int TreasureCount { get => treasureCount; set => treasureCount = value; }
        public int GuardViewRadius { get => guardViewRadius; }
        public int ThiefViewRadius { get => thiefViewRadius; }
    }
}

