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
            pauseObjectiveText.text = "Defeat The Eel";
        }
        if(GameDataHolder.objectiveId == 6)
        {
            pauseObjectiveText.text = "Find The Exit To The Cave";
        }
        if(GameDataHolder.objectiveId == 7)
        {
            pauseObjectiveText.text = "Descend Into The Cave";
        }
        if(GameDataHolder.objectiveId == 8)
        {
            pauseObjectiveText.text = "Reach The Cage Before It Closes";
        }
        if(GameDataHolder.objectiveId == 9)
        {
            pauseObjectiveText.text = "Find A Way To Destroy The Biolamps";
        }
        if(GameDataHolder.objectiveId == 10)
        {
            pauseObjectiveText.text = "Find Your Way Out Of The Marsh";
        }
    }
}
