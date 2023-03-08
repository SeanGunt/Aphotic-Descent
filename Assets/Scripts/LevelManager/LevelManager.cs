using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject subTutorial, kelpMaze, crabLab, eelCave, psShrimpCave, mudMarsh, anglerTrench;
    [SerializeField] private Light mainLight;
    private Color fogColor, lightColor;
    
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

            ChangeLighting("#084A6C","#133F59", 0.03f, 0.15f);
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

            ChangeLighting("#294163", "#428FB2", 0.03f, 0.8f);
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

            ChangeLighting("#084A6C","#133F59", 0.03f, 0.15f);
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
            ChangeLighting("#084A6C","#133F59", 0.02f, 0.15f);
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

            ChangeLighting("#084A6C","#133F59", 0.03f, 0.15f);
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

            ChangeLighting("#084A6C","#133F59", 0.03f, 0.15f);
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

            ChangeLighting("#084A6C","#133F59", 0.03f, 0.15f);
        }
    }

    private void ChangeLighting(string a, string b, float density, float lighStrength)
    {
        mainLight.intensity = lighStrength;
        if (ColorUtility.TryParseHtmlString(a, out lightColor))
        {
            mainLight.color = lightColor;
        }
        RenderSettings.fogDensity = density;
        if (ColorUtility.TryParseHtmlString(b, out fogColor))
        {
            RenderSettings.fogColor = fogColor;
        }
    }
}
