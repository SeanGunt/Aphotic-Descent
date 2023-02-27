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
    }
}
