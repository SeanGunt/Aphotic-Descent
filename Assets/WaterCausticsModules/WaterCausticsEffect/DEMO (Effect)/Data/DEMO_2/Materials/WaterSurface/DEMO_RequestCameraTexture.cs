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
    public class DEMO_RequestCameraTexture : MonoBehaviour {
        public bool m_OpaqueTexture = true;
        public bool m_DepthTexture = true;

        private void OnEnable () {
            WaterCausticsEffectFeature.onEnqueue -= enqueuePass;
            WaterCausticsEffectFeature.onEnqueue += enqueuePass;
        }

        private void OnDisable () {
            WaterCausticsEffectFeature.onEnqueue -= enqueuePass;
        }

        private RequestTexPass _pass;
        private void enqueuePass (ScriptableRenderer renderer, Camera cam) {
            if (cam.cameraType == CameraType.Preview) return;
            if (_pass == null) _pass = new RequestTexPass ();
            _pass.Setup (m_OpaqueTexture, m_DepthTexture);
            renderer.EnqueuePass (_pass);
        }

        internal class RequestTexPass : ScriptableRenderPass {
            private bool _opaque, _depth;
            internal RequestTexPass () {
                base.profilingSampler = new ProfilingSampler ("RequestTexPass");
                this.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
            }

            internal void Setup (bool opaque, bool depth) {
                _opaque = opaque;
                _depth = depth;
            }

            public override void OnCameraSetup (CommandBuffer cmd, ref RenderingData renderingData) {
                ScriptableRenderPassInput request = 0;
                if (_opaque) request |= ScriptableRenderPassInput.Color;
                if (_depth) request |= ScriptableRenderPassInput.Depth;
                ConfigureInput (request);
            }

            public override void Execute (ScriptableRenderContext context, ref RenderingData renderingData) { }

        }

    }
}
#endif // end of WCE_URP
