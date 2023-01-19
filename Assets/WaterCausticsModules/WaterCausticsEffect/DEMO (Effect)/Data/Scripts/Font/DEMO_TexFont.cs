// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if WCE_URP
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu ("")]
    public class DEMO_TexFont : MonoBehaviour {
        public RenderPassEvent m_RenderEventAboveWater = RenderPassEvent.AfterRenderingSkybox;
        public RenderPassEvent m_RenderEventUnderWater = RenderPassEvent.AfterRenderingTransparents;

        private void OnEnable () {
            WaterCausticsEffectFeature.onEnqueue -= enqueuePass;
            WaterCausticsEffectFeature.onEnqueue += enqueuePass;
        }

        private void OnDisable () {
            WaterCausticsEffectFeature.onEnqueue -= enqueuePass;
        }

        private WCE_Demo_TexFontPass _pass;
        private void enqueuePass (ScriptableRenderer renderer, Camera cam) {
            if (_pass == null) _pass = new WCE_Demo_TexFontPass ();
            var camY = cam.transform.position.y;
            float surfY = Shader.GetGlobalFloat ("_WCECF_SurfaceY");
            var evt = surfY < camY ? m_RenderEventAboveWater : m_RenderEventUnderWater;
            _pass.Setup (evt);
            renderer.EnqueuePass (_pass);
        }

        // ----------------------------------------------------------- ScriptableRenderPass
        internal class WCE_Demo_TexFontPass : ScriptableRenderPass {
            private ShaderTagId _shaderTagId;
            private FilteringSettings _filteringSettings;

            internal WCE_Demo_TexFontPass () {
                base.profilingSampler = new ProfilingSampler (nameof (WCE_Demo_TexFontPass));
                _shaderTagId = new ShaderTagId ("WCE_DEMO_TexFont");
                _filteringSettings = new FilteringSettings (RenderQueueRange.all, layerMask: ~0);
            }

            internal void Setup (RenderPassEvent evt) {
                this.renderPassEvent = evt;
            }

            public override void Execute (ScriptableRenderContext context, ref RenderingData renderingData) {
                DrawingSettings drawingSettings = CreateDrawingSettings (_shaderTagId, ref renderingData, SortingCriteria.None);
                drawingSettings.perObjectData = 0; // ←ライト情報など使用しないので省略

                CommandBuffer cmd = CommandBufferPool.Get ();
                using (new ProfilingScope (cmd, base.profilingSampler)) {
                    context.ExecuteCommandBuffer (cmd);
                    cmd.Clear ();
                    context.DrawRenderers (renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
                }
                context.ExecuteCommandBuffer (cmd);
                CommandBufferPool.Release (cmd);
            }
        }

    }
}
#endif // end of WCE_URP
