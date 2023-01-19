// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_ShaderGlobalTime : MonoBehaviour {
#if WCE_DEVELOPMENT
        public bool m_useSpecifiedTime;
        public float m_specifiedTime;
#endif
        private class pID {
            readonly internal static int _DemoTime = Shader.PropertyToID ("_DemoTime");
        }

        [System.NonSerialized] private double _time;
        private void OnApplicationFocus (bool focusStatus) {
            _time = 0;
        }

        private void OnApplicationPause (bool pauseStatus) {
            _time = 0;
        }

        private void OnEnable () {
            _time = 0;
        }

        private void Update () {
            _time += Time.deltaTime;
            if (_time > 5000f) _time = 0f;
#if WCE_DEVELOPMENT
            if (m_useSpecifiedTime)
                _time = m_specifiedTime * 0.1f;
#endif
            Shader.SetGlobalFloat (pID._DemoTime, (float) _time);
        }
    }
}
