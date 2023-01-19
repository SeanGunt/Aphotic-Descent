// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if WCE_URP
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_SetLightShadowRez : MonoBehaviour {
#if UNITY_EDITOR
        /*------------------------------------------------------------------------ 
        Set the shadow resolution of additional lights to Mid since URP11 
        to avoid the warning "Reduced additional punctual light shadows resolution...".
        -------------------------------------------------------------------------*/
        private void Awake () {
            var lightAry = Resources.FindObjectsOfTypeAll<Light> ();
            foreach (var light in lightAry) {
                if (EditorUtility.IsPersistent (light)) continue;
                if (light.type == LightType.Directional) continue;
                var lightData = light.GetComponent<UniversalAdditionalLightData> ();
                if (!lightData)
                    lightData = light.gameObject.AddComponent<UniversalAdditionalLightData> ();
                if (lightData) {
                    Type type = typeof (UniversalAdditionalLightData);
                    FieldInfo field = type.GetField ("m_AdditionalLightsShadowResolutionTier", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != null) {
                        int val = (int) field.GetValue (lightData);
                        if (val == 2) {
                            // Low:0 Mid:1 High:2
                            if (light.type == LightType.Spot)
                                field.SetValue (lightData, 1);
                            if (light.type == LightType.Point)
                                field.SetValue (lightData, 0);
                        }
                    }
                }
            }
        }
#endif
    }
}
#endif // end of WCE_URP
