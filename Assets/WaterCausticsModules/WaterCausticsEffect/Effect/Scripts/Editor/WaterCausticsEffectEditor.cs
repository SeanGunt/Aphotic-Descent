// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if UNITY_EDITOR && WCE_URP
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MH.WaterCausticsModules.Effect;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MH.WaterCausticsModules {
    [CanEditMultipleObjects]
    [CustomEditor (typeof (WaterCausticsEffect))]
    public class WaterCausticsEffectEditor : Editor {
        private SerializedProperty m_method;
        private SerializedProperty m_normalSrc;
        private SerializedProperty m_debugInfo;
        private SerializedProperty m_debugMode;
        private SerializedProperty m_useLayer;
        private SerializedProperty m_renderLayerMask;
        private SerializedProperty m_layerMask;
        private SerializedProperty m_clipOutside;
        private SerializedProperty m_texture;
        private SerializedProperty m_textureChannel;
        private SerializedProperty m_textureRotation;
        private SerializedProperty m_texRotSinCos;
        private SerializedProperty m_useRandomTiling;
        private SerializedProperty m_tilingSeed;
        private SerializedProperty m_tilingRotation;
        private SerializedProperty m_tilingHardness;
        private SerializedProperty m_intensity;
        private SerializedProperty m_mainLit;
        private SerializedProperty m_addLit;
        private SerializedProperty m_colorShift;
        private SerializedProperty m_colorShiftDir;
        private SerializedProperty m_scale;
        private SerializedProperty m_surfaceY;
        private SerializedProperty m_surfFadeStart;
        private SerializedProperty m_surfFadeEnd;
        private SerializedProperty m_useDepthFade;
        private SerializedProperty m_depthFadeStart;
        private SerializedProperty m_depthFadeEnd;
        private SerializedProperty m_useDistanceFade;
        private SerializedProperty m_distanceFadeStart;
        private SerializedProperty m_distanceFadeEnd;
        private SerializedProperty m_litSaturation;
        private SerializedProperty m_multiply;
        private SerializedProperty m_normalAttenRate;
        private SerializedProperty m_normalAtten;
        private SerializedProperty m_transparentBackside;
        private SerializedProperty m_backsideShadow;
        private SerializedProperty m_shadowIntensity;
        private SerializedProperty m_receiveShadows;
        private SerializedProperty m_useMainLit;
        private SerializedProperty m_useAddLit;
        private SerializedProperty m_useImageMask;
        private SerializedProperty m_imageMaskTexture;
        private SerializedProperty m_stencilRef;
        private SerializedProperty m_stencilReadMask;
        private SerializedProperty m_stencilWriteMask;
        private SerializedProperty m_stencilComp;
        private SerializedProperty m_stencilPass;
        private SerializedProperty m_stencilFail;
        private SerializedProperty m_stencilZFail;
        private SerializedProperty m_cullMode;
        private SerializedProperty m_zWriteMode;
        private SerializedProperty m_zTestMode;
        private SerializedProperty m_depthOffsetFactor;
        private SerializedProperty m_depthOffsetUnits;
        private SerializedProperty m_shader;
        private SerializedProperty m_noTexture;
        private SerializedProperty m_useCustomFunc;
        private SerializedProperty m_renderEvent;
        private SerializedProperty m_renderEventAdjust;

        private void prepProperties () {
            if (m_method != null) return;
            m_method = serializedObject.FindProperty ("m_method");
            m_normalSrc = serializedObject.FindProperty ("m_normalSrc");
            m_debugInfo = serializedObject.FindProperty ("m_debugInfo");
            m_debugMode = serializedObject.FindProperty ("m_debugMode");
            m_useLayer = serializedObject.FindProperty ("m_useLayer");
            m_renderLayerMask = serializedObject.FindProperty ("m_renderLayerMask");
            m_layerMask = serializedObject.FindProperty ("m_layerMask");
            m_clipOutside = serializedObject.FindProperty ("m_clipOutside");
            m_texture = serializedObject.FindProperty ("m_texture");
            m_textureChannel = serializedObject.FindProperty ("m_textureChannel");
            m_textureRotation = serializedObject.FindProperty ("m_textureRotation");
            m_texRotSinCos = serializedObject.FindProperty ("m_texRotSinCos");
            m_useRandomTiling = serializedObject.FindProperty ("m_useRandomTiling");
            m_tilingSeed = serializedObject.FindProperty ("m_tilingSeed");
            m_tilingRotation = serializedObject.FindProperty ("m_tilingRotation");
            m_tilingHardness = serializedObject.FindProperty ("m_tilingHardness");
            m_intensity = serializedObject.FindProperty ("m_intensity");
            m_mainLit = serializedObject.FindProperty ("m_mainLit");
            m_addLit = serializedObject.FindProperty ("m_addLit");
            m_colorShift = serializedObject.FindProperty ("m_colorShift");
            m_colorShiftDir = serializedObject.FindProperty ("m_colorShiftDir");
            m_scale = serializedObject.FindProperty ("m_scale");
            m_surfaceY = serializedObject.FindProperty ("m_surfaceY");
            m_surfFadeStart = serializedObject.FindProperty ("m_surfFadeStart");
            m_surfFadeEnd = serializedObject.FindProperty ("m_surfFadeEnd");
            m_useDepthFade = serializedObject.FindProperty ("m_useDepthFade");
            m_depthFadeStart = serializedObject.FindProperty ("m_depthFadeStart");
            m_depthFadeEnd = serializedObject.FindProperty ("m_depthFadeEnd");
            m_useDistanceFade = serializedObject.FindProperty ("m_useDistanceFade");
            m_distanceFadeStart = serializedObject.FindProperty ("m_distanceFadeStart");
            m_distanceFadeEnd = serializedObject.FindProperty ("m_distanceFadeEnd");
            m_litSaturation = serializedObject.FindProperty ("m_litSaturation");
            m_multiply = serializedObject.FindProperty ("m_multiply");
            m_normalAttenRate = serializedObject.FindProperty ("m_normalAttenRate");
            m_normalAtten = serializedObject.FindProperty ("m_normalAtten");
            m_transparentBackside = serializedObject.FindProperty ("m_transparentBackside");
            m_backsideShadow = serializedObject.FindProperty ("m_backsideShadow");
            m_shadowIntensity = serializedObject.FindProperty ("m_shadowIntensity");
            m_receiveShadows = serializedObject.FindProperty ("m_receiveShadows");
            m_useMainLit = serializedObject.FindProperty ("m_useMainLit");
            m_useAddLit = serializedObject.FindProperty ("m_useAddLit");
            m_useImageMask = serializedObject.FindProperty ("m_useImageMask");
            m_imageMaskTexture = serializedObject.FindProperty ("m_imageMaskTexture");
            m_stencilRef = serializedObject.FindProperty ("m_stencilRef");
            m_stencilReadMask = serializedObject.FindProperty ("m_stencilReadMask");
            m_stencilWriteMask = serializedObject.FindProperty ("m_stencilWriteMask");
            m_stencilComp = serializedObject.FindProperty ("m_stencilComp");
            m_stencilPass = serializedObject.FindProperty ("m_stencilPass");
            m_stencilFail = serializedObject.FindProperty ("m_stencilFail");
            m_stencilZFail = serializedObject.FindProperty ("m_stencilZFail");
            m_cullMode = serializedObject.FindProperty ("m_cullMode");
            m_zWriteMode = serializedObject.FindProperty ("m_zWriteMode");
            m_zTestMode = serializedObject.FindProperty ("m_zTestMode");
            m_depthOffsetFactor = serializedObject.FindProperty ("m_depthOffsetFactor");
            m_depthOffsetUnits = serializedObject.FindProperty ("m_depthOffsetUnits");
            m_shader = serializedObject.FindProperty ("m_shader");
            m_noTexture = serializedObject.FindProperty ("m_noTexture");
            m_useCustomFunc = serializedObject.FindProperty ("m_useCustomFunc");
            m_renderEvent = serializedObject.FindProperty ("m_renderEvent");
            m_renderEventAdjust = serializedObject.FindProperty ("m_renderEventAdjust");
        }

        private SerializedObject _wceData;
        private SerializedProperty m_autoManageFeature;
        private SerializedObject prepWceData () {
            if (_wceData == null)
                _wceData = new SerializedObject (WaterCausticsEffectData.GetAsset ());
            if (m_autoManageFeature == null) {
                m_autoManageFeature = _wceData.FindProperty ("m_autoManageFeature");
            }
            _wceData.Update ();
            return _wceData;
        }

        static readonly GUIContent [] _cullingEnumStr = {
            new GUIContent ("Both"),
            new GUIContent ("Back"),
            new GUIContent ("Front"),
        };

        static readonly string descGenFromDepth = "[Generate from Depth] \nGenerate from _CameraDepthTexture generated by the system. This method is not good for smooth surfaces, but it does produce the correct normals.";
        static readonly string descCamNormalTex = "[Camera Normals Tex] \nSampling _CameraNormalsTexture generated by the system. This is high quality, but may produce strange results with materials that do not support normal output.";

        static readonly GUIContent [] _normalSrcStr = {
            new GUIContent ("Generate from Depth (LQ)", $"{descGenFromDepth}\n\n{descCamNormalTex}"),
            new GUIContent ("Camera Normals Tex (HQ)", $"{descGenFromDepth}\n\n{descCamNormalTex}"),
        };

        static readonly string [] _renderingLayerMaskNamesSpare = { "Layer1", "Layer2", "Layer3", "Layer4", "Layer5", "Layer6", "Layer7", "Layer8", "Layer9", "Layer10", "Layer11", "Layer12", "Layer13", "Layer14", "Layer15", "Layer16", "Layer17", "Layer18", "Layer19", "Layer20", "Layer21", "Layer22", "Layer23", "Layer24", "Layer25", "Layer26", "Layer27", "Layer28", "Layer29", "Layer30", "Layer31", "Layer32", };

        private List<ScriptableRendererData> _rendererDataList;
        private bool _hasGetRenderData;

        protected virtual void OnEnable () {
            foreach (var tar in targets.OfType<WaterCausticsEffect> ())
                tar.VersionCheck ();
        }

        UniversalRenderPipelineAsset urpAsset => GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        public override void OnInspectorGUI () {
            if (urpAsset) {
                var wceData = prepWceData ();
                prepProperties ();
                serializedObject.Update ();
                using (var check = new EditorGUI.ChangeCheckScope ()) {
                    drawProperties ();
                    serializedObject.ApplyModifiedProperties ();
                    wceData.ApplyModifiedProperties ();
                    if (check.changed) {
                        foreach (var tar in targets.OfType<WaterCausticsEffect> ())
                            tar.OnInspectorChanged ();
                    }
                }
            } else {
                // -- URP設定が完了していない場合
                onInspectorGUI_NotSettingYet ();
            }
        }

        // -----------------------------------------------------------
        readonly Color colorPinkBar = new Color (1f, 0.3f, 0.6f, 0.3f);
        readonly Color colorPinkContent = new Color (1f, 0.3f, 0.6f, 1f);
        readonly Color colorGreenContent = new Color (0.4f, 1f, 0.7f, 1f);
        private float lineH = EditorGUIUtility.singleLineHeight + 2;
        readonly float SPACE_SUB_TOP_5 = 5f;
        readonly float SPACE_SUB_BTM_12 = 12f;
        readonly float SPACE_MAIN_TOP_7 = 7f;
        readonly float SPACE_MAIN_BTM_5 = 5f;
        private Color _defaultGUIColor;

        private void setLabelAreaWidth (float labelWidthMin, float valWidthMin) {
            if (EditorGUIUtility.labelWidth < labelWidthMin)
                EditorGUIUtility.labelWidth = labelWidthMin;
            if (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth < valWidthMin)
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - valWidthMin;
        }

        private void drawProperties () {
            _defaultGUIColor = GUI.color;
            storeIndentWidth ();
            EditorGUIUtility.labelWidth += 6;
            EditorGUI.indentLevel++;
            setLabelAreaWidth (labelWidthMin: 140f, valWidthMin: 170f);
            bool isEditingMultiObj = serializedObject.isEditingMultipleObjects;
            bool isMethodEach = (m_method.enumValueIndex == (int) Method.EachMesh && !m_method.hasMultipleDifferentValues);
            bool isMethodOnce = (m_method.enumValueIndex == (int) Method.AtOnce && !m_method.hasMultipleDifferentValues);
            // ---------------------------------------------------------------------------------- System

            // ------ RendererFeature
            if (!_hasGetRenderData) {
                _hasGetRenderData = true;
                WaterCausticsEffectFeatureEditor.GetAllRendererData (out _rendererDataList);
                if (WaterCausticsEffectData.GetAsset ().AutoManageFeature)
                    WaterCausticsEffectFeatureEditor.AddFeatureToAllRenderers (_rendererDataList, useUndo : false);
            }
            bool someRenderNotHasFeature = !WaterCausticsEffectFeatureEditor.CheckAllHasActiveFeature (_rendererDataList);
            bool currentRenderNotHasFeature = someRenderNotHasFeature && !WaterCausticsEffectFeature.effective;
            // ---------------------- 

            if (expandMainGroup (m_method, true, "System", isPink : currentRenderNotHasFeature)) {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);

                // ------ RendererFeature
                if (someRenderNotHasFeature) {
                    EditorGUI.indentLevel -= 1;
                    // labelWarning("To apply this effect, a Renderer Feature needs to be added to a Renderer.", 11);
                    using (new ColorScope (currentRenderNotHasFeature ? colorPinkContent : _defaultGUIColor)) {
                        string str = "To apply this effect, a Renderer Feature needs to be added to a Renderer.";
                        if (currentRenderNotHasFeature)
                            EditorGUILayout.HelpBox ($"The Renderer Feature has not been added or activated in the current Renderer.\n{str}", MessageType.Warning);
                        else
                            EditorGUILayout.HelpBox ($"The Renderer Feature has not been added or activated in some Renderers.\n{str}", MessageType.Warning);
                        EditorGUILayout.Space (2);
                        Rect rect = GUILayoutUtility.GetRect (0, 0);
                        rect.height = lineH;
                        rect.width *= 0.5f;
                        if (GUI.Button (rect, new GUIContent ("Select Renderer", "Search and select RendererData assets."), EditorStyles.miniButton)) {
                            // -- 選択ボタン
                            if (WaterCausticsEffectFeatureEditor.GetAllRendererData (out var list)) {
                                // -- 成功
                                WaterCausticsEffectFeatureEditor.SelectAndPing (list);
                                if (list.Count >= 2) {
                                    var pathStr = WaterCausticsEffectFeatureEditor.AssetsToPathStr (list);
                                    EditorApplication.delayCall += () => EditorApplication.delayCall += () =>
                                        EditorUtility.DisplayDialog ("Multiple found.", $"Multiple RendererData assets found.\n\n{pathStr}", "OK");
                                }
                            } else {
                                // -- 見つからない
                                EditorUtility.DisplayDialog ("Not found.", $"Not found.", "OK");
                            }
                        }
                        rect.x += rect.width;
                        if (GUI.Button (rect, new GUIContent ("Fix It", "Add a Renderer Feature to Renderers."), EditorStyles.miniButton)) {
                            // -- 追加ボタン
                            if (WaterCausticsEffectFeatureEditor.AddFeatureToAllRenderers (useUndo: true)) {
                                // -- 成功
                            } else {
                                // -- 失敗
                                bool showURL = EditorUtility.DisplayDialog ("Failed", "Processing failed. Please add the Renderer Feature to Renderers manually.", "Open URP Manual", "Cancel");
                                if (showURL) Application.OpenURL (Constant.URL_HOW_TO_ADD_FEATURE);
                            }
                        }
                        EditorGUILayout.Space (lineH);
                        using (new ColorScope (_defaultGUIColor)) {
                            EditorGUILayout.Space (1);
                            if (labelLink (new GUIContent ("How to add Renderer Feature to Renderer", Constant.URL_HOW_TO_ADD_FEATURE), 10))
                                Application.OpenURL (Constant.URL_HOW_TO_ADD_FEATURE);
                            EditorGUILayout.Space (4);
                        }
                        EditorGUI.indentLevel += 1;
                        EditorGUILayout.Space (15);
                    }
                }
                // ----------------------

                EditorGUILayout.PropertyField (m_method, new GUIContent ("Effect Method", "[At Once]\nDraws the effect at once using the camera's depth and normal texture. It can also be applied to objects with materials that are deformed by shaders. \n\n[Each Mesh]\nDraws effects to each mesh. Masking with layers and the surface to be drawn can be specified. However, it cannot be applied to objects with materials that deform with shaders. For such objects, embed custom function in the shader or use the At Once method."));
                if (isMethodOnce) {
                    using (new IndentScope (-2f, 0f)) {
                        popup (m_normalSrc, _normalSrcStr, "Normal Data", $"How to get normal vector in world space.\n\n{descGenFromDepth}\n\n{descCamNormalTex}");
                    }
                }
                drawBoolAndValue (m_debugInfo, m_debugMode, true, new GUIContent ("Debug Info", "Display data for debugging. This is only valid on the editor.\n\n" +
                    $"[{DebugMode.Normal}]\nDisplays world-space normal data.\n\n" +
                    $"[{DebugMode.Depth}]\nDisplays the depth data.\n\n" +
                    $"[{DebugMode.Facing}]\nDisplays the plane facing the camera as bright.\n\n" +
                    $"[{DebugMode.Caustics}]\nDisplays only caustics effects.\n\n" +
                    $"[{DebugMode.LightArea}]\nDisplays the affected area by each light.\n\n" +
                    $"If some objects are not rendering correctly on At Once method with Camera Normals Tex, the object's material is outputting the wrong normals. Check the normals on this screen and modify the material (shader) of the object."));

                EditorGUILayout.Space (SPACE_SUB_BTM_12);
                // ---------------------------------------------------------------------------------- Influence Scope
                if (expandSubGroup (m_useImageMask, true, "Influence Scope")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);
                    selectGameObjectLayer (new GUIContent ("Layer", "The layer in which this effect exists."));
                    if (isMethodEach) {
                        EditorGUILayout.PropertyField (m_layerMask, new GUIContent ("Layer Mask", "Specify the layer on which the effect will be drawn. Objects on unchecked layers will be ignored."));
                    }
                    if (isMethodEach) {
                        EditorGUILayout.PropertyField (m_clipOutside, new GUIContent ("Clip Outside", "Draw effects only inside the volume."));
                    }
                    drawBoolAndValue (m_useImageMask, m_imageMaskTexture, hide : true, new GUIContent ("Image Mask", "Masking with an image."));

                    if (isMethodEach) {
                        popup (m_cullMode, _cullingEnumStr, "Render Face", "Which face to draw.");
                    }

                    // EditorGUILayout.Space (2);
                    // if (isExpand (m_renderEventAdjust, false, new GUIContent ("Advanced", "Advanced Settings"))) {
                    //     using (new IndentScope (0f, 0f)) {
                    EditorGUILayout.PropertyField (m_useCustomFunc, new GUIContent ("Custom Function", "Supports Custom Function for shader. \nTransmits the settings to the WaterCausticsEmissionSync function embedded in the shader. \nTurning this On will copy the settings to a global shader variable. \n\nIf there are multiple effects with this setting On in a scene, the last active effect will be used."));

                    {
                        // 描画タイミング設定
                        bool isExpanded = isExpand (m_renderEvent, true, new GUIContent (""));
                        EditorGUILayout.Space (-EditorGUIUtility.singleLineHeight - 2f);
                        string baseTimingName = splitCamelCase (((RenderPassEvent) m_renderEvent.intValue).ToString ());
                        int sysOpqTiming = (int) WaterCausticsEffect.SYS_OPAQUE_TEX_EVENT;
                        string sysOpqName = WaterCausticsEffect.SYS_OPAQUE_TEX_EVENT.ToString ();
                        string sysOpqDesc = $"{sysOpqName}({sysOpqTiming})";
                        string sysOpqDescPlusOne = $"{sysOpqName}+1 ({sysOpqTiming+1})";
                        string defaultTiming = WaterCausticsEffect.RENDER_EVENT.ToString ();
                        int defaultTimingAdj = WaterCausticsEffect.RENDER_EVENT_ADJ;
                        int baseTiming = m_renderEvent.intValue;
                        int adjTiming = m_renderEventAdjust.intValue;
                        int adjusted = baseTiming + adjTiming;
                        bool isHasDifVal = m_renderEvent.hasMultipleDifferentValues || m_renderEventAdjust.hasMultipleDifferentValues;
                        EditorGUILayout.LabelField (new GUIContent ("Draw Timing", $"Specifies the timing of drawing.\n\nTo display this effect on _CameraOpaqueTexture, it must be drawn before {sysOpqDesc}."), new GUIContent (isHasDifVal ? "-" : $"{baseTimingName} {(adjTiming < 0 ? "-" : "+")}{Mathf.Abs(adjTiming)} ({Mathf.Clamp(adjusted, 0, 1000)})"));
                        if (isExpanded) {
                            using (new IndentScope (-2f, 4f)) {
                                EditorGUILayout.PropertyField (m_renderEvent, new GUIContent ($"Render Event", $"Controls when the render executes. \n[Default: {defaultTiming}]"));
                                EditorGUILayout.PropertyField (m_renderEventAdjust, new GUIContent ("Adjustment", $"Controls when the render executes. This number is added to the Draw Timing above. \n[Default: {defaultTimingAdj}]"));
                                bool isEarly = (adjusted <= sysOpqTiming);
                                string warning = isEarly ? $"To use a value between 0 and 1 in the Multiply Color setting, set it after {sysOpqDescPlusOne}." :
                                    $"To display this effect on _CameraOpaqueTexture, it must be drawn before {sysOpqDesc}.";
                                EditorGUILayout.HelpBox (new GUIContent (warning, ""), true);
                            }
                        }
                    }

                    if (isMethodOnce) {
                        var names = urpAsset.renderingLayerMaskNames;
                        if (names == null) names = _renderingLayerMaskNamesSpare;
                        bitMask (m_renderLayerMask, names, "Render Mask", "Rendering Layer Mask of this effect. It works as same as RenderingLayerMask of MeshRenderer.");
                    }
                    if (isMethodEach) {
                        if (isExpand (m_zTestMode, false, new GUIContent ("Depth Buffer", "Adjust Depth Testing and Depth Offset."))) {
                            using (new IndentScope (0, 2)) {
                                EditorGUILayout.PropertyField (m_zWriteMode, new GUIContent ("ZWrite", "Whether to write depth values to the depth buffer."));
                                EditorGUILayout.PropertyField (m_zTestMode, new GUIContent ("ZTest", "Comparison method with already existing depth values."));
                                EditorGUILayout.PropertyField (m_depthOffsetFactor, new GUIContent ("Offset Factor", "Offset Factor"));
                                EditorGUILayout.PropertyField (m_depthOffsetUnits, new GUIContent ("Offset Units", "Offset Units"));
                            }
                        }
                        EditorGUILayout.Space (2);
                    }
                    if (isExpand (m_stencilRef, false, new GUIContent ("Stencil Buffer", "The Stencil Buffer can be used to limit the objects to be drawn or used for subsequent effects."))) {
                        using (new IndentScope (0, 2)) {
                            EditorGUILayout.PropertyField (m_stencilRef, new GUIContent ("Ref", "Stencil Reference Value"));
                            EditorGUILayout.PropertyField (m_stencilReadMask, new GUIContent ("ReadMask", "Stencil Read Mask"));
                            EditorGUILayout.PropertyField (m_stencilWriteMask, new GUIContent ("WriteMask", "Stencil Write Mask"));
                            EditorGUILayout.PropertyField (m_stencilComp, new GUIContent ("Comp", "Stencil Compare Operation"));
                            EditorGUILayout.PropertyField (m_stencilPass, new GUIContent ("Pass", "Stencil Pass Operation"));
                            EditorGUILayout.PropertyField (m_stencilFail, new GUIContent ("Fail", "Stencil Fail Operation"));
                            EditorGUILayout.PropertyField (m_stencilZFail, new GUIContent ("ZFail", "Stencil Z Fail Operation"));
                        }
                    }
                    //     }
                    // }

                }
                EditorGUILayout.Space (SPACE_SUB_BTM_12);
            }

            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            // ---------------------------------------------------------------------------------- Effect Group
            bool useTextureWarning = (m_texture.objectReferenceValue == null && isGameObjectOnScene () && !isEditingMultiObj);
            if (expandMainGroup (m_texture, true, "Caustics Effect", isPink : useTextureWarning && m_texture.isExpanded)) {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);

                if (expandSubGroup (m_textureRotation, true, "Texture", isPink : useTextureWarning)) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);

                    using (new ColorScope (useTextureWarning?colorPinkContent : GUI.color)) {
                        EditorGUILayout.PropertyField (m_texture, new GUIContent ("Caustics Texture", $"Set the RenderTexture specified as the output destination in the {typeof(WaterCausticsTexGenerator).Name}."));
                        if (!m_texture.hasMultipleDifferentValues) {
                            if (useTextureWarning) {
                                EditorGUILayout.Space (1);
                                EditorGUILayout.BeginHorizontal ();
                                GUILayout.FlexibleSpace ();
                                if (GUILayout.Button ("Search from this Scene", EditorStyles.miniButton, GUILayout.Width (150))) {
                                    var gen = FindObjectsOfType<WaterCausticsTexGenerator> ().FirstOrDefault (g => g.renderTexture != null);
                                    if (gen != null)
                                        m_texture.objectReferenceValue = gen.renderTexture;
                                    else
                                        EditorUtility.DisplayDialog ("Not Found", $"There is no {typeof(WaterCausticsTexGenerator).Name} with active and having RenderTexture in this scene.", "OK");
                                }
                                EditorGUILayout.EndHorizontal ();
                                EditorGUILayout.Space (3);
                            } else {
                                var tex = m_texture.objectReferenceValue as Texture;
                                if (tex != null) {
                                    EditorGUILayout.HelpBox ($"{tex.width}x{tex.height} / {tex.graphicsFormat}", MessageType.None);
                                    EditorGUILayout.Space (3);
                                }
                            }
                        }
                    }
                    EditorGUILayout.PropertyField (m_textureChannel, new GUIContent ("Channel", "Channels to be used. Set to R if using R-channel only textures."));
                    using (var check = new EditorGUI.ChangeCheckScope ()) {
                        using (new IndentScope (0f, 0f, 1)) {
                            EditorGUILayout.PropertyField (m_textureRotation, new GUIContent ("Rotation", "Rotate the texture."));
                            drawDirMark (m_textureRotation, true);
                        }
                        if (check.changed) {
                            float rad = m_textureRotation.floatValue * Mathf.Deg2Rad;
                            m_texRotSinCos.vector2Value = new Vector2 (Mathf.Sin (rad), Mathf.Cos (rad));
                        }
                    }
                    EditorGUILayout.PropertyField (m_useRandomTiling, new GUIContent ("Random Tiling", "Use randomized hexagonal tiling. This reduces unnatural repetitions that appear on distant planes."));
                    if (m_useRandomTiling.boolValue) {
                        using (new IndentScope (-2f, 0f)) {
                            EditorGUILayout.PropertyField (m_tilingSeed, new GUIContent ("Seed", "Random seed value."));
                            EditorGUILayout.PropertyField (m_tilingHardness, new GUIContent ("Hardness", "Edge hardness."));
                            EditorGUILayout.PropertyField (m_tilingRotation, new GUIContent ("Rotation", "Rotate tiles randomly."));
                        }
                    }
                    EditorGUILayout.Space (SPACE_SUB_BTM_12);
                }

                if (expandSubGroup (m_scale, true, "Dimensions")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);

                    EditorGUILayout.PropertyField (m_scale, new GUIContent ("Scale", "Texture size at the height of the water surface."));
                    EditorGUILayout.PropertyField (m_surfaceY, new GUIContent ("Water Surface Y", "Height of the water surface. Y-axis. The projected position of the light is calculated with respect to this plane."));

                    EditorGUILayout.Space (4);
                    drawStartEndProp (m_surfFadeStart, m_surfFadeEnd, new GUIContent ("Surface Fade", "Attenuates light as it approaches the surface of the water."));
                    EditorGUILayout.PropertyField (m_useDepthFade, new GUIContent ("Depth Fade", "Attenuates light as depth increases."));
                    if (m_useDepthFade.boolValue) {
                        using (new IndentScope (-2f, 0f))
                        drawStartEndProp (m_depthFadeStart, m_depthFadeEnd, new GUIContent ("Range", "Attenuates light as depth increases."));
                    }
                    EditorGUILayout.PropertyField (m_useDistanceFade, new GUIContent ("Distance Fade", "Attenuates light as distance increases."));
                    if (m_useDistanceFade.boolValue) {
                        using (new IndentScope (-2f, 0f))
                        drawStartEndProp (m_distanceFadeStart, m_distanceFadeEnd, new GUIContent ("Range", "Attenuates light as distance increases."));
                    }

                    EditorGUILayout.Space (SPACE_SUB_BTM_12);
                }
                if (expandSubGroup (m_intensity, true, "Effect")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);

                    EditorGUILayout.PropertyField (m_intensity, new GUIContent ("Intensity", "Intensity of effect."));
                    using (new IndentScope (-1f, 1f)) {
                        drawBoolAndValue (m_useMainLit, m_mainLit, hide : true, new GUIContent ("Main Light", "Adjust the intensity of the main light. If the checkbox is Off, the main light calculation is skipped."));
                        drawBoolAndValue (m_useAddLit, m_addLit, hide : true, new GUIContent ("Additional Lights", "Adjust the intensity of the additional lights. If the check box is Off, the calculation of additional lights is skipped."));
                    }
                    EditorGUILayout.Space (2);

                    drawBoolAndValue (m_receiveShadows, m_shadowIntensity, hide : true, new GUIContent ("Shadow", "Strength of shadow. If the checkbox is unchecked, the shadow calculation is skipped. \n\nFor the At Once method, \"Transparent Receive Shadows\" in the URP settings must also be set to On."));
                    EditorGUILayout.Space (2);
                    EditorGUILayout.PropertyField (m_colorShift, new GUIContent ("Color Shift", "Amount of RGB channel shift."));
                    if (m_colorShift.floatValue > 0f) {
                        using (new IndentScope (-2f, 0f, 2)) {
                            EditorGUILayout.PropertyField (m_colorShiftDir, new GUIContent ("Direction", "Direction of shift for RGB channels."));
                            drawDirMark (m_colorShiftDir, true);
                        }
                    }
                    EditorGUILayout.Space (2);

                    EditorGUILayout.PropertyField (m_litSaturation, new GUIContent ("Light Color", "Color intensity of the light."));
                    EditorGUILayout.Space (2);
                    EditorGUILayout.PropertyField (m_multiply, new GUIContent ("Multiply Color", "Multiply by the screen color and then add. \nIf this value is 1 or 0, it is processed faster because it uses the shader's Blend function. Otherwise, _CameraOpaqueTexture is sampled and multiplied. If the Draw Timing setting is before _CameraOpaqueTexture is drawn, it is processed as 1."));
                    if (!isEditingMultiObj && (m_multiply.floatValue > 0f && m_multiply.floatValue < 1f) &&
                        ((m_renderEvent.intValue + m_renderEventAdjust.intValue) <= (int) WaterCausticsEffect.SYS_OPAQUE_TEX_EVENT)
                    ) {
                        // 描画タイミングが早すぎる場合警告
                        using (new IndentScope (0f, 0f)) {
                            EditorGUILayout.HelpBox (new GUIContent ("Only 0 or 1 is valid because the Draw Timing is too early.", "Draw Timing in Influence Scope settings is too early and _CameraOpaqueTexture does not exist, so it cannot be drawn with a setting between 0 and 1. Therefore, it is drawn as 1."), true);
                        }
                    }

                    EditorGUILayout.Space (3);
                    EditorGUILayout.PropertyField (m_normalAtten, new GUIContent ("Normal Attenuation", "Attenuation due to the angle between the normal and the light ray. \n[Default: 1]")); {
                        using (new IndentScope (-2f, 2f)) {
                            using (new DisableScope (m_normalAtten.floatValue > 0f)) {
                                EditorGUILayout.PropertyField (m_normalAttenRate, new GUIContent ("Rate", "Rate of normal attenuation. How quickly does the light fade. \n[Default: 1.5]"));
                                EditorGUILayout.PropertyField (m_transparentBackside, new GUIContent ("Transparent", "The intensity of light transmitted to the backside. \n[Default: 0]"));
                            }
                            using (new DisableScope (m_receiveShadows.boolValue && (m_normalAtten.floatValue < 1f || m_transparentBackside.floatValue > 0f))) {
                                EditorGUILayout.PropertyField (m_backsideShadow, new GUIContent ("Backside Shadow", "The intensity of shadow on the backside. \n[Default: 0]"));
                            }
                        }
                    }
                    EditorGUILayout.Space (SPACE_SUB_BTM_12);
                }
            }

            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            // ---------------------------------------------------------------------------------- Advanced Settings
            if (expandMainGroup (m_backsideShadow, false, "Advanced Settings")) {
                EditorGUILayout.Space (SPACE_MAIN_TOP_7);

                if (expandSubGroup (m_normalAttenRate, false, "Renderer Feature")) {
                    EditorGUILayout.Space (SPACE_SUB_TOP_5);
                    using (var check = new EditorGUI.ChangeCheckScope ()) {
                        EditorGUILayout.PropertyField (m_autoManageFeature, new GUIContent ("Auto-Management", "Automatically manages Renderer Feature. It automatically adds the Renderer Feature for this effect to all Renderer Data in the project.\n\nIf this is turned off, it is required to manually add the Renderer Feature to the Renderer Data."));
                        if (check.changed && m_autoManageFeature.boolValue == true) {
                            WaterCausticsEffectFeatureEditor.AddFeatureToAllRenderers (useUndo: false);
                        }
                        if (!m_autoManageFeature.boolValue) {
                            EditorGUILayout.Space (1);
                            EditorGUI.indentLevel++;
                            EditorGUILayout.HelpBox ("To apply this effect, a Renderer Feature needs to be added to a Renderer.", MessageType.None);
                            EditorGUILayout.Space (1);
                            if (labelLink (new GUIContent ("How to add Renderer Feature to Renderer", Constant.URL_HOW_TO_ADD_FEATURE), 10, 2))
                                Application.OpenURL (Constant.URL_HOW_TO_ADD_FEATURE);
                            EditorGUI.indentLevel--;
                        }
                    }
                    EditorGUILayout.Space (SPACE_SUB_BTM_12);
                }
            }


            EditorGUILayout.Space (SPACE_SUB_BTM_12);
            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            EditorGUILayout.Space (SPACE_MAIN_BTM_5);
            EditorGUI.indentLevel--;
        }

        // ----------------------------------------------------------- Parts
        private string splitCamelCase (string str) {
            return Regex.Replace (
                Regex.Replace (str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        private float _indentWidth;
        private void storeIndentWidth () {
            if (_indentWidth != 0f) return;
            var x0 = EditorGUI.IndentedRect (Rect.zero).x;
            EditorGUI.indentLevel++;
            _indentWidth = EditorGUI.IndentedRect (Rect.zero).x - x0;
            EditorGUI.indentLevel--;
        }

        private void drawBoolAndValue (SerializedProperty propBool, SerializedProperty propValue, bool hide, GUIContent label) {
            EditorGUILayout.PropertyField (propBool, label);
            var rect = GUILayoutUtility.GetLastRect ();
            var labelW = EditorGUIUtility.labelWidth;
            if (!hide || propBool.boolValue) {
                using (new DisableScope (propBool.boolValue)) {
                    EditorGUIUtility.labelWidth += 25;
                    EditorGUI.PropertyField (rect, propValue, new GUIContent (" "));
                }
            }
            EditorGUIUtility.labelWidth = labelW;
        }

        private void selectGameObjectLayer (GUIContent label) {
            var gameObjects = targets.Select (t => (t as WaterCausticsEffect).gameObject).ToArray ();
            var layers = gameObjects.Select (go => go.layer).Distinct ().ToArray ();
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = (layers.Length != 1);
                int newVal = EditorGUILayout.LayerField (label, layers [0]);
                EditorGUI.showMixedValue = false;
                if (check.changed) {
                    Undo.RecordObjects (gameObjects, "Changed Layer");
                    foreach (var go in gameObjects) {
                        go.layer = newVal;
                        EditorUtility.SetDirty (go);
                    }
                    if (layers.Length >= 2 && gameObjects.Length == Selection.objects.Length) {
                        // 上部のレイヤー表示Multiple(-)を更新するため選択中の場合は再選択
                        bool isSelected = true;
                        foreach (var o in gameObjects) {
                            if (!Selection.objects.Contains (o)) {
                                isSelected = false;
                                break;
                            }
                        }
                        if (isSelected) {
                            Selection.activeGameObject = null;
                            EditorApplication.delayCall += () => { Selection.objects = gameObjects; };
                        }
                    }
                }
            }
        }

        private void selectLayer (SerializedProperty prop, GUIContent label) {
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                var newVal = EditorGUILayout.LayerField (label, prop.intValue);
                EditorGUI.showMixedValue = false;
                if (check.changed)
                    prop.intValue = newVal;
            }
        }

        private void drawStartEndProp (SerializedProperty propStt, SerializedProperty propEnd, GUIContent label) {
            EditorGUILayout.LabelField (label);
            var rect = GUILayoutUtility.GetLastRect ();
            rect.x += EditorGUIUtility.labelWidth;
            rect.width -= EditorGUIUtility.labelWidth;
            drawStartEndPropInRect (rect, propStt, propEnd);
        }


        private void drawStartEndPropInRect (Rect rect, SerializedProperty propStt, SerializedProperty propEnd) {
            bool isWide = (rect.width > 140);
            float span = isWide ? 5f : 3f;
            var rect2 = rect;
            var rect3 = rect;
            rect2.width = rect3.width = (rect.width - span) * 0.5f;
            rect3.x += rect2.width + span;
            var storeIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var labelW = EditorGUIUtility.labelWidth;
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = propStt.hasMultipleDifferentValues;
                EditorGUIUtility.labelWidth = isWide? 32 : 10;
                float newStt = EditorGUI.FloatField (rect2, isWide ? "Start" : "S", propStt.floatValue);
                if (check.changed)
                    propStt.floatValue = Mathf.Clamp (newStt, 0, propEnd.floatValue);
            }
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = propEnd.hasMultipleDifferentValues;
                EditorGUIUtility.labelWidth = isWide ? 26 : 10;
                float newEnd = EditorGUI.FloatField (rect3, isWide ? "End" : "E", propEnd.floatValue);
                if (check.changed)
                    propEnd.floatValue = Mathf.Max (newEnd, propStt.floatValue);
            }
            EditorGUI.showMixedValue = false;
            EditorGUIUtility.labelWidth = labelW;
            EditorGUI.indentLevel = storeIndent;
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

        private void popup (SerializedProperty prop, GUIContent [] enumStr, string text, string tooltip) {
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                var newVal = EditorGUILayout.Popup (new GUIContent (text, tooltip), prop.enumValueIndex, enumStr);
                EditorGUI.showMixedValue = false;
                if (check.changed)
                    prop.enumValueIndex = newVal;
            }
        }

        private void popup (SerializedProperty prop, string [] enumStr, string text, string tooltip) {
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                var newVal = EditorGUILayout.Popup (new GUIContent (text, tooltip), prop.enumValueIndex, enumStr);
                EditorGUI.showMixedValue = false;
                if (check.changed)
                    prop.enumValueIndex = newVal;
            }
        }

        private void bitMask (SerializedProperty prop, string [] options, string text, string tooltip) {
            using (var check = new EditorGUI.ChangeCheckScope ()) {
                EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
                int newVal = EditorGUILayout.MaskField (new GUIContent (text, tooltip), prop.intValue, options);
                EditorGUI.showMixedValue = false;
                if (check.changed) {
                    uint uintVal = unchecked ((uint) newVal);
                    prop.longValue = uintVal;
                }
            }
        }

        private bool isExpand (SerializedProperty prop, bool isDefaultOpen, GUIContent label) {
            prop.isExpanded = EditorGUILayout.Foldout (prop.isExpanded != isDefaultOpen, label) != isDefaultOpen;
            return prop.isExpanded != isDefaultOpen;
        }

        private bool isExpandMarkOnly (SerializedProperty prop, bool isDefaultOpen, float adjustX = 0f, float adjustY = 0f) {
            var rect = GUILayoutUtility.GetLastRect ();
            prop.isExpanded = EditorGUI.Foldout (rect, prop.isExpanded != isDefaultOpen, " ") != isDefaultOpen;
            return prop.isExpanded != isDefaultOpen;
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
            internal LabelAndIndentScope (GUIContent label, float spaceTop = 0f, float spaceMid = 2f, float spaceBtm = 0f) {
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
            internal IndentScope (float spaceTop = 3f, float spaceBtm = 10f, int indent = 1) {
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

        // ----------------------------------------------------------- URPパッケージは在るけれど 設定が完了していない場合
        private bool isDark => EditorGUIUtility.isProSkin;
        private GUIStyle _textStyle, _linkUrlStyle, _warningStyle;

        private void prepStyle () {
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
        }

        private void onInspectorGUI_NotSettingYet () {
            prepStyle ();
            storeIndentWidth ();
            EditorGUILayout.Space (50);
            labelWarning ($"UniversalRP setup is not completed.");
            EditorGUILayout.Space (10);
            GUILayout.Label ($"Please refer to the Unity manual page to setup, or create a new project with the URP 3D template.", _textStyle);
            EditorGUILayout.Space (10);
            var urlA = "https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.0/manual/InstallURPIntoAProject.html";
            if (labelLink (new GUIContent ("Unity URP Setup Manual", urlA)))
                Application.OpenURL (urlA);
            EditorGUILayout.Space (5);
            var urlB = Constant.URL_MANUAL;
            if (labelLink (new GUIContent ("Asset Manual", urlB)))
                Application.OpenURL (urlB);
            EditorGUILayout.Space (50);
        }

        void labelWarning (string text, int fontSize = 18) {
            prepStyle ();
            _warningStyle.fontSize = fontSize;
            Color tmp = GUI.backgroundColor;
            GUI.backgroundColor = isDark ? new Color (1f, 0.3f, 0.6f, 0.4f) : new Color (1f, 0.3f, 0.6f, 0.5f);
            GUILayout.Label (text, _warningStyle);
            GUI.backgroundColor = tmp;
        }

        private bool labelLink (GUIContent label, int fontSize = 14, int indent = 0, params GUILayoutOption [] options) {
            prepStyle ();
            _linkUrlStyle.fontSize = fontSize;
            var rect = GUILayoutUtility.GetRect (label, _linkUrlStyle, options);
            rect.x += indent * _indentWidth;
            Handles.BeginGUI ();
            Handles.color = _linkUrlStyle.normal.textColor;
            Handles.DrawLine (new Vector3 (rect.xMin, rect.yMax), new Vector3 (rect.xMax, rect.yMax));
            Handles.color = Color.white;
            Handles.EndGUI ();
            EditorGUIUtility.AddCursorRect (rect, MouseCursor.Link);
            return GUI.Button (rect, label, _linkUrlStyle);
        }

        // ----------------------------------------------------------- 
    }

}

#endif // End of WCE_URP
