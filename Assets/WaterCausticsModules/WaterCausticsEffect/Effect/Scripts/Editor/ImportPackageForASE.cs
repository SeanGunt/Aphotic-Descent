// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && WCE_URP && AMPLIFY_SHADER_EDITOR
using UnityEditor;
using UnityEngine;
#pragma warning disable 162

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    AmplifyShaderEditorがある環境でアセットがインポートされた際、
    AmplifyShaderEditor用のカスタムファンクションパッケージを自動インポート
    -------------------------------------------------------------------------*/
    public class ImportPackageForASE {
#if WCE_DEVELOPMENT
        static private readonly bool isDeveloping = true;
#else
        static private readonly bool isDeveloping = false;
#endif

        [InitializeOnLoadMethod]
        public static void registerCallback () {
            AssetDatabase.importPackageCompleted -= importCompleted;
            AssetDatabase.importPackageCompleted += importCompleted;
        }

        static void importCompleted (string packageName) {
            if (isDeveloping) return;
            if (Constant.CheckPackageName (packageName) && findAsset<EffectModuleRef> (out var asset, out var path) && asset.packageForASE) {
                string packagePath = AssetDatabase.GetAssetPath (asset.packageForASE);
                if (packagePath != null && packagePath.EndsWith (Constant.ASE_PACKAGE_NAME)) {
                    AssetDatabase.ImportPackage (packagePath, false);
                    AssetDatabase.DeleteAsset (packagePath);
                    AssetDatabase.Refresh ();
                }
            }
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
    }
}
#endif
