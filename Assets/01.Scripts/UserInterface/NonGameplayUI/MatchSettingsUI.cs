using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Penwyn.Game;
using TMPro;

namespace Penwyn.UI
{
    public class MatchSettingsUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField thiefCount;
        [SerializeField] TMP_InputField treasureCount;

        [SerializeField] TMP_InputField thiefViewRadius;
        [SerializeField] TMP_InputField guardViewRadius;

        MatchSettings settings;

        void LoadSettings(MatchSettings matchSettings)
        {
            thiefCount.text = settings.ThiefCount.ToString();
            treasureCount.text = settings.TreasureCount.ToString();
            thiefViewRadius.text = settings.ThiefViewRadius.ToString();
            guardViewRadius.text = settings.GuardViewRadius.ToString();
        }

        void OnEnable()
        {
            settings = GameManager.Instance.MatchSettings;
            LoadSettings(settings);
        }
    }

}