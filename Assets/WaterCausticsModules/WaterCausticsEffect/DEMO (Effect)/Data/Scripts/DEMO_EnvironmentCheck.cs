// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;
using UnityEngine.UI;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_EnvironmentCheck : MonoBehaviour {
        public GameObject m_Warning;
        public Text m_Text;

#if !WCE_URP //|| WCE_DEVELOPMENT
        private readonly bool URP_OK = false;
#else
        private readonly bool URP_OK = true;
#endif

        private void OnEnable () {
            if (m_Warning) m_Warning.SetActive (!URP_OK);
            if (m_Text) {
                m_Text.text = URP_OK ? "Warning" : $"The Effect module requires URP package {Constant.REQUIRE_URP_VER}\nand Unity {Constant.REQUIRE_UNITY_VER}.";
            }
        }
    }
}
