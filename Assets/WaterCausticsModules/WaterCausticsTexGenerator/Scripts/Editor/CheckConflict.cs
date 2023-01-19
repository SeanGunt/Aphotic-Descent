// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
#pragma warning disable 162

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    スクリプトインポート時チェック
    古いバージョンのファイルがまだある場合にダイアログとWarningを表示
    ※他でスクリプトエラーが発生していると呼ばれないので注意
    -------------------------------------------------------------------------*/
    public class CheckConflict {

        // ファイルのGUID ※TexGenフォルダ内のファイルであること
        static readonly string GUID = "3a4975b64748c9a4db8c082b2e1877eb";
        static readonly string OLD_FILE_NAME = "IconPause.png"; // v2には無い
        static bool conflicting {
            get {
                var path = AssetDatabase.GUIDToAssetPath (GUID);
                // ※削除した後、実際に無くてもパスが返ってくるので注意 File.Exists必須
                return !string.IsNullOrEmpty (path) && path.EndsWith (OLD_FILE_NAME) && File.Exists (path);
            }
        }
        static readonly double INTERVAL = 30d;
        static double s_NestTime;

        [InitializeOnLoadMethod]
        static private void initializeOnLoad () {
            EditorApplication.delayCall += delayFunc;
        }

        static void delayFunc () {
            EditorApplication.update -= editorUpdate;
            if (conflicting) {
                EditorApplication.update += editorUpdate;
                showDialog ();
            }
        }

        static void editorUpdate () {
            if (EditorApplication.timeSinceStartup > s_NestTime) {
                if (conflicting) {
                    showError ();
                    s_NestTime = EditorApplication.timeSinceStartup + INTERVAL;
                } else {
                    EditorApplication.update -= editorUpdate;
                }
            }
        }

#if WCE_DEVELOPMENT
        [MenuItem ("WCM/TestDialog/CheckConflict")]
        static void test () { showDialog (); showError (); }
#endif

        static string title = $"Conflicting Error  ({Constant.ASSET_NAME})";
        static string body = $"Conflicting error with older version.\nPlease re-import after delete the folder.\n\n1, Delete the \"Assets/{Constant.ASSET_FOLDER_NAME}\" folder.\n\n2, Re-import this asset again.\n\n[{Constant.ASSET_NAME}]";
        static void showDialog () {
            EditorUtility.DisplayDialog ($"Conflicting Error ({Constant.ASSET_NAME})", body, "OK");
        }
        static void showError () {
            Debug.LogError ($"{title}  {body.Replace("\n", " ")}\n\n\n");
        }

    }
}
#endif
