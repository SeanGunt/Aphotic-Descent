// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && WCE_URP
using UnityEditor;
using UnityEngine;

namespace MH.WaterCausticsModules {
    public class WaterCausticsEffectMenuItem {
        [MenuItem ("GameObject/WaterCausticsModules/TexGen and Effect (with RenderTexture Asset)", false, 0)]
        static public void CreateTexGenAndEffectPair () {
            var texGen = WaterCausticsTexGeneratorMenuItem.CreateTexGeneratorWithRT ();
            var effect = createEffectGO ();
            effect.texture = texGen.renderTexture;
        }

        [MenuItem ("GameObject/WaterCausticsModules/Effect", false, 3)]
        static public void CreateEffectGO () {
            GameObject selectedGO = Selection.activeGameObject;
            var effect = createEffectGO ();
            var texGen = selectedGO?.GetComponent<WaterCausticsTexGenerator> ();
            if (texGen == null)
                texGen = Object.FindObjectOfType<WaterCausticsTexGenerator> ();
            if (texGen != null)
                effect.texture = texGen.renderTexture;
        }

        static private WaterCausticsEffect createEffectGO () {
            var go = new GameObject ("WaterCausticsEffect");
            Undo.RegisterCreatedObjectUndo (go, "Create WaterCausticsEffect");
            var tra = go.transform;
            if (Selection.activeGameObject != null) {
                var baseTra = Selection.activeGameObject.transform;
                tra.parent = baseTra.parent;
                tra.SetSiblingIndex (baseTra.GetSiblingIndex () + 1);
            }
            tra.localPosition = Vector3.zero;
            tra.localRotation = Quaternion.identity;
            tra.localScale = Vector3.one * 3f;
            Selection.activeObject = go;
            Selection.activeObject = null;
            Selection.activeObject = go;
            return go.AddComponent<WaterCausticsEffect> ();
        }

    }
}
#endif
