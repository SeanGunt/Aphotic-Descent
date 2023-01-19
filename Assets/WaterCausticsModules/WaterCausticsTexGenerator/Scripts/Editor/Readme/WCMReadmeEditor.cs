// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MH.WaterCausticsModules {

    [CustomEditor (typeof (WCMReadme))]
    public class WCMReadmeEditor : Editor {
        private bool isDark => EditorGUIUtility.isProSkin;
        private GUIStyle _baseStyle, _titleStyle, _textStyle, _headerStyle, _linkUrlStyle, _linkObjStyle, _warningStyle;
        void prepStyle () {
            if (_baseStyle != null) return;
            _baseStyle = new GUIStyle (EditorStyles.label);
            _baseStyle.wordWrap = true;
            _baseStyle.fontSize = 13;
            _baseStyle.richText = true;

            _titleStyle = new GUIStyle (_baseStyle);
            _titleStyle.fontStyle = FontStyle.Bold;
            _titleStyle.fontSize = 18;

            _textStyle = new GUIStyle (_baseStyle);
            _textStyle.fontSize = 14;

            _headerStyle = new GUIStyle (_baseStyle);
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.fontSize = 16;

            _linkUrlStyle = new GUIStyle (_baseStyle);
            _linkUrlStyle.wordWrap = false;
            _linkUrlStyle.normal.textColor = isDark ? new Color (0f, 0.8f, 1f, 1f) : new Color (0f, 0.4f, 0.8f, 1f);
            _linkUrlStyle.hover.textColor = _linkUrlStyle.normal.textColor + Color.white * (isDark ? 0.3f : 0.2f);
            _linkUrlStyle.stretchWidth = false;

            _linkObjStyle = new GUIStyle (_linkUrlStyle);
            _linkObjStyle.normal.textColor = isDark ? new Color (0f, 0.85f, 0.4f, 1f) : new Color (0f, 0.5f, 0.2f, 1f);
            _linkObjStyle.hover.textColor = _linkObjStyle.normal.textColor + Color.white * (isDark ? 0.3f : 0.2f);

            _warningStyle = new GUIStyle (_baseStyle);
            _warningStyle.normal.textColor = isDark ? Color.white : new Color (0.8f, 0.1f, 0f, 1f);
            _warningStyle.normal.background = Texture2D.whiteTexture;
        }

        protected override void OnHeaderGUI () {
            var icon = (target as WCMReadme).icon;
            if (icon == null) return;
            float iconW = EditorGUIUtility.currentViewWidth;
            float iconH = Mathf.Min (iconW * icon.height / icon.width, 150f);
            iconW = iconH * icon.width / icon.height;
            Rect rect = GUILayoutUtility.GetRect (iconW, iconH);
            rect.width -= 1f;
            GUI.DrawTexture (rect, icon, ScaleMode.ScaleAndCrop);
        }


#if !UNITY_2020_3_OR_NEWER //|| WCE_DEVELOPMENT
        private readonly bool UNITY_VER_OK = false;
#else
        private readonly bool UNITY_VER_OK = true;
#endif

#if !WCM_URP_10 //|| WCE_DEVELOPMENT
        private readonly bool URP_OK = false;
#else
        private readonly bool URP_OK = true;
#endif

        public override void OnInspectorGUI () {
            prepStyle ();

            var tar = target as WCMReadme;
            GUILayout.Space (6);
            GUILayout.Label (tar.title, _titleStyle);
            GUILayout.Space (2);
            GUILayout.Label (Constant.WCE_VERSION_STR, _baseStyle);
            GUILayout.Space (4);

            if (!UNITY_VER_OK) {
                GUILayout.Space (2);
                labelWarning ($"This asset is not compatible with current Unity version {Application.unityVersion}. Unity {Constant.REQUIRE_UNITY_VER} is required.");
                GUILayout.Space (6);
            }

            if (!URP_OK) {
                GUILayout.Space (2);
                GUILayout.Label ($"This asset contains two modules TexGenerator and Effect. The Effect module is available for import in environments using UniversalRP {Constant.REQUIRE_URP_VER}. TexGenerator works with all render pipelines. The generated texture could be used in Light Cookie, Decal, Projector, and Materials.", _textStyle);
                GUILayout.Space (6);
            }

            GUILayout.Space (2);

            foreach (var section in tar.sections) {
                bool used = false;
                if (hasStr (section.heading)) {
                    GUILayout.Space (12);
                    GUILayout.Label (section.heading, _headerStyle);
                    used = true;
                }
                if (hasStr (section.text)) {
                    GUILayout.Space (2);
                    GUILayout.Label (section.text, _baseStyle);
                    used = true;
                }
                if (hasStr (section.linkText) && (hasStr (section.url) || section.obj != null)) {
                    GUILayout.Space (2);
                    bool isObj = (section.obj != null);
                    used = true;
                    if (labelLink (new GUIContent (section.linkText, hasStr (section.linkDesc) ? section.linkDesc : section.url), isObj)) {
                        if (isObj) {
                            AssetDatabase.OpenAsset (section.obj);
                        } else {
                            Application.OpenURL (section.url);
                        }
                    }
                }
                if (used) GUILayout.Space (4);
            }
            GUILayout.Space (80);
        }

        private bool hasStr (string str) => !string.IsNullOrEmpty (str);

        void labelWarning (string text) {
            Color tmp = GUI.backgroundColor;
            GUI.backgroundColor = isDark ? new Color (1f, 0.3f, 0.6f, 0.4f) : new Color (1f, 0.3f, 0.6f, 0.5f);
            GUILayout.Label (text, _warningStyle);
            GUI.backgroundColor = tmp;
        }

        private bool labelLink (GUIContent label, bool isObj = false, params GUILayoutOption [] options) {
            GUIStyle style = isObj ? _linkObjStyle : _linkUrlStyle;
            var rect = GUILayoutUtility.GetRect (label, style, options);
            Handles.BeginGUI ();
            Handles.color = style.normal.textColor;
            Handles.DrawLine (new Vector3 (rect.xMin, rect.yMax), new Vector3 (rect.xMax, rect.yMax));
            Handles.color = Color.white;
            Handles.EndGUI ();
            EditorGUIUtility.AddCursorRect (rect, MouseCursor.Link);
            return GUI.Button (rect, label, style);
        }
    }
}
#endif
