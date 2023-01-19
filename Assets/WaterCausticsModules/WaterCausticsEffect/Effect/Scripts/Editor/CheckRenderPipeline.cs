// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && (!WCE_URP || WCE_DEVELOPMENT)
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#pragma warning disable 162

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    URP10.4以上のパッケージを持っていない場合にEffectモジュールを削除。
    initializeOnLoad と インポート時に削除を試す
    -------------------------------------------------------------------------*/
    public class CheckRenderPipeline : AssetPostprocessor {
        static private readonly string classFileName = $"{typeof(CheckRenderPipeline).Name}.cs";

#if WCE_DEVELOPMENT
        [MenuItem ("WCM/TestDialog/DeleteEffectModuleDialog")]
        static void dialogTest () => showDialogAndWarning ();
        static private readonly bool isDeveloping = true;
#else
        static private readonly bool isDeveloping = false;
#endif

        [InitializeOnLoadMethod]
        private static void initializeOnLoad () {
            EditorApplication.delayCall += delayCall;
        }
        private static void delayCall () {
            if (isDeveloping) return;
            if (findAsset<EffectModuleRef> (out var asset, out var path)) {
                if (asset.effectModule) {
                    var folderPath = AssetDatabase.GetAssetPath (asset.effectModule);
                    if (!string.IsNullOrEmpty (folderPath) && folderPath.EndsWith (Constant.EFFECT_FOLDER_NAME)) {
                        deleteEffectFolder (folderPath);
                    }
                }
            }
        }

        private static string s_folderPath;
        static void OnPostprocessAllAssets (string [] imported, string [] deleted, string [] moved, string [] movedFrom) {
            if (isDeveloping) return;
            if (findAsset<EffectModuleRef> (out var asset, out var path) && asset.effectModule) {
                var folderPath = AssetDatabase.GetAssetPath (asset.effectModule);
                if (!string.IsNullOrEmpty (folderPath) && folderPath.EndsWith (Constant.EFFECT_FOLDER_NAME)) {
                    if (imported.Any (a => a.StartsWith (folderPath))) {
                        // Shaderファイルなどを先に削除 ※Shader Error回避
                        deleteWithoutMat (folderPath);
                        // フォルダの削除はDelayCallで行う ※MaterialPostprocessorでのNullエラー回避
                        s_folderPath = folderPath;
                        EditorApplication.delayCall += () => deleteEffectFolder (s_folderPath);
                    } else {
                        deleteEffectFolder (folderPath);
                    }
                }
            }
        }

        static void deleteWithoutMat (string folderPath) {
            string [] guids = AssetDatabase.FindAssets ("", new [] { folderPath });
            string [] paths = guids.Select (guid => AssetDatabase.GUIDToAssetPath (guid)).Where (p => p.EndsWith (".shader") || p.EndsWith (".asset") || p.EndsWith (".lighting")).ToArray ();
#if UNITY_2020_1_OR_NEWER
            AssetDatabase.DeleteAssets (paths, new List<string> ());
#else
            foreach (var p in paths) AssetDatabase.DeleteAsset (p);
#endif
        }

        static void deleteEffectFolder (string folderPath) {
            if (isDeveloping || string.IsNullOrEmpty (folderPath)) return;
            if (AssetDatabase.DeleteAsset (folderPath)) {
                AssetDatabase.Refresh ();
                showDialogAndWarning ();
            }
        }

        static void showDialogAndWarning () {
            // ----- Dialog表示
            EditorUtility.DisplayDialog ("Effect module removed.", $"The Effect module of this asset was removed.\n\nBecause the UniversalRP package {Constant.REQUIRE_URP_VER} could not be detected in this project.\n\nSee the Manual for details.\n\n\n({Constant.ASSET_NAME})", "OK");
        }

        static private bool findAsset<T> (out T asset, out string path) where T : Object {
            asset = null;
            path = null;
            var guids = AssetDatabase.FindAssets ($"t:{typeof (T).ToString()}", new [] { "Assets" }); // ※フォルダ名の最後にスラッシュがあると古いUnityでエラーになるので注意
            if (guids.Length == 0) return false;
            path = AssetDatabase.GUIDToAssetPath (guids [0]);
            asset = AssetDatabase.LoadAssetAtPath<T> (path);
            return asset != null;
        }

    }
}
#endif
