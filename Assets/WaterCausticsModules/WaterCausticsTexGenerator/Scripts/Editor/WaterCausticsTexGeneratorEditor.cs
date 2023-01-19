// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MH.WaterCausticsModules.TexGen {

    [CanEditMultipleObjects]
    [CustomEditor (typeof (WaterCausticsTexGenerator))]
    public class WaterCausticsTexGeneratorEditor : Editor {
#if UNITY_2020_3_OR_NEWER //&& !WCE_DEVELOPMENT
        private SerializedProperty m_generateInEditMode;
        private SerializedProperty m_animateInEditMode;
        private SerializedProperty m_pause;
        private SerializedProperty m_density;
        private SerializedProperty m_height;
        private SerializedProperty m_speed;
        private SerializedProperty m_flow;
        private SerializedProperty m_flowDirection;
        private SerializedProperty m_waves;
        private SerializedProperty m_calcResolution;
        private SerializedProperty m_renderTexture;
        private SerializedProperty m_FillGap;
        private SerializedProperty m_lightRay;
        private SerializedProperty m_lightTransform;
        private SerializedProperty m_lightVector;
        private SerializedProperty m_lightDirection;
        private SerializedProperty m_lightIncidentAngle;
        private SerializedProperty m_style;
        private SerializedProperty m_refractedRay;
        private SerializedProperty m_brightness;
        private SerializedProperty m_gamma;
        private SerializedProperty m_clamp;
        private SerializedProperty m_refractionIndex;
        private SerializedProperty m_useChromaticAberration;
        private SerializedProperty m_chromaticAberration;
        private SerializedProperty m_usePostProcessing;
        private SerializedProperty m_useBlur;
        private SerializedProperty m_blurIterations;
        private SerializedProperty m_blurSpread;
        private SerializedProperty m_msaa;
        private SerializedProperty m_blurDirectional;
        private SerializedProperty m_blurDirection;
        private SerializedProperty m_colorShift;
        private SerializedProperty m_colorShiftDir;
        private SerializedProperty m_useSyncDirection;
        private SerializedProperty m_postContrast;
        private SerializedProperty m_postBrightness;
        private SerializedProperty m_computeShader;
        private SerializedProperty m_shader;

        private void prepProperties () {
            if (m_generateInEditMode != null) return;
            m_generateInEditMode = serializedObject.FindProperty ("m_generateInEditMode");
            m_animateInEditMode = serializedObject.FindProperty ("m_animateInEditMode");
            m_pause = serializedObject.FindProperty ("m_pause");
            m_density = serializedObject.FindProperty ("m_density");
            m_height = serializedObject.FindProperty ("m_height");
            m_speed = serializedObject.FindProperty ("m_speed");
            m_flow = serializedObject.FindProperty ("m_flow");
            m_flowDirection = serializedObject.FindProperty ("m_flowDirection");
            m_waves = serializedObject.FindProperty ("m_waves");
            m_calcResolution = serializedObject.FindProperty ("m_calcResolution");
            m_renderTexture = serializedObject.FindProperty ("m_renderTexture");
            m_FillGap = serializedObject.FindProperty ("m_FillGap");
            m_lightRay = serializedObject.FindProperty ("m_lightRay");
            m_lightTransform = serializedObject.FindProperty ("m_lightTransform");
            m_lightVector = serializedObject.FindProperty ("m_lightVector");
            m_lightDirection = serializedObject.FindProperty ("m_lightDirection");
            m_lightIncidentAngle = serializedObject.FindProperty ("m_lightIncidentAngle");
            m_style = serializedObject.FindProperty ("m_style");
            m_refractedRay = serializedObject.FindProperty ("m_refractedRay");
            m_brightness = serializedObject.FindProperty ("m_brightness");
            m_gamma = serializedObject.FindProperty ("m_gamma");
            m_clamp = serializedObject.FindProperty ("m_clamp");
            m_refractionIndex = serializedObject.FindProperty ("m_refractionIndex");
            m_useChromaticAberration = serializedObject.FindProperty ("m_useChromaticAberration");
            m_chromaticAberration = serializedObject.FindProperty ("m_chromaticAberration");
            m_usePostProcessing = serializedObject.FindProperty ("m_usePostProcessing");
            m_useBlur = serializedObject.FindProperty ("m_useBlur");
            m_blurIterations = serializedObject.FindProperty ("m_blurIterations");
            m_blurSpread = serializedObject.FindProperty ("m_blurSpread");
            m_msaa = serializedObject.FindProperty ("m_msaa");
            m_blurDirectional = serializedObject.FindProperty ("m_blurDirectional");
            m_blurDirection = serializedObject.FindProperty ("m_blurDirection");
            m_colorShift = serializedObject.FindProperty ("m_colorShift");
            m_colorShiftDir = serializedObject.FindProperty ("m_colorShiftDir");
            m_useSyncDirection = serializedObject.FindProperty ("m_useSyncDirection");
            m_postContrast = serializedObject.FindProperty ("m_postContrast");
            m_postBrightness = serializedObject.FindProperty ("m_postBrightness");
            m_computeShader = serializedObject.FindProperty ("m_computeShader");
            m_shader = serializedObject.FindProperty ("m_shader");
        }

        static readonly GUIContent [] _litRayTypeEnumGC = {
            new GUIContent ("Direction"),
            new GUIContent ("Vector"),
            new GUIContent ("Transform"),
            new GUIContent ("Sun"),
            new GUIContent ("Auto"),
        };

        static readonly int [] _litDirTypeEnumOrder = { 4, 0, 1, 2, 3, };

        static readonly string [] _resEnumStr = {
            "64 x 64",
            "96 x 96",
            "128 x 128",
            "160 x 160",
            "256 x 256",
            "320 x 320",
            "512 x 512",
        };

        static readonly string [] _msaaEnumStr = { "None", "2x", "4x", "8x" };

        private enum EnumMSAA {
            NoMSAA = 1,
            MSAA2x = 2,
            MSAA4x = 4,
            MSAA8x = 8
        }

        private readonly string _animInEditModeDesc = "Animate the wave even in edit mode, and force update scene and game view.\n\nTo use it, AdvancedSettings / In EditMode / Generate needs to be On.";

        // ----------------------------------------------------------- 
        private void OnEnable () {
            foreach (var tar in targets.OfType<WaterCausticsTexGenerator> ()) {
                tar.VersionCheck ();
                tar.CheckRenderTex ();
            }
            PreviewWindow.SetTargetIfShowing (target as WaterCausticsTexGenerator);
        }

        private void OnDisable () {
            PreviewWindow.RemoveTarget (target as WaterCausticsTexGenerator);
        }

        public override void OnInspectorGUI () {
            prepProperties ();
            prepWaveList (); 
            serializedObject.Update ();
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                drawProperties ();
                serializedObject.ApplyModifiedProperties ();
                if (check.changed)
                    PreviewWindow.InspectorChanged (target as WaterCausticsTexGenerator);
            }
        }

        // ----------------------------------------------------------- Draw
        readonly Color colorPinkBar = new Color (1f, 0.3f, 0.6f, 0.3f);
        readonly Color colorPinkContent = new Color (1f, 0.3f, 0.6f, 1f);
        readonly Color colorGreenButton = new Color (0.0f, 1f, 0.66f, 1f);
        readonly float SPACE_SUB_TOP_5 = 5f;
        readonly float SPACE_SUB_BTM_12 = 12f;
        readonly float SPACE_MAIN_TOP_7 = 7f;
        readonly float SPACE_MAIN_BTM_5 = 5f;

        private void setLabelAreaWidth (float labelWidthMin, float valWidthMin) {
            if (EditorGUIUtility.labelWidth < labelWidthMin)
                EditorGUIUtility.labelWidth = labelWidthMin;
            if (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth < valWidthMin)
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - valWidthMin;
        }

        private void drawProperties () {
            storeIndentWidth ();
            EditorGUIUtility.labelWidth += 6;
            EditorGUI.indentLevel++;
            setLabelAreaWidth (labelWidthMin: 145f, valWidthMin: 170f);

            bool useTextureWarning = (m_renderTexture.objectReferenceValue == null && isGameObjectOnScene () && !serializedObject.isEditingMultipleObjects);
            // ---------------------------------------------------------------------------------- Calculation
            bool expandCalc = expandMainGroup (m_calcResolution, true, "Calculation", isPink : useTextureWarning);
            // --------- Preview Window Button
            if (!expandCalc) {
                var rect = GUILayoutUtility.GetLastRect ();
                float width = 150f;
                rect.x += rect.width - width;
                rect.width = width;
                rect.y += 1f;
                if (PreviewWindow.IsShowing ()) {
                    if (GUI.Button (rect, "Close Preview", EditorStyles.miniButton)) {
                        PreviewWindow.CloseWindow ();
                    }
                } else {
                    bool useBtn = !serializedObject.isEditingMultipleObjects && !useTextureWarning;
                    using (new DisableScope (useBtn)) {
                        using (new ColorScope (useBtn?colorGreenButton : GUI.color))
                        if (GUI.Button (rect, "Open Preview", EditorStyles.miniButton)) {
                            PreviewWindow.OpenWindow (target as WaterCausticsTexGenerator);
                        }
                    }
                }
            } else {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);
                // --------- RenderTexture
                using (new ColorScope (useTextureWarning ? colorPinkContent : GUI.color)) {
                    EditorGUILayout.PropertyField (m_renderTexture, new GUIContent ("Render Texture", "Output destination RenderTexture."));
                    if (!m_renderTexture.hasMultipleDifferentValues) {
                        if (useTextureWarning) {
                            EditorGUILayout.BeginHorizontal ();
                            GUILayout.FlexibleSpace ();
                            var descBtn = new GUIContent ("Create RenderTexture", "Create a new RenderTexture asset. It will be created in the same folder as the scene file.");
                            if (GUILayout.Button (descBtn, EditorStyles.miniButton, GUILayout.Width (150))) {
                                m_renderTexture.objectReferenceValue = WaterCausticsTexGeneratorMenuItem.CreateRTAsset ();
                            }
                            EditorGUILayout.EndHorizontal ();
                        } else {
                            if (m_renderTexture.objectReferenceValue != null) {
                                var rt = m_renderTexture.objectReferenceValue as RenderTexture;
                                string mipmapStr = rt.useMipMap ? "MipMapOn" : "MipMapOff";
                                string str = $"{rt.width}x{rt.height} / {rt.graphicsFormat} / {(EnumMSAA)rt.antiAliasing} / {mipmapStr}";
                                EditorGUILayout.HelpBox (str, MessageType.None);
                                if (rt.antiAliasing > 1 && rt.useMipMap) {
                                    EditorGUILayout.Space (1);
                                    str = "MipMap is not applied because RenderTexture's Anti-aliasing (MSAA) is On. To use MipMap and MSAA together, turn off MSAA of RenderTexture and use MSAA in PostProcessing of this component.";
                                    EditorGUILayout.HelpBox (str, MessageType.Warning);
                                    EditorGUILayout.BeginHorizontal ();
                                    GUILayout.FlexibleSpace ();
                                    if (GUILayout.Button (new GUIContent ("Fix", "Turn off MSAA of RenderTexture and use MSAA in PostProcessing of this component."), EditorStyles.miniButton, GUILayout.Width (150))) {
                                        Undo.RecordObject (rt, "Turn off MSAA of RenderTexture and use MSAA in PostProcessing of this component.");
                                        EditorUtility.SetDirty (rt);
                                        int tmp = rt.antiAliasing;
                                        if (rt.IsCreated ()) {
                                            bool isActive = (RenderTexture.active == rt);
                                            if (isActive) RenderTexture.active = null;
                                            rt.Release ();
                                            rt.antiAliasing = 1;
                                            rt.Create ();
                                            if (isActive) RenderTexture.active = rt;
                                        } else {
                                            rt.antiAliasing = 1;
                                        }
                                        m_usePostProcessing.boolValue = true;
                                        m_msaa.intValue = Mathf.Max (tmp, m_msaa.intValue);
                                    }
                                    EditorGUILayout.EndHorizontal ();
                                    EditorGUILayout.Space (4);
                                }
                            }
                        }
                    }
                }
                EditorGUILayout.Space (1);
                popup (m_calcResolution, _resEnumStr, "Calc Resolution", "Resolution used for internal calculations.");
                EditorGUILayout.PropertyField (m_FillGap, new GUIContent ("Fill Gap", "If the edges of the output image are not drawn, increase the value. Make it as small as possible to reduce the load."));
                using (new DisableScope (m_generateInEditMode.boolValue)) {
                    EditorGUILayout.PropertyField (m_animateInEditMode, new GUIContent ("Animate In EditMode", _animInEditModeDesc));
                }

                // ---------------------------------------------------------------------------------- Preview Window Button
                EditorGUILayout.Space (5);
                EditorGUILayout.BeginHorizontal ();
                GUILayout.FlexibleSpace ();
                if (PreviewWindow.IsShowing ()) {
                    if (GUILayout.Button ("Close Preview", EditorStyles.miniButton, GUILayout.Width (150))) {
                        PreviewWindow.CloseWindow ();
                    }
                } else {
                    bool useBtn = !serializedObject.isEditingMultipleObjects && !useTextureWarning;
                    using (new DisableScope (useBtn)) {
                        using (new ColorScope (useBtn?colorGreenButton : GUI.color))
                        if (GUILayout.Button ("Open Preview", EditorStyles.miniButton, GUILayout.Width (150))) {
                            PreviewWindow.OpenWindow (target as WaterCausticsTexGenerator);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal ();


                EditorGUILayout.Space (SPACE_SUB_BTM_12 - SPACE_MAIN_BTM_5);
            }

            EditorGUILayout.Space (SPACE_MAIN_BTM_5);


            // ---------------------------------------------------------------------------------- Parameters
            if (expandMainGroup (m_density, true, "Parameters")) {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);
                // ----------------------------------------- Wave
                if (expandSubGroup (m_pause, true, "Wave")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);
                    EditorGUILayout.PropertyField (m_pause, new GUIContent ("Pause", "Suspend the overall waves."));
                    EditorGUILayout.PropertyField (m_density, new GUIContent ("Density", "Adjust the overall density."));
                    EditorGUILayout.PropertyField (m_height, new GUIContent ("Height", "Adjust the overall height."));

                    using (new DisableScope (!m_pause.boolValue)) {
                        EditorGUILayout.PropertyField (m_speed, new GUIContent ("Speed", "Adjust the overall speed."));
                    }
                    EditorGUILayout.Space (1);
                    using (new DisableScope (!m_pause.boolValue)) {
                        EditorGUILayout.PropertyField (m_flow, new GUIContent ("Flow", "Adjust the overall flow."));
                        if (m_flow.floatValue != 0f) {
                            using (new IndentScope (-2f, 0f, 2)) {
                                EditorGUILayout.PropertyField (m_flowDirection, new GUIContent ("Direction", "Adjust the overall direction of flow."));
                                drawDirMark (m_flowDirection, !m_pause.boolValue);
                            }
                        }
                    }
                    EditorGUILayout.Space (8);
                    drawLine ();
                    EditorGUILayout.Space (8);
                    if (isExpand (m_waves, false, new GUIContent ("Waves", $"Wave settings. Supports up to {WaterCausticsTexGenerator.WAVE_MAX_CNT}."))) {
                        EditorGUILayout.Space (3);
                        drawWaveList ();
                    } else {
                        EditorGUILayout.Space (5);
                    }
                    EditorGUILayout.Space (SPACE_SUB_BTM_12);
                }
                // ----------------------------------------- Refraction
                if (expandSubGroup (m_refractionIndex, true, "Refraction")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);

                    EditorGUILayout.PropertyField (m_style, new GUIContent ("Style", "How to calculate light focusing."));
                    EditorGUILayout.PropertyField (m_refractionIndex, new GUIContent ("Refraction Index", "Index of refraction."));

                    EditorGUILayout.Space (1);
                    if (isExpand (m_lightRay, false, new GUIContent ("Details"))) {
                        EditorGUILayout.PropertyField (m_useChromaticAberration, new GUIContent ("Chromatic Aberration", "Simulate more realistic chromatic aberrations. \nAmount of calculation increases. For mobile devices, consider using color shift.\n\nRequires RenderTexture having RGB channels."));
                        if (m_useChromaticAberration.boolValue) {
                            using (new IndentScope (-2, 2)) {
                                EditorGUILayout.PropertyField (m_chromaticAberration, new GUIContent ("Intensity", "Shifts the refractive index in the RGB channels."));
                            }
                        }
                        EditorGUILayout.Space (1);
                        orderedPopup (m_lightRay, _litRayTypeEnumGC, _litDirTypeEnumOrder, "Light Ray", "Direction of the ray from the Light.");
                        using (new IndentScope (0, 1)) {
                            switch ((LightRay) m_lightRay.enumValueIndex) {
                                case LightRay.Vector:
                                    EditorGUILayout.PropertyField (m_lightVector, new GUIContent ("Direction", "Specify the direction of light rays as a vector. It will be normalized."));
                                    break;
                                case LightRay.Transform:
                                    EditorGUILayout.PropertyField (m_lightTransform, new GUIContent ("Transform", "Transforms the light to be referenced."));
                                    break;
                                case LightRay.LitSettingSun:
                                    EditorGUILayout.LabelField ("Use the sun setting in the Light Settings window.", new GUIStyle ("HelpBox"));
                                    break;
                                case LightRay.Auto:
                                    EditorGUILayout.LabelField ("Use the shader's global variable \"_LightDirection\".", new GUIStyle ("HelpBox"));
                                    break;
                                case LightRay.Direction:
                                default:
                                    EditorGUILayout.PropertyField (m_lightIncidentAngle, new GUIContent ("Incident Angle", "Angle of incidence of light rays."));
                                    if (m_lightIncidentAngle.floatValue > 0f) {
                                        drawSyncDirProp (m_lightDirection, true, new GUIContent ("Direction", "Direction of light rays. \n\nChain mark is On, the Light Ray, Directional Blur, and Color Shift directions are synchronized."));
                                    }
                                    break;
                            }
                        }
                        EditorGUILayout.Space (1);
                        EditorGUILayout.PropertyField (m_refractedRay, new GUIContent ("Refracted Ray", "Normalize the rays after refraction or extend them until they hit the bottom."));
                    }
                    EditorGUILayout.Space (SPACE_SUB_BTM_12);
                }
                // ----------------------------------------- Adjustment
                if (expandSubGroup (m_brightness, true, "Adjustment")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);
                    EditorGUILayout.PropertyField (m_gamma, new GUIContent ("Gamma", "Adjusts the contrast."));
                    EditorGUILayout.PropertyField (m_brightness, new GUIContent ("Brightness", "Adjust the brightness."));
                    EditorGUILayout.PropertyField (m_clamp, new GUIContent ("Clamp", "Limit the brightness to this value."));
                }
                EditorGUILayout.Space (SPACE_SUB_BTM_12 - SPACE_MAIN_BTM_5);
            }
            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            // ---------------------------------------------------------------------------------- Post Processing
            if (expandMainGroup (m_usePostProcessing, true, "Post Processing")) {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);

                EditorGUILayout.PropertyField (m_usePostProcessing, new GUIContent ("Post Processing", "Apply the post processing effect. \nWhen post-processing is used, it is first drawn to a temporary RenderTexture, then post-processed, and output to a destination RenderTexture."));
                using (new DisableScope (m_usePostProcessing.boolValue)) {
                    EditorGUILayout.Space (1);
                    popup (m_msaa, _msaaEnumStr, "MSAA", "Multi-Sampling Anti-Alias. \nWhen post-processing is used, it is first drawn to a temporary RenderTexture, then post-processed, and output to a destination RenderTexture. This setting is MSAA for that first drawing. \n\nIf using Post Processing, MSAA of the RenderTexture for output is not used and will be disabled on the actual device.");
                    EditorGUILayout.Space (1);
                    EditorGUILayout.PropertyField (m_useBlur, new GUIContent ("Blur", "Use blur or not."));
                    if (isExpandMarkOnly (m_useBlur, true)) {
                        using (new IndentScope (-2f, 1f)) {
                            using (new DisableScope (m_useBlur.boolValue)) {
                                EditorGUILayout.PropertyField (m_blurIterations, new GUIContent ("Blur Iteration", "Number of iterations."));
                                EditorGUILayout.PropertyField (m_blurSpread, new GUIContent ("Spread", "Amount of spread."));
                                using (new DisableScope (m_blurIterations.intValue > 1 || m_blurSpread.floatValue > 0f)) {
                                    EditorGUILayout.PropertyField (m_blurDirectional, new GUIContent ("Directional", "Give directionality to the blur."));
                                    if (m_blurDirectional.floatValue > 0f) {
                                        bool isActive = m_usePostProcessing.boolValue && m_useBlur.boolValue && (m_blurIterations.intValue > 1 || m_blurSpread.floatValue > 0f);
                                        drawSyncDirProp (m_blurDirection, isActive, new GUIContent ("Direction", "Blur direction.\n\nChain mark is On, the Light Ray, Directional Blur, and Color Shift directions are synchronized."));
                                    }
                                }
                            }
                        }
                    }
                    EditorGUILayout.Space (1);
                    EditorGUILayout.PropertyField (m_colorShift, new GUIContent ("Color Shift", "Amount of color shift.\n\nRequires RenderTexture having RGB channels."));
                    if (m_colorShift.floatValue > 0f) {
                        drawSyncDirProp (m_colorShiftDir, m_usePostProcessing.boolValue, new GUIContent ("Direction", "Color Shift direction.\n\nChain mark is On, the Light Ray, Directional Blur, and Color Shift directions are synchronized."));
                    }
                    EditorGUILayout.Space (1);
                    EditorGUILayout.PropertyField (m_postContrast, new GUIContent ("Contrast", "Contrast adjustment."));
                    EditorGUILayout.PropertyField (m_postBrightness, new GUIContent ("Brightness", "Brightness adjustment in post-processing."));
                }
                EditorGUILayout.Space (SPACE_SUB_BTM_12 - SPACE_MAIN_BTM_5);
            }
            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            // ---------------------------------------------------------------------------------- Advanced Settings
            if (expandMainGroup (m_computeShader, false, "Advanced Settings")) {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);
                if (expandSubGroup (m_generateInEditMode, true, "In EditMode")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);
                    EditorGUILayout.PropertyField (m_generateInEditMode, new GUIContent ("Generate", "Whether to generate while in edit mode."));
                    using (new DisableScope (m_generateInEditMode.boolValue)) {
                        EditorGUILayout.PropertyField (m_animateInEditMode, new GUIContent ("Animate", _animInEditModeDesc));
                    }
                }
            }
            EditorGUILayout.Space (SPACE_SUB_BTM_12);
            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            EditorGUI.indentLevel--;
        }


        // ----------------------------------------------------------- Wave List
        private ReorderableList _waveList;
        private float lineH = EditorGUIUtility.singleLineHeight + 2;
        private void prepWaveList () {
            if (_waveList != null) return;
            _waveList = new ReorderableList (serializedObject, m_waves, true, false, true, true);
            _waveList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                rect.x += 12f;
                rect.width -= 12f;
                rect.y += 2f;
                using (new AdjustLabelSpaceWidthScope (-48f)) {
                    rect.height = EditorGUIUtility.singleLineHeight;

                    var element = m_waves.GetArrayElementAtIndex (index);
                    element.isExpanded = EditorGUI.Foldout (rect, element.isExpanded, new GUIContent ($" ", ""));
                    var m_active = element.FindPropertyRelative ("m_active");
                    EditorGUI.PropertyField (rect, m_active, new GUIContent ($"Wave [{index}]", "Enables/Disables this wave."));
                    if (element.isExpanded) {
                        var m_density = element.FindPropertyRelative ("m_density");
                        var m_height = element.FindPropertyRelative ("m_height");
                        var m_fluctuation = element.FindPropertyRelative ("m_fluctuation");
                        var m_flow = element.FindPropertyRelative ("m_flow");
                        var m_direction = element.FindPropertyRelative ("m_direction");
                        var indent2 = 10f;
                        using (new AdjustLabelSpaceWidthScope (-indent2)) {
                            rect.x += indent2;
                            rect.width -= indent2;
                            rect.y += 2;
                            using (new DisableScope (m_active.boolValue)) {
                                rect.y += lineH;
                                EditorGUI.PropertyField (rect, m_density, new GUIContent ($"Density", "Density of this wave."));
                                rect.y += lineH;
                                EditorGUI.PropertyField (rect, m_height, new GUIContent ($"Height", "Height of this wave."));
                            }
                            bool isEnable = m_active.boolValue && !m_pause.boolValue && !this.m_pause.boolValue;
                            using (new DisableScope (isEnable)) {
                                rect.y += lineH;
                                EditorGUI.PropertyField (rect, m_fluctuation, new GUIContent ($"Fluctuation", "Fluctuation of this wave."));
                                rect.y += lineH + 2;
                                EditorGUI.PropertyField (rect, m_flow, new GUIContent ($"Flow", "Amount of flow."));
                                bool showFlowDir = (m_flow.floatValue != 0f || m_flow.hasMultipleDifferentValues);
                                if (showFlowDir) {
                                    rect.y += lineH;
                                    rect.x += _indentWidth;
                                    rect.width -= _indentWidth;
                                    drawDirMark (rect, m_direction, isEnable);
                                    rect.x += _indentWidth;
                                    rect.width -= _indentWidth;
                                    using (new AdjustLabelSpaceWidthScope (-_indentWidth * 2f)) {
                                        EditorGUI.PropertyField (rect, m_direction, new GUIContent ($"Direction", "Flow Direction. \n\n       -90:Up \n180:Left   0:Right \n       90:Down"));
                                    }
                                }
                            }
                        }
                    }
                }

            };
            _waveList.drawElementBackgroundCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                ReorderableList.defaultBehaviours.DrawElementBackground (rect, index, isActive && isFocused, isFocused, true);
            };
            _waveList.elementHeightCallback = (int index) => {
                var element = m_waves.GetArrayElementAtIndex (index);
                var height = EditorGUI.GetPropertyHeight (m_waves.GetArrayElementAtIndex (index)) + 4f;
                if (element.isExpanded) height += -lineH + 10;
                var m_flow = element.FindPropertyRelative ("m_flow");
                bool showFlowDir = (m_flow.floatValue != 0f || m_flow.hasMultipleDifferentValues);
                if (!showFlowDir) height += -lineH;
                return height;
            };
        }

        private void drawWaveList () {
            var rect = GUILayoutUtility.GetRect (0f, _waveList.GetHeight ());
            rect = EditorGUI.IndentedRect (rect);
            _waveList.displayAdd = _waveList.count < WaterCausticsTexGenerator.WAVE_MAX_CNT;
            _waveList.displayRemove = _waveList.count > 1;
            _waveList.DoList (rect);
        }


        // ----------------------------------------------------------- Parts
        private float _indentWidth;
        private void storeIndentWidth () {
            if (_indentWidth != 0f) return;
            var x0 = EditorGUI.IndentedRect (Rect.zero).x;
            EditorGUI.indentLevel++;
            _indentWidth = EditorGUI.IndentedRect (Rect.zero).x - x0;
            EditorGUI.indentLevel--;
        }

        static private Vector3 [] syncIconPts = {
            new Vector3 (-0.18f, -0.135f),
            new Vector3 (-0.29f, -0.135f),
            new Vector3 (-0.332426f, -0.117426f),
            new Vector3 (-0.35f, -0.075f),
            new Vector3 (-0.35f, 0.075f),
            new Vector3 (-0.332426f, 0.117426f),
            new Vector3 (-0.29f, 0.135f),
            new Vector3 (0.02f, 0.135f),
            new Vector3 (0.0624264f, 0.117426f),
            new Vector3 (0.08f, 0.075f),
            new Vector3 (0.08f, -0.01f),
        };
        static private Vector3 [] asyncIconPts = {
            new Vector3 (-0.07f, -0.135f),
            new Vector3 (-0.29f, -0.135f),
            new Vector3 (-0.332426f, -0.117426f),
            new Vector3 (-0.35f, -0.075f),
            new Vector3 (-0.35f, 0.075f),
            new Vector3 (-0.332426f, 0.117426f),
            new Vector3 (-0.29f, 0.135f),
            new Vector3 (-0.07f, 0.135f),
        };

        private void drawSyncDirProp (SerializedProperty prop, bool isActive, GUIContent label) {
            using (new IndentScope (-2f, 0f, 2)) {
                // ----- Sync Icon
                {
                    var rect = GUILayoutUtility.GetRect (0, 0);
                    rect.height = lineH;
                    rect.x += EditorGUIUtility.labelWidth - 15f;
                    rect.y += 2f;
                    rect.width = 15f;
                    bool clicked = isActive && Event.current != null && Event.current.type == EventType.MouseDown && Event.current.button == 0 && rect.Contains (Event.current.mousePosition);
                    if (clicked) {
                        bool newVal = m_useSyncDirection.hasMultipleDifferentValues ? true : !m_useSyncDirection.boolValue;
                        m_useSyncDirection.boolValue = newVal;
                        if (newVal) m_lightDirection.floatValue = m_blurDirection.floatValue = m_colorShiftDir.floatValue = prop.floatValue;
                    }
                    GUI.BeginClip (rect, new Vector2 (rect.width * 0.5f, rect.height * 0.5f), Vector2.zero, false);
                    if (!m_useSyncDirection.hasMultipleDifferentValues) {
                        float alpha = (isActive? 0.8f : 0.4f) * (m_useSyncDirection.boolValue? 1f : 0.4f);
                        drawSyncIcon (m_useSyncDirection.boolValue, 15f, 0f, alpha);
                    } else {
                        float alpha = (isActive? 0.8f : 0.4f) * 0.7f;
                        drawSyncIcon (true, 15f, -2f, alpha);
                        alpha *= 0.6f;
                        drawSyncIcon (false, 15f, 2f, alpha);
                    }
                    GUI.EndClip ();
                    void drawSyncIcon (bool sync, float size, float shiftX, float alpha) {
                        var pts = sync? syncIconPts : asyncIconPts;
                        var tmpMatrix = Handles.matrix;
                        Handles.color = colorMulAlpha (EditorStyles.label.normal.textColor, alpha);
                        Handles.matrix = tmpMatrix * Matrix4x4.TRS (Vector3.right * shiftX, Quaternion.Euler (0f, 0f, -45f), Vector3.one * size);
                        Handles.DrawAAPolyLine (Texture2D.whiteTexture, 1.1f, pts);
                        Handles.matrix *= Matrix4x4.Rotate (Quaternion.Euler (0f, 0f, 180f));
                        Handles.DrawAAPolyLine (Texture2D.whiteTexture, 1.1f, pts);
                        Handles.matrix = tmpMatrix;
                    }
                }

                // ----- DirMark & Slider
                using (var check = new EditorGUI.ChangeCheckScope ()) {
                    EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                    float newDir = EditorGUILayout.Slider (label, prop.floatValue, -180f, 180f);
                    drawDirMark (prop, isActive);
                    if (check.changed) {
                        for (int i = 0; i < serializedObject.targetObjects.Length; i++) {
                            var so = new SerializedObject (serializedObject.targetObjects [i]);
                            var useSyncProp = so.FindProperty (m_useSyncDirection.name);
                            if (useSyncProp.boolValue) {
                                so.FindProperty (m_lightDirection.name).floatValue = newDir;
                                so.FindProperty (m_blurDirection.name).floatValue = newDir;
                                so.FindProperty (m_colorShiftDir.name).floatValue = newDir;
                            } else {
                                so.FindProperty (prop.name).floatValue = newDir;
                            }
                            so.ApplyModifiedProperties ();
                        }
                        serializedObject.SetIsDifferentCacheDirty ();
                    }
                    EditorGUI.showMixedValue = false;
                }
            }
        }

        static private Color colorMulAlpha (Color c, float mulAlpha) => new Color (c.r, c.g, c.b, c.a * mulAlpha);

        private void drawDirMark (SerializedProperty prop, bool isActive = true) {
            EditorGUI.indentLevel--;
            var rect = EditorGUI.IndentedRect (GUILayoutUtility.GetLastRect ());
            drawDirMark (rect, prop, isActive);
            EditorGUI.indentLevel++;
        }
        private void drawDirMark (Rect rect, SerializedProperty prop, bool isActive = true) {
            rect.width = EditorGUIUtility.labelWidth;
            Vector2 origin = new Vector2 (CIRCLE_R + 1, rect.height * 0.5f);
            float dir = prop.floatValue;
            Handles.color = colorMulAlpha (EditorStyles.label.normal.textColor, isActive ? 0.8f : 0.4f);
            var tmpMatrix = Handles.matrix;
            GUI.BeginClip (rect, origin, Vector2.zero, false);
            Handles.matrix = tmpMatrix * Matrix4x4.Scale (Vector3.one * CIRCLE_R);
            Handles.DrawAAPolyLine (Texture2D.whiteTexture, 1, circlePts);
            if (!prop.hasMultipleDifferentValues) {
                Handles.matrix = tmpMatrix * Matrix4x4.Rotate (Quaternion.Euler (0f, 0f, dir)) * Matrix4x4.Scale (Vector3.one * (CIRCLE_R - 0.5f));
                Handles.DrawAAConvexPolygon (arrowAry);
            }
            Handles.matrix = tmpMatrix;
            GUI.EndClip ();
        }

        static private Vector2 dirToVec (float dir) => new Vector2 (Mathf.Sin (dir * Mathf.Deg2Rad), -Mathf.Cos (dir * Mathf.Deg2Rad));
        const float CIRCLE_R = 5f;
        static readonly private Vector3 [] arrowAry = {
            dirToVec (0),
            dirToVec (150f),
            dirToVec (170f),
            dirToVec (-170f),
            dirToVec (-150f),
        };
        static readonly private Vector3 [] circlePts = {
            new Vector3 (-1f, 0f),
            new Vector3 (-0.87f, -0.5f),
            new Vector3 (-0.5f, -0.87f),
            new Vector3 (0f, -1f),
            new Vector3 (0.5f, -0.87f),
            new Vector3 (0.87f, -0.5f),
            new Vector3 (1f, 0f),
            new Vector3 (0.87f, 0.5f),
            new Vector3 (0.5f, 0.87f),
            new Vector3 (0f, 1f),
            new Vector3 (-0.5f, 0.87f),
            new Vector3 (-0.87f, 0.5f),
            new Vector3 (-1f, 0f),
        };

        bool isGameObjectOnScene () {
            return (target as Component).gameObject.scene.IsValid ();
        }

        private void drawRectMain (bool isPink = false) {
            Color color = isPink ? colorPinkBar : new Color (0f, 0f, 0f, 0.2f);
            Rect rect = GUILayoutUtility.GetRect (0, 0);
            rect.height = lineH;
            rect.x -= _indentWidth + 4;
            rect.width += _indentWidth + 8;
            EditorGUI.DrawRect (rect, color);
        }

        private bool expandMainGroup (SerializedProperty prop, bool defOpen, string label, bool isPink = false) {
            EditorGUI.indentLevel--;
            drawRectMain (isPink);
            bool expand = isExpand (prop, true, new GUIContent (label));
            EditorGUI.indentLevel++;
            return expand;
        }


        private bool expandSubGroup (SerializedProperty prop, bool defOpen, string label, bool isPink = false) {
            Color color = isPink ? colorPinkBar : colorMulAlpha (EditorStyles.label.normal.textColor, 0.1f);
            GUILayout.Label (" ");
            Rect rect = GUILayoutUtility.GetLastRect ();
            rect.y += 1f;
            rect.x += 3f;
            rect.width -= 3f;
            Rect rect2 = rect;
            rect2.x -= 14f;
            rect2.width += 15f;
            EditorGUI.DrawRect (rect2, color);
            GUI.Label (rect, label);
            if (prop == null) {
                return true;
            } else {
                rect.x -= 14;
                prop.isExpanded = EditorGUI.Foldout (rect, prop.isExpanded != defOpen, " ") != defOpen;
                return prop.isExpanded != defOpen;
            }
        }


        private void popup (SerializedProperty prop, string [] enumStr, string text, string tooltip) {
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                var newVal = EditorGUILayout.Popup (new GUIContent (text, tooltip), prop.enumValueIndex, enumStr);
                if (check.changed)
                    prop.enumValueIndex = newVal;
                EditorGUI.showMixedValue = false;
            }
        }

        private void orderedPopup (SerializedProperty prop, GUIContent [] enumStr, int [] intAry, string text, string tooltip) {
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                var newVal = EditorGUILayout.IntPopup (new GUIContent (text, tooltip), prop.enumValueIndex, enumStr, intAry);
                if (check.changed)
                    prop.enumValueIndex = newVal;
                EditorGUI.showMixedValue = false;
            }
        }

        private bool isExpand (SerializedProperty prop, bool defOpen, GUIContent label) {
            prop.isExpanded = EditorGUILayout.Foldout (prop.isExpanded != defOpen, label) != defOpen;
            return prop.isExpanded != defOpen;
        }

        private bool isExpandMarkOnly (SerializedProperty prop, bool defOpen, float adjustX = 0f, float adjustY = 0f) {
            var rect = GUILayoutUtility.GetLastRect ();
            prop.isExpanded = EditorGUI.Foldout (rect, prop.isExpanded != defOpen, " ") != defOpen;
            return prop.isExpanded != defOpen;
        }

        private void drawLine () {
            Rect r = GUILayoutUtility.GetRect (0, 0);
            Color col = colorMulAlpha (EditorStyles.label.normal.textColor, 0.09f);
            EditorGUI.DrawRect (new Rect (r.x + 14f, r.y, r.width - 14f, 1f), col);
        }

        // ----------------------------------------------------------- Scope

        private class AdjustLabelSpaceWidthScope : GUI.Scope {
            private readonly float _store;
            internal AdjustLabelSpaceWidthScope (float adjust) {
                _store = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth += adjust;
            }
            protected override void CloseScope () {
                EditorGUIUtility.labelWidth = _store;
            }
        }

        private class LabelAndIndentScope : GUI.Scope {
            private float _spaceBtm;
            internal LabelAndIndentScope (string label, float spaceTop = 0f, float spaceMid = 2f, float spaceBtm = 0f) {
                _spaceBtm = spaceBtm;
                EditorGUILayout.Space (spaceTop);
                EditorGUILayout.LabelField (label);
                EditorGUILayout.Space (spaceMid);
                EditorGUI.indentLevel++;
            }
            protected override void CloseScope () {
                EditorGUI.indentLevel--;
                EditorGUILayout.Space (_spaceBtm);
            }
        }

        private class IndentScope : GUI.Scope {
            private float _spaceBtm;
            private int _indent;
            internal IndentScope (float spaceTop = 0f, float spaceBtm = 0f, int indent = 1) {
                _spaceBtm = spaceBtm;
                _indent = indent;
                EditorGUILayout.Space (spaceTop);
                EditorGUI.indentLevel += indent;
            }
            protected override void CloseScope () {
                EditorGUI.indentLevel -= _indent;
                EditorGUILayout.Space (_spaceBtm);
            }
        }


        private class DisableScope : GUI.Scope {
            private readonly bool _tmp;
            internal DisableScope (bool isActive = false) {
                _tmp = isActive;
                if (!_tmp) EditorGUI.BeginDisabledGroup (true);
            }
            protected override void CloseScope () {
                if (!_tmp) EditorGUI.EndDisabledGroup ();
            }
        }

        private class ColorScope : GUI.Scope {
            private readonly Color _tmp;
            internal ColorScope (Color color) {
                _tmp = GUI.color;
                GUI.color = color;
            }
            protected override void CloseScope () {
                GUI.color = _tmp;
            }
        }

#else
        // ----------------------------------------------------------- 以下 UNITY 2030.3より下用
        public override void OnInspectorGUI () {
            onInspectorGUI_UnityVersionLow ();
        }
#endif

        private bool isDark => EditorGUIUtility.isProSkin;
        private GUIStyle _textStyle, _linkUrlStyle, _warningStyle;
        private void onInspectorGUI_UnityVersionLow () {
            if (_textStyle == null) {
                _textStyle = new GUIStyle (EditorStyles.label);
                _textStyle.wordWrap = true;
                _textStyle.fontSize = 14;

                _linkUrlStyle = new GUIStyle (_textStyle);
                _linkUrlStyle.wordWrap = false;
                _linkUrlStyle.normal.textColor = isDark ? new Color (0f, 0.8f, 1f, 1f) : new Color (0f, 0.4f, 0.8f, 1f);
                _linkUrlStyle.hover.textColor = _linkUrlStyle.normal.textColor + Color.white * (isDark ? 0.3f : 0.2f);
                _linkUrlStyle.stretchWidth = false;

                _warningStyle = new GUIStyle (_textStyle);
                _warningStyle.normal.textColor = isDark ? Color.white : new Color (0.8f, 0.1f, 0f, 1f);
                _warningStyle.normal.background = Texture2D.whiteTexture;
            }

            EditorGUILayout.Space (50);
            labelWarning ($"This asset is not compatible with current Unity version {Application.unityVersion}. Unity {Constant.REQUIRE_UNITY_VER} is required.");
            EditorGUILayout.Space (10);
            GUILayout.Label ($"Current : Unity {Application.unityVersion}", _textStyle);
            GUILayout.Label ($"Require : Unity {Constant.REQUIRE_UNITY_VER}", _textStyle);

            EditorGUILayout.Space (10);
            var url = Constant.URL_MANUAL;
            if (labelLink (new GUIContent ("Open Asset Manual", url)))
                Application.OpenURL (url);
            EditorGUILayout.Space (50);
        }

        void labelWarning (string text) {
            Color tmp = GUI.backgroundColor;
            GUI.backgroundColor = isDark ? new Color (1f, 0.3f, 0.6f, 0.4f) : new Color (1f, 0.3f, 0.6f, 0.5f);
            GUILayout.Label (text, _warningStyle);
            GUI.backgroundColor = tmp;
        }

        private bool labelLink (GUIContent label, params GUILayoutOption [] options) {
            GUIStyle style = _linkUrlStyle;
            var rect = GUILayoutUtility.GetRect (label, style, options);
            Handles.BeginGUI ();
            Handles.color = style.normal.textColor;
            Handles.DrawLine (new Vector3 (rect.xMin, rect.yMax), new Vector3 (rect.xMax, rect.yMax));
            Handles.color = Color.white;
            Handles.EndGUI ();
            EditorGUIUtility.AddCursorRect (rect, MouseCursor.Link);
            return GUI.Button (rect, label, style);
        }

        // ----------------------------------------------------------- 
    }

}

#endif
