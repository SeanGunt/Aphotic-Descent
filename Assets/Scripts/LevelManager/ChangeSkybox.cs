using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material skyMaterial1;
    public Material skyMaterial2;

   
    void Update()
    {
        if (GameDataHolder.inSub)
        {
            RenderSettings.skybox = skyMaterial1;
        }
        if (GameDataHolder.inMudMarsh || GameDataHolder.inAnglerTrench)
        {
            RenderSettings.skybox = skyMaterial2;
        }
    }
    
}
