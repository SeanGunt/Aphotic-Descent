// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using UnityEngine;

namespace MH.WaterCausticsModules {
#if WCE_DEVELOPMENT
    [CreateAssetMenu]
#endif
    public class TexGenModuleRef : ScriptableObject {
        // WaterCausticsModulesフォルダ
        [SerializeField] private Object m_assetFolder;
        internal Object assetFolder => m_assetFolder;
        
        // WaterCausticsTexGeneratorフォルダ
        [SerializeField] private Object m_texGenModule;
        internal Object texGenModule => m_texGenModule;
    }
}
#endif
