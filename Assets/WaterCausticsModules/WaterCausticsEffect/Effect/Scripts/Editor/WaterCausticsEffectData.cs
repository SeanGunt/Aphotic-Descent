// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && WCE_URP
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MH.WaterCausticsModules {
    public class WaterCausticsEffectData : ScriptableObject {
        // ---------------------------------------------------------
        [SerializeField] internal bool m_autoManageFeature = true;
        internal bool AutoManageFeature => m_autoManageFeature;

        // --------------------------------------------------------- 
        static private WaterCausticsEffectData _s_ins;
        static internal WaterCausticsEffectData GetAsset () {
            if (_s_ins) return _s_ins;
            if (findAsset<WaterCausticsEffectData> (out var ins, out var path)) return _s_ins = ins;
            return _s_ins = createAsset<WaterCausticsEffectData> ();
        }

        static private bool findAsset<T> (out T asset, out string path) where T : Object {
            asset = null;
            path = null;
            var guids = AssetDatabase.FindAssets ($"t:{typeof (T).ToString()}", new [] { "Assets" });
            if (guids.Length == 0) return false;
            path = AssetDatabase.GUIDToAssetPath (guids [0]);
            asset = AssetDatabase.LoadAssetAtPath<T> (path);
            return asset != null;
        }

        static private T createAsset<T> () where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T> ();
            MonoScript mono = MonoScript.FromScriptableObject (asset);
            string scriptPath = AssetDatabase.GetAssetPath (mono);
            string folderPath = Path.GetDirectoryName (scriptPath).Replace ("\\", "/");
            string path = $"{folderPath}/{Path.GetFileNameWithoutExtension (scriptPath)}.asset";
            AssetDatabase.CreateAsset (asset, path);
            AssetDatabase.SaveAssets ();
            return asset;
        }

        // ---------------------------------------------------------
    }
}
#endif
