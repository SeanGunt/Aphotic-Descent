// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && WCE_URP

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    Previewウィンドウでエフェクトを無効化
    -------------------------------------------------------------------------*/
    public class DisableInPreviewWindow : IPreprocessBuildWithReport, IPostprocessBuildWithReport {
        private static readonly string KEYWORD = "_WCE_DISABLED";
        public int callbackOrder => 1;
        public void OnPreprocessBuild (BuildReport report) {
            // ビルド前処理
            disableEvent ();
        }

        public void OnPostprocessBuild (BuildReport report) {
            // ビルド後処理
            enableEvent ();
        }

        [InitializeOnLoadMethod]
        static void OnInitialize () {
            // Editor起動直後、プレイ開始時
            enableEvent ();
        }

        static private void enableEvent () {
            WaterCausticsEffectFeature.onCamRender -= onEnqueue;
            WaterCausticsEffectFeature.onCamRender += onEnqueue;
        }

        static private void disableEvent () {
            Shader.DisableKeyword (KEYWORD);
            WaterCausticsEffectFeature.onCamRender -= onEnqueue;
        }

        static private void onEnqueue (Camera cam) {
            if (cam.cameraType == CameraType.Preview)
                Shader.EnableKeyword (KEYWORD);
            else
                Shader.DisableKeyword (KEYWORD);
        }

        public class ShaderPreprocessor : IPreprocessShaders {
            // シェーダバリアント削除
            public int callbackOrder => 1;
            public void OnProcessShader (Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data) {
                var deleteKeyword = new ShaderKeyword (DisableInPreviewWindow.KEYWORD);
                for (var i = data.Count - 1; i >= 0; --i)
                    if (data [i].shaderKeywordSet.IsEnabled (deleteKeyword))
                        data.RemoveAt (i);
            }
        }

    }
}
#endif
