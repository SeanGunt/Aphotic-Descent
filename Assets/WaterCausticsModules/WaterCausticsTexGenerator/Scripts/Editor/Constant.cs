// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using System.Text.RegularExpressions;
#endif

namespace MH.WaterCausticsModules {
    public class Constant {
        public const string URL_MANUAL = "https://hacoapp.com/asset/wce/v2/ManualPDF.pdf";
        public const string ASSET_NAME = "WaterCausticsEffect for URP";
        public const string ASSET_FOLDER_NAME = "WaterCausticsModules";
        public const string EFFECT_FOLDER_NAME = "WaterCausticsEffect";
        public const string ASE_PACKAGE_NAME = "ForAmplifyShaderEditor.unitypackage";
        public const string REQUIRE_UNITY_VER = "2020.3 or later";
        public const string REQUIRE_URP_VER = "10.4 or later";
        public const string SHADER_NAME_HEADER = "Hidden/WaterCausticsModules/";
        public const string URL_HOW_TO_ADD_FEATURE = "https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/urp-renderer-feature-how-to-add.html";
        public const string README_MENU_ITEM_PATH = "Window/WaterCausticsModules/Readme";
        public const int WCE_VERSION_INT = 20001;
        public const string WCE_VERSION_STR = "2.0.1";

#if UNITY_EDITOR
        static public bool CheckPackageName (string packageName) {
            // OK : "Water Caustics Effect for URP"
            // OK : "Water Caustics Effect for URP V2"
            // OK : "WaterCausticsEffect for URP"
            // OK : "WaterCausticsEffectForURP"
            // OK : "WaterCausticsEffectForURP_V2"
            // OK : "Water Caustics Modules"
            // OK : "Water Caustics Module"
            // OK : "PackageWaterCausticsModules_v2"
            // OK : "WaterCausticsTexGen"
            // NO : "WaterEffect"
            packageName = packageName.ToLower ();
            if (new Regex (".*water ?caustics ?effect ?for ?urp.*").IsMatch (packageName))
                return true;
            if (new Regex (".*water ?caustics ?modules?.*").IsMatch (packageName))
                return true;
            if (new Regex (".*water ?caustics ?(tex|texture) ?gen?.*").IsMatch (packageName))
                return true;
            return false;
        }
#endif
    }
}
