// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using UnityEngine;

namespace MH.WaterCausticsModules {
#if WCE_DEVELOPMENT
    [CreateAssetMenu]
#endif
    public class EffectModuleRef : ScriptableObject {
        // WaterCausticEffectフォルダ
        [SerializeField] private Object m_effectModule; 
        internal Object effectModule => m_effectModule;

        // ShaderFunctionsフォルダ
        [SerializeField] private Object m_customFunc;
        internal Object customFunc => m_customFunc;

        // ForAmplifyShaderEditor.unitypackageファイル
        [SerializeField] private Object m_packageForASE;
        internal Object packageForASE => m_packageForASE;
    }
}
#endif
