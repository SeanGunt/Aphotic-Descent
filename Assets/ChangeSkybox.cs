using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material skyMaterial1;
    public Material skyMaterial2;

    public GameObject MudHeart, Trench;
   
    void Start()
    {
        RenderSettings.skybox = skyMaterial1;
    }

   
    void Update()
    {
        if (MudHeart.activeInHierarchy == true)
 {
        RenderSettings.skybox = skyMaterial2;
    }
    }
    
}
