// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if WCE_URP
using System.Collections.Generic;
using UnityEngine;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_GodRayEffect : MonoBehaviour {
        // ------------------------------------------------------ 
        public Light m_SunLight;
        public Transform m_RotateAxis;
        [Range (-1.0f, 1.0f)] public float m_RotateSpeed = 0.04f;
        public Vector2 m_JitterRotation = new Vector2 (0.5f, 0.5f);
        public Material m_Material;
        public List<MeshRenderer> m_Renderers;
        [Space]
        [Min (0f)] public float m_Intensity = 0.1f;
        [Min (0f)] public float m_Depth = 5f;
        public bool m_ReceiveShadow = true;
        [Min (0f)] public float m_SoftParticle = 0.4f;
        [Min (0f)] public float m_SurfaceFadeDepth = 1f;
        [Min (0f)] public float m_EyeDistanceFadeMin = 2f;
        [Min (0f)] public float m_EyeDistanceFadeMax = 3f;

        private void OnValidate () {
            m_EyeDistanceFadeMax = Mathf.Max (m_EyeDistanceFadeMin, m_EyeDistanceFadeMax);
        }

        // ------------------------------------------------------
        private float _rot;

        // ------------------------------------------------------

        void Update () {
            if (m_SunLight)
                transform.rotation = m_SunLight.transform.rotation * Quaternion.Euler (-90f + m_JitterRotation.x, m_JitterRotation.y, 0f);
            if (Application.isPlaying && m_RotateAxis) {
                _rot = (_rot + m_RotateSpeed * Time.deltaTime * 60f + 360f) % 360f;
                m_RotateAxis.localRotation = Quaternion.Euler (0, _rot, 0);
            }
            foreach (var r in m_Renderers)
                if (r) r.forceRenderingOff = (m_Intensity == 0f || !m_SunLight || !m_SunLight.isActiveAndEnabled);
            if (m_Material) {
                m_Material.SetFloat ("_Intensity", m_Intensity);
                m_Material.SetFloat ("_DepthCoef", 1f / Mathf.Max (m_Depth, 0.0000001f));
                m_Material.SetFloat ("_DepthFade", m_SoftParticle);
                m_Material.SetFloat ("_SurfaceAttenCoef", 1f / Mathf.Max (m_SurfaceFadeDepth, 0.0000001f));
                m_Material.SetFloat ("_SightDepthFadeStart", m_EyeDistanceFadeMin);
                m_Material.SetFloat ("_SightDepthFadeRange", m_EyeDistanceFadeMax - m_EyeDistanceFadeMin);
                if (m_ReceiveShadow)
                    m_Material.DisableKeyword ("_RECEIVE_SHADOWS_OFF");
                else
                    m_Material.EnableKeyword ("_RECEIVE_SHADOWS_OFF");
            }
        }
    }
}
#endif
