// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && WCE_URP
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MH.WaterCausticsModules {
    [CustomEditor (typeof (WaterCausticsEffectFeature), true)]
    public class WaterCausticsEffectFeatureEditor : Editor {
        private GUIStyle _style;
        private void init () {
            _style = new GUIStyle (EditorStyles.label);
            _style.wordWrap = true;
            _style.fontSize -= 1;
        }
        public override void OnInspectorGUI () {
            if (_style == null) init ();
            EditorGUILayout.Space (10);
            string str = "This Renderer Function is required to apply WaterCausticsEffect.";
            GUILayout.Label (str, _style);
        }

        //-------------------------------------------------------------------------------------------------------------- 
        // static internal bool AddToCurrentRendererData (bool useUndo) {
        //     if (GetCurrentRendererData (out var dt))
        //         return AddFeatureToRenderer (dt, useUndo);
        //     return false;
        // }

        // static internal bool GetCurrentRendererData (out ScriptableRendererData dt) {
        //     dt = null;
        //     try {
        //         var urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        //         var propertyInfo = typeof (UniversalRenderPipelineAsset).GetProperty ("scriptableRendererData", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        //         if (propertyInfo == null) return false;
        //         dt = propertyInfo.GetValue (urpAsset) as ScriptableRendererData;
        //         return dt != null;
        //     } catch { }
        //     return false;
        // } 


        public class ModificationProcessor : UnityEditor.AssetModificationProcessor {
            // WaterCausticsEffectFeatureスクリプトが削除されるとき、登録済みのRendererFeatureを削除
            // ※フォルダ削除時、中身のパスは渡されないので注意
            private static AssetDeleteResult OnWillDeleteAsset (string deletePath, RemoveAssetOptions options) {
                bool isDirectory = File.GetAttributes (deletePath).HasFlag (FileAttributes.Directory);
                string scriptPath = getScriptPath<WaterCausticsEffectFeature> ();
                if ((isDirectory && scriptPath.StartsWith (deletePath)) || deletePath == scriptPath) {
                    // RendererFeatureを削除
                    DeleteAllFeatures ();
                }
                return AssetDeleteResult.DidNotDelete;
            }
        }

        static private string getScriptPath<T> () where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T> ();
            MonoScript mono = MonoScript.FromScriptableObject (asset);
            string path = AssetDatabase.GetAssetPath (mono);
            if (Application.isPlaying) Destroy (asset);
            else DestroyImmediate (asset);
            return path;
        }

#if WCE_DEVELOPMENT
        [MenuItem ("WCM/RendererFeatureTest/DeleteAllFeatures")]
#endif
        static internal void DeleteAllFeatures () {
            if (GetAllRendererData (out var list))
                foreach (var dt in list)
                    DeleteFeature (dt);
        }

        static internal void DeleteFeature (ScriptableRendererData dt) {
            if (!dt) return;
            try {
                // ※ TODO URPのバージョンが上がるたび処理方法に変更がないか要確認
                if (dt.rendererFeatures.Any (a => a is WaterCausticsEffectFeature)) {
                    if (!EditorUtility.IsPersistent (dt)) return;
                    var feature = dt.rendererFeatures.FirstOrDefault (a => a is WaterCausticsEffectFeature);
                    AssetDatabase.RemoveObjectFromAsset (feature);
                    dt.rendererFeatures.Remove (feature);
                    dt.SetDirty ();
                    EditorUtility.SetDirty (dt);
                    saveAssetSafe (dt);
                }
            } catch { }
        }

        static private void saveAssetSafe (Object o) {
            // ※AssetDatabase.SaveAssetIfDirty は Unity 2021.1.17、2020.3.16で追加されたので分岐
#if !UNITY_2020_3_OR_NEWER || UNITY_2020_3_0 || UNITY_2020_3_1 || UNITY_2020_3_2 || UNITY_2020_3_3 || UNITY_2020_3_4 || UNITY_2020_3_5 || UNITY_2020_3_6 || UNITY_2020_3_7 || UNITY_2020_3_8 || UNITY_2020_3_9 || UNITY_2020_3_10 || UNITY_2020_3_11 || UNITY_2020_3_12 || UNITY_2020_3_13 || UNITY_2020_3_14 || UNITY_2020_3_15 || UNITY_2021_1
            AssetDatabase.SaveAssets ();
#else
            AssetDatabase.SaveAssetIfDirty (o);
#endif
        }


        static internal bool AddFeatureToRenderer (ScriptableRendererData dt, bool useUndo) {
            if (!dt) return false;
            try {
                if (dt.rendererFeatures.Any (a => a is WaterCausticsEffectFeature)) {
                    // -- すでにある場合 アクティブ化
                    var feature = dt.rendererFeatures.FirstOrDefault (a => a is WaterCausticsEffectFeature) as WaterCausticsEffectFeature;
                    if (!feature.isActive) {
                        if (useUndo) Undo.RegisterCompleteObjectUndo (feature, "Feature Set Active");
                        feature.SetActive (true);
                        EditorUtility.SetDirty (feature);
                        saveAssetSafe (dt);
                    }
                    WaterCausticsEffectFeature.OnAddedByScript ();
                    return true;
                } else {
                    // -- まだ無い場合
                    // ※ TODO URPのバージョンが上がるたび処理方法に変更がないか要確認 10.9, 12.1, 14.0 OK
                    if (!EditorUtility.IsPersistent (dt)) return false;
                    var validateMethod = dt.GetType ().GetMethod ("ValidateRendererFeatures", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (validateMethod == null) return false;
                    var feature = ScriptableRendererFeature.CreateInstance<WaterCausticsEffectFeature> ();
                    feature.name = typeof (WaterCausticsEffect).Name;
                    if (useUndo) {
                        Undo.RegisterCreatedObjectUndo (feature, "Add Renderer Feature");
                        Undo.RegisterCompleteObjectUndo (dt, "Add Renderer Feature");
                    }
                    AssetDatabase.AddObjectToAsset (feature, dt);
                    dt.rendererFeatures.Add (feature);
                    validateMethod.Invoke (dt, null);
                    dt.SetDirty ();
                    EditorUtility.SetDirty (dt);
                    saveAssetSafe (dt);
                    WaterCausticsEffectFeature.OnAddedByScript ();
                    return true;
                }
            } catch { }
            return false;
        }

        static internal bool CheckAllHasActiveFeature (List<ScriptableRendererData> list) {
            return list != null && !list.Any (a => checkHasActiveFeature (a) == false);
        }

        static internal bool checkHasActiveFeature (ScriptableRendererData dt) {
            return dt != null && dt.rendererFeatures.Any (a => a is WaterCausticsEffectFeature && a.isActive);
        }

        static internal bool AddFeatureToAllRenderers (out List<ScriptableRendererData> list, bool useUndo) {
            if (GetAllRendererData (out list))
                return AddFeatureToAllRenderers (list, useUndo);
            return false;
        }

#if WCE_DEVELOPMENT
        [MenuItem ("WCM/RendererFeatureTest/AddFeatureToAllRenderers")]
#endif
        static internal bool AddFeatureToAllRenderers () {
            return AddFeatureToAllRenderers (useUndo: true);

        }
        static internal bool AddFeatureToAllRenderers (bool useUndo) {
            if (GetAllRendererData (out var list))
                return AddFeatureToAllRenderers (list, useUndo);
            return false;
        }

        static internal bool AddFeatureToAllRenderers (List<ScriptableRendererData> list, bool useUndo) {
            if (list == null) return false;
            bool result = true;
            foreach (var dt in list)
                result &= AddFeatureToRenderer (dt, useUndo);
            return result;
        }

        static internal bool GetAllRendererData (out List<ScriptableRendererData> list) {
            list = new List<ScriptableRendererData> ();
            var guids = AssetDatabase.FindAssets ($"t:{typeof(ScriptableRendererData).ToString()}", new [] { "Assets" });
            if (guids.Length == 0) return false;
            foreach (var guid in guids) {
                var dt = AssetDatabase.LoadAssetAtPath<ScriptableRendererData> (AssetDatabase.GUIDToAssetPath (guid));
                list.Add (dt);
            }
            return list.Count != 0;
        }

        static internal void SelectAndPing (List<ScriptableRendererData> list) {
            Selection.objects = list.ToArray ();
            foreach (var dt in list) EditorGUIUtility.PingObject (dt);
        }

        static internal string AssetsToPathStr (List<ScriptableRendererData> list) {
            var str = "";
            for (int i = 0; i < list.Count; i++)
                str += $"{i+1}: {AssetDatabase.GetAssetPath (list [i])}\n";
            return str;
        }
        //------------------------------------------------------------------------ 
    }

}
#endif
