// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if WCE_URP
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MH.WaterCausticsModules {
    /*------------------------------------------------------------------------ 
    This RendererFeature is used to stably apply WaterCausticsEffect to cameras 
    created by other effects.
    For example, a mirror or water reflection effect may use a temporary camera 
    that is not placed in the scene, and the effect cannot be applied stably. 
    In that case, register this RendererFeature to the RendererData Asset. 
    The effect will be applied stably to all cameras.

    このRendererFeatureは、他のエフェクトで作成されたカメラへWaterCausticsEffect
    を安定的に適用するために使用します。
    鏡や水面の反射エフェクトなどではシーンに配置されない一時的なカメラを使用する
    場合があり、安定してエフェクトを適用出来ません。その場合はこの RendererFeature
    を RendererData Asset に登録して下さい。安定してエフェクトが適用されるように
    なります。
    -------------------------------------------------------------------------*/

#if UNITY_2021_2_OR_NEWER
    [DisallowMultipleRendererFeature ("WaterCausticsEffect (Renderer Feature)")]
#elif WCE_URP_10_8
    [DisallowMultipleRendererFeature]
#endif
    [HelpURL (Constant.URL_MANUAL)]
    public class WaterCausticsEffectFeature : ScriptableRendererFeature {
        static private WaterCausticsEffectFeature s_ins;
        static public event Action<Camera> onCamRender;
        static public event Action<ScriptableRenderer, Camera> onEnqueue;
        static private int s_lastFrame;
        static internal bool effective => (s_ins != null && s_ins.isActive && s_lastFrame >= Time.renderedFrameCount - 1);
        static internal void OnAddedByScript () => s_lastFrame = Time.renderedFrameCount;
        public override void Create () { }
        public override void AddRenderPasses (ScriptableRenderer renderer, ref RenderingData rendData) {
            s_ins = this;
            s_lastFrame = Time.renderedFrameCount;
            var cam = rendData.cameraData.camera;
            onCamRender?.Invoke (cam);
            onEnqueue?.Invoke (renderer, cam);
        }
    }
}
#endif
