using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseObjectiveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pauseObjectiveText;
    private void Update()
    {
        if (GameDataHolder.objectiveId >= 1)
        {
            pauseObjectiveText.text = "Escape the Submarine";
        }
        
        if (GameDataHolder.objectiveId == 2)
        {
            pauseObjectiveText.text = "Find A Way Through The Kelp Maze";
        }

        if (GameDataHolder.objectiveId == 3)
        {
            pauseObjectiveText.text = "Proceed To The Lab";
        }

        if (GameDataHolder.objectiveId == 4)
        {
            pauseObjectiveText.text = "Investigate The Lab";
        }

        if (GameDataHolder.objectiveId == 5)
        {
            pauseObjectiveText.text = "Neutralize The Eel";
        }
        if(GameDataHolder.objectiveId == 6)
        {
            pauseObjectiveText.text = "End Its Suffering";
        }
        if(GameDataHolder.objectiveId == 7)
        {
            pauseObjectiveText.text = "Find The Exit To The Cave";
        }
        if(GameDataHolder.objectiveId == 8)
        {
            pauseObjectiveText.text = "Descend Into The Cave";
        }
        if(GameDataHolder.objectiveId == 9)
        {
            pauseObjectiveText.text = "Reach The Cage Before It Closes";
        }
        if(GameDataHolder.objectiveId == 10)
        {
            pauseObjectiveText.text = "Cut The Biolamps Down";
        }
        if(GameDataHolder.objectiveId == 11)
        {
            pauseObjectiveText.text = "Repel The Creature";
        }
        if(GameDataHolder.objectiveId == 12)
        {
            pauseObjectiveText.text = "Find Your Way Through The Marsh";
        }
        if(GameDataHolder.objectiveId == 13)
        {
            pauseObjectiveText.text = "Find Your Submarine";
        }
    }
}
