// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;
using UnityEngine.UI;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_VersionCheck : MonoBehaviour {
        public GameObject m_Warning;
        public Text m_Text;

#if !UNITY_2020_3_OR_NEWER //|| WCE_DEVELOPMENT
        private readonly bool UNITY_VER_OK = false;
#else
        private readonly bool UNITY_VER_OK = true;
#endif

        private void OnEnable () {
            if (m_Warning) m_Warning.SetActive (!UNITY_VER_OK);
            if (m_Text) m_Text.text = UNITY_VER_OK ? "Warning" : $"This asset is not compatible with current Unity version.\nCurrent : Unity {Application.unityVersion}\nRequire : Unity {Constant.REQUIRE_UNITY_VER}";
        }
    }
}
