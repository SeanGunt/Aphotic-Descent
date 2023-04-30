using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    private bool zooplanktonAchievementAcquired, eelkilledAchievementAcquired, allLogsCollected;

    private void Update()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if (GameDataHolder.numOfZooplanktonCollected >= 20 && !zooplanktonAchievementAcquired)
        {
            SteamUserStats.SetAchievement("All_Zooplankton_Found");
            SteamUserStats.StoreStats();
            zooplanktonAchievementAcquired = true;
        }

        if (GameDataHolder.eelIsDead && !eelkilledAchievementAcquired)
        {
            SteamUserStats.SetAchievement("Eel_Defeated");
            SteamUserStats.StoreStats();
            eelkilledAchievementAcquired = true;
        }

        if(GameDataHolder.numOfLogsCollected >= 33 && !allLogsCollected)
        {
            SteamUserStats.SetAchievement("All_Logs_Collected");
            SteamUserStats.StoreStats();
            allLogsCollected = true;
        }
    }
}
