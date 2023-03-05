using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject subTutorial, kelpMaze, crabLab, eelCave, psShrimpCave, mudMarsh, anglerTrench;
    
    void Update()
    {
        if(GameDataHolder.inSub)
        {
            subTutorial.gameObject.SetActive(true);
            kelpMaze.gameObject.SetActive(false);
            crabLab.gameObject.SetActive(false);
            eelCave.gameObject.SetActive(false);
            psShrimpCave.gameObject.SetActive(false);
            mudMarsh.gameObject.SetActive(false);
            anglerTrench.gameObject.SetActive(false);
        }
        else if(GameDataHolder.inKelpMaze)
        {
            subTutorial.gameObject.SetActive(false);
            kelpMaze.gameObject.SetActive(true);
            crabLab.gameObject.SetActive(false);
            eelCave.gameObject.SetActive(false);
            psShrimpCave.gameObject.SetActive(false);
            mudMarsh.gameObject.SetActive(false);
            anglerTrench.gameObject.SetActive(false);
        }
        else if(GameDataHolder.inLab)
        {
            subTutorial.gameObject.SetActive(false);
            kelpMaze.gameObject.SetActive(false);
            crabLab.gameObject.SetActive(true);
            eelCave.gameObject.SetActive(false);
            psShrimpCave.gameObject.SetActive(false);
            mudMarsh.gameObject.SetActive(false);
            anglerTrench.gameObject.SetActive(false);
        }
        else if(GameDataHolder.inEelCave)
        {
            subTutorial.gameObject.SetActive(false);
            kelpMaze.gameObject.SetActive(false);
            crabLab.gameObject.SetActive(false);
            eelCave.gameObject.SetActive(true);
            psShrimpCave.gameObject.SetActive(false);
            mudMarsh.gameObject.SetActive(false);
            anglerTrench.gameObject.SetActive(false);
        }
        else if(GameDataHolder.inPsShrimpCave)
        {
            subTutorial.gameObject.SetActive(false);
            kelpMaze.gameObject.SetActive(false);
            crabLab.gameObject.SetActive(false);
            eelCave.gameObject.SetActive(false);
            psShrimpCave.gameObject.SetActive(true);
            mudMarsh.gameObject.SetActive(false);
            anglerTrench.gameObject.SetActive(false);
        }
        else if(GameDataHolder.inMudMarsh)
        {
            subTutorial.gameObject.SetActive(false);
            kelpMaze.gameObject.SetActive(false);
            crabLab.gameObject.SetActive(false);
            eelCave.gameObject.SetActive(false);
            psShrimpCave.gameObject.SetActive(false);
            mudMarsh.gameObject.SetActive(true);
            anglerTrench.gameObject.SetActive(false);
        }
        else if(GameDataHolder.inAnglerTrench)
        {
            subTutorial.gameObject.SetActive(false);
            kelpMaze.gameObject.SetActive(false);
            crabLab.gameObject.SetActive(false);
            eelCave.gameObject.SetActive(false);
            psShrimpCave.gameObject.SetActive(false);
            mudMarsh.gameObject.SetActive(false);
            anglerTrench.gameObject.SetActive(true);
        }
    }
}
