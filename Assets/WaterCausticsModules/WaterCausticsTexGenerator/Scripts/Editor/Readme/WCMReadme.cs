// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    Readme
    ※クラス名がReadmeだと他の名前空間のReadmeでもアイコンが変わってしまうので注意
    -------------------------------------------------------------------------*/
    public class WCMReadme : ScriptableObject {
        // ------------------------------------ ScriptableObject
        public Texture2D icon;
        public string title;
        public Section [] sections;

        [System.Serializable]
        public class Section {
            public string heading, text, linkText, linkDesc, url;
            public Object obj;
        }

        // ------------------------------------ AssetDatabase Callback
        // このアセットがインポートされた場合にReadmeを選択
        [InitializeOnLoadMethod]
        static private void registerCallback () {
            AssetDatabase.importPackageCompleted -= importCompleted;
            AssetDatabase.importPackageCompleted += importCompleted;
        }

        static void importCompleted (string packageName) {
            if (Constant.CheckPackageName (packageName))
                EditorApplication.delayCall += selectReadme;
        }

        // TODO ウィンドウ化
        [MenuItem (Constant.README_MENU_ITEM_PATH)]
        static void selectReadme () {
            if (findAsset<WCMReadme> (out var asset, out var path)) {
                showInspectorWindow ();
                EditorGUIUtility.PingObject (asset);
                Selection.activeObject = asset;
            }
        }

        private static void showInspectorWindow () {
            var windowType = typeof (Editor).Assembly.GetType ("UnityEditor.InspectorWindow");
            EditorWindow.GetWindow (windowType);
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

        // -------------------------------------------------------
    }
}
#endif
