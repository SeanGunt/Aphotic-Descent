// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && UNITY_2020_3_OR_NEWER
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MH.WaterCausticsModules {
    public class WaterCausticsTexGeneratorMenuItem {
        [MenuItem ("GameObject/WaterCausticsModules/TexGen (with RenderTexture Asset)", false, 1)]
        static public WaterCausticsTexGenerator CreateTexGeneratorWithRT () {
            var texGen = createGameObject ();
            texGen.renderTexture = CreateRTAsset ();
            Selection.activeObject = texGen.gameObject;
            return texGen;
        }

        [MenuItem ("GameObject/WaterCausticsModules/TexGen", false, 2)]
        static public void CreateTexGeneratorGO () {
            createGameObject ();
        }

        static private WaterCausticsTexGenerator createGameObject () {
            var go = new GameObject ("WaterCausticsTexGen");
            Undo.RegisterCreatedObjectUndo (go, "Create WaterCausticsTexGen");
            var tra = go.transform;
            if (Selection.activeGameObject != null) {
                var baseTra = Selection.activeGameObject.transform;
                tra.parent = baseTra.parent;
                tra.SetSiblingIndex (baseTra.GetSiblingIndex () + 1);
            }
            tra.localPosition = Vector3.zero;
            tra.localRotation = Quaternion.identity;
            tra.localScale = Vector3.one;
            Selection.activeObject = go;
            return go.AddComponent<WaterCausticsTexGenerator> ();
        }

        static public RenderTexture CreateRTAsset () {
            var rtName = "WaterCausticsRT";
            var scene = SceneManager.GetActiveScene ();
            var path = scene.path;
            if (string.IsNullOrEmpty (path))
                path = $"Assets/{rtName}";
            else
                path = $"{Path.GetDirectoryName (path).Replace ("\\", "/")}/{scene.name}_{rtName}";
            int idx = 1;
            var savePath = $"{path}.renderTexture";
            while (File.Exists (savePath))
                savePath = $"{path}{++idx}.renderTexture";
            RenderTextureDescriptor desc = new RenderTextureDescriptor (512, 512, RenderTextureFormat.ARGBHalf, 0);
            desc.autoGenerateMips = false;
            desc.msaaSamples = 1;
            desc.useMipMap = true;
            desc.autoGenerateMips = true;
            RenderTexture rt = new RenderTexture (desc);
            rt.anisoLevel = 4;
            rt.wrapMode = TextureWrapMode.Repeat;
            rt.Create ();
            AssetDatabase.CreateAsset (rt, savePath);
            AssetDatabase.SaveAssets ();
            EditorGUIUtility.PingObject (rt);
            Debug.Log ("A new RenderTexture was created at the following location.\n\"" + savePath + "\"\n");
            return rt;
        }

    }
}

#endif
