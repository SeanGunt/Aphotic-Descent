// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && (!UNITY_2020_3_OR_NEWER || WCE_DEVELOPMENT)
using UnityEditor;
using UnityEngine;
#pragma warning disable 162

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    このアセットがインポートされた際、Unityのバージョンが低い場合にダイアログとWarningを表示
    -------------------------------------------------------------------------*/
    public class VersionCheck {
#if WCE_DEVELOPMENT
        [MenuItem ("WCM/TestDialog/VersionCheck")]
        static void dialogTest () => showDialog ();
        static private readonly bool isDeveloping = true;
#else
        static private readonly bool isDeveloping = false;
#endif

        [InitializeOnLoadMethod]
        static private void registerCallback () {
            AssetDatabase.importPackageCompleted -= importCompleted;
            AssetDatabase.importPackageCompleted += importCompleted;
        }

        static void importCompleted (string packageName) {
            if (isDeveloping) return;
            if (Constant.CheckPackageName (packageName))
                showDialog ();
        }

        static void showDialog () {
            string str0 = $"Requires Unity {Constant.REQUIRE_UNITY_VER}";
            string str1 = $"This asset is not compatible with current Unity version.\n\nCurrent : {Application.unityVersion} \nRequire : {Constant.REQUIRE_UNITY_VER}\n\n\n({Constant.ASSET_NAME})";
            EditorUtility.DisplayDialog (str0, str1, "OK");
            Debug.LogWarning ($"{str1.Replace("\n", " ")}\n\n\n");
        }

    }
}
#endif
