// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if WCE_URP
using System.Collections.Generic;
using MH.WaterCausticsModules.Effect;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace MH.WaterCausticsModules {

    namespace Effect {
        // ----------------------------------------------------------- Enum
        public enum TexChannel { RGB, R, G, B, A }
        public enum Method { AtOnce = 0, EachMesh = 1 }
        public enum NormalSrc { DepthTex = 0, NormalTex = 1 }
        public enum DebugMode { Normal = 1, Depth, Facing, Caustics, LightArea }
        public enum ListUsage { Ignore = 0, Valid = 1 }
    }

    [ExecuteAlways]
    [DisallowMultipleComponent]
    [HelpURL (Constant.URL_MANUAL)]
    [AddComponentMenu ("WaterCausticsModules/WaterCausticsEffect")]
    public class WaterCausticsEffect : MonoBehaviour {
        // ----------------------------------------------------------- Constant
        private Vector3 [] _texChannelVec = { new Vector3 (0, 1, 2), Vector3.zero, Vector3.one, Vector3.one * 2, Vector3.one * 3 };

        static internal readonly RenderPassEvent SYS_OPAQUE_TEX_EVENT = RenderPassEvent.AfterRenderingSkybox; // URP OpaqueTex描画タイミング
        static internal readonly RenderPassEvent RENDER_EVENT = SYS_OPAQUE_TEX_EVENT; // EachMesh描画タイミング
        static internal readonly int RENDER_EVENT_ADJ = -1; // EachMesh描画タイミング

        // ----------------------------------------------------------- SerializeField
        [SerializeField] private Method m_method = Method.AtOnce;
        [SerializeField] private NormalSrc m_normalSrc = NormalSrc.NormalTex; // for AtOnceMethod only
        [SerializeField] private bool m_debugInfo = false;
        [SerializeField] private DebugMode m_debugMode = DebugMode.Normal;
        [SerializeField] private uint m_renderLayerMask = 1; // for AtOnceMethod only
        [SerializeField] private LayerMask m_layerMask = ~0;
        [FormerlySerializedAs ("m_clipOutsideVolume")] [SerializeField] private bool m_clipOutside = true;
        [SerializeField] private bool m_useImageMask = true;
        [SerializeField] private Texture m_imageMaskTexture;
        [SerializeField] private Texture m_texture;
        [SerializeField] private TexChannel m_textureChannel;
        [SerializeField, Range (-180f, 180f)] private float m_textureRotation = 15f;
        [SerializeField] private Vector2 m_texRotSinCos = new Vector2 (0, 1);
        [SerializeField] private bool m_useRandomTiling;
        [SerializeField, Range (0, 1000)] private int m_tilingSeed = 0;
        [SerializeField, Range (0f, 0.2f)] private float m_tilingRotation = 0.02f;
        [SerializeField, Range (0.75f, 0.999f)] private float m_tilingHardness = 0.85f;
        [SerializeField, Min (0.0001f)] private float m_scale = 1f;
        [FormerlySerializedAs ("m_waterSurfaceY")] [SerializeField] private float m_surfaceY = 2f;
        [FormerlySerializedAs ("m_waterSurfaceAttenOffset")] [SerializeField, Min (0f)] private float m_surfFadeStart;
        [FormerlySerializedAs ("m_waterSurfaceAttenWide")] [SerializeField, Min (0f)] private float m_surfFadeEnd = 0.5f;
        [FormerlySerializedAs ("m_useDepthAtten")] [SerializeField] private bool m_useDepthFade = false;
        [SerializeField, Min (0f)] private float m_depthFadeStart = 0f;
        [FormerlySerializedAs ("m_depthAttenDepth")] [SerializeField, Min (0f)] private float m_depthFadeEnd = 50f;
        [SerializeField] private bool m_useDistanceFade = false;
        [SerializeField, Min (0f)] private float m_distanceFadeStart = 30f;
        [SerializeField, Min (0f)] private float m_distanceFadeEnd = 100f;
        [SerializeField, Min (0f)] private float m_intensity = 5f;
        [FormerlySerializedAs ("m_adjustMainLit")] [SerializeField, Range (0f, 10f)] private float m_mainLit = 1f;
        [FormerlySerializedAs ("m_adjustAddLit")] [SerializeField, Range (0f, 10f)] private float m_addLit = 1f;
        [SerializeField] float m_colorShiftU; // Legacy
        [SerializeField] float m_colorShiftV; // Legacy
        [SerializeField, Range (0f, 5f)] float m_colorShift = 0.6f;
        [SerializeField, Range (-180f, 180f)] float m_colorShiftDir = 120f;
        [SerializeField, Range (0f, 2f)] private float m_litSaturation = 0.2f;
        [FormerlySerializedAs ("m_multiplyOpaqueColor")] [SerializeField] private bool m_useMultiply; // Legacy
        [FormerlySerializedAs ("m_multiplyOpaqueIntensity")] [SerializeField, Range (0f, 1f)] private float m_multiply = 1f;
        [FormerlySerializedAs ("m_normalAttenIntensity")] [SerializeField, Range (0f, 1f)] private float m_normalAtten = 1f;
        [FormerlySerializedAs ("m_normalAttenPower")] [SerializeField, Range (1f, 8f)] private float m_normalAttenRate = 1.5f;
        [SerializeField, Range (0f, 1f)] private float m_transparentBackside = 0f;
        [SerializeField, Range (0f, 1f)] private float m_backsideShadow = 0f;
        [SerializeField] private bool m_receiveShadows = true;
        [SerializeField, Range (0f, 1f)] private float m_shadowIntensity = 1f;
        [FormerlySerializedAs ("m_useMainLight")] [SerializeField] private int m_version = Constant.WCE_VERSION_INT;
        [SerializeField] private bool m_useMainLit = true;
        [FormerlySerializedAs ("m_useAdditionalLights")] [SerializeField] private bool m_useAddLit = true;
        [FormerlySerializedAs ("m_syncWithShaderFunctions")] [SerializeField] private bool m_useCustomFunc = false;
        [SerializeField] private RenderPassEvent m_renderEvent = RENDER_EVENT;
        [SerializeField] private int m_renderEventAdjust = RENDER_EVENT_ADJ;
        [SerializeField, Range (0, 255)] private int m_stencilRef = 0;
        [SerializeField, Range (0, 255)] private int m_stencilReadMask = 255;
        [SerializeField, Range (0, 255)] private int m_stencilWriteMask = 255;
        [SerializeField] private CompareFunction m_stencilComp = CompareFunction.Always;
        [SerializeField] private StencilOp m_stencilPass = StencilOp.Keep;
        [SerializeField] private StencilOp m_stencilFail = StencilOp.Keep;
        [SerializeField] private StencilOp m_stencilZFail = StencilOp.Keep;
        [SerializeField] private CullMode m_cullMode = CullMode.Off;
        [SerializeField] private bool m_zWriteMode = false;
        [SerializeField] private CompareFunction m_zTestMode = CompareFunction.Equal;
        [SerializeField] private float m_depthOffsetFactor = 0;
        [SerializeField] private float m_depthOffsetUnits = 0;
        [SerializeField] private Shader m_shader;
        [SerializeField] private Texture m_noTexture;

        // ----------------------------------------------------------- private property
        private RenderPassEvent eventAdjusted => (RenderPassEvent) Mathf.Clamp ((int) m_renderEvent + m_renderEventAdjust, 0, 1000);
        private bool existOpaqueTex => (eventAdjusted > SYS_OPAQUE_TEX_EVENT);
        private bool useBlendMultiply => (m_multiply == 1f || (m_multiply > 0f && !existOpaqueTex));
        private float multiplyByTex => (m_multiply < 1f && existOpaqueTex) ? m_multiply : 0f;
        private float multiplyRaw => m_multiply;
        private Vector2 calcColorShiftVec () => dirToVec (m_colorShiftDir) * m_colorShift * 0.01f;
        private float finalMainLit => m_intensity * m_mainLit * (m_useMainLit ? 1f : 0f);
        private float finalAddLit => m_intensity * m_addLit * (m_useAddLit ? 1f : 0f);
        private bool isIntensityZero => (finalMainLit + finalAddLit == 0f);
        private bool useClipOutside => (m_method == Method.AtOnce || m_clipOutside);

        // ----------------------------------------------------------- Public property
        private void setValAndNeedSetMat<T> (ref T prop, T val) {
            prop = val;
            _needUpdateMat = true;
        }
        public Method method {
            get => m_method;
            set => setValAndNeedSetMat (ref m_method, value);
        }
        public NormalSrc normalSrc {
            get => m_normalSrc;
            set => setValAndNeedSetMat (ref m_normalSrc, value);
        }
        public bool debugInfo {
            get => m_debugInfo;
            set => setValAndNeedSetMat (ref m_debugInfo, value);
        }
        public DebugMode debugMode {
            get => m_debugMode;
            set => setValAndNeedSetMat (ref m_debugMode, value);
        }
        public uint renderingLayerMask {
            get => m_renderLayerMask;
            set => setValAndNeedSetMat (ref m_renderLayerMask, value);
        }
        public LayerMask layerMask {
            get => m_layerMask;
            set => m_layerMask = value;
        }
        public bool clipOutside {
            get => m_clipOutside;
            set => setValAndNeedSetMat (ref m_clipOutside, value);
        }
        public Texture texture {
            get => m_texture;
            set => setValAndNeedSetMat (ref m_texture, value);
        }
        public TexChannel textureChannel {
            get => m_textureChannel;
            set => setValAndNeedSetMat (ref m_textureChannel, value);
        }
        public float textureRotation {
            get => m_textureRotation;
            set {
                float rad = value * Mathf.Deg2Rad;
                m_texRotSinCos = new Vector2 (Mathf.Sin (rad), Mathf.Cos (rad));
                setValAndNeedSetMat (ref m_textureRotation, value);
            }
        }
        public bool useRandomTiling {
            get => m_useRandomTiling;
            set => setValAndNeedSetMat (ref m_useRandomTiling, value);
        }
        public int tilingSeed {
            get => m_tilingSeed;
            set => setValAndNeedSetMat (ref m_tilingSeed, Mathf.Max (0, value));
        }
        public float tilingRotation {
            get => m_tilingRotation;
            set => setValAndNeedSetMat (ref m_tilingRotation, Mathf.Clamp (value, 0f, 1f));
        }
        public float tilingHardness {
            get => m_tilingHardness;
            set => setValAndNeedSetMat (ref m_tilingHardness, Mathf.Clamp (value, 0.001f, 0.999f));
        }
        public float intensity {
            get => m_intensity;
            set => setValAndNeedSetMat (ref m_intensity, Mathf.Max (0f, value));
        }
        public float mainLightIntensity {
            get => m_mainLit;
            set => setValAndNeedSetMat (ref m_mainLit, Mathf.Max (0f, value));
        }
        public float additionalLightsIntensity {
            get => m_addLit;
            set => setValAndNeedSetMat (ref m_addLit, Mathf.Max (0f, value));
        }
        public float scale {
            get => m_scale;
            set => setValAndNeedSetMat (ref m_scale, Mathf.Max (0.0001f, value));
        }
        public float colorShift {
            get => m_colorShift;
            set => setValAndNeedSetMat (ref m_colorShift, Mathf.Max (0f, value));
        }
        public float colorShiftDirection {
            get => m_colorShiftDir;
            set => setValAndNeedSetMat (ref m_colorShiftDir, wrapAngle180 (value));
        }
        public float surfaceY {
            get => m_surfaceY;
            set => setValAndNeedSetMat (ref m_surfaceY, value);
        }
        public float surfaceFadeStart {
            get => m_surfFadeStart;
            set => setValAndNeedSetMat (ref m_surfFadeStart, Mathf.Max (0f, value));
        }
        public float surfaceFadeEnd {
            get => m_surfFadeEnd;
            set => setValAndNeedSetMat (ref m_surfFadeEnd, Mathf.Max (0f, value));
        }

        public bool useDepthFade {
            get => m_useDepthFade;
            set => setValAndNeedSetMat (ref m_useDepthFade, value);
        }
        public float depthAttenStart {
            get => m_depthFadeStart;
            set => setValAndNeedSetMat (ref m_depthFadeStart, Mathf.Max (0f, value));
        }
        public float depthAttenEnd {
            get => m_depthFadeEnd;
            set => setValAndNeedSetMat (ref m_depthFadeEnd, Mathf.Max (0f, value));
        }
        public bool useDistanceAtten {
            get => m_useDistanceFade;
            set => setValAndNeedSetMat (ref m_useDistanceFade, value);
        }
        public float distanceAttenStart {
            get => m_distanceFadeStart;
            set => setValAndNeedSetMat (ref m_distanceFadeStart, Mathf.Max (0f, value));
        }
        public float distanceAttenEnd {
            get => m_distanceFadeEnd;
            set => setValAndNeedSetMat (ref m_distanceFadeEnd, Mathf.Max (0f, value));
        }
        public float lightSaturation {
            get => m_litSaturation;
            set => setValAndNeedSetMat (ref m_litSaturation, Mathf.Max (0f, value));
        }
        public bool receiveShadows {
            get => m_receiveShadows;
            set => setValAndNeedSetMat (ref m_receiveShadows, value);
        }
        public float shadowIntensity {
            get => m_shadowIntensity;
            set => setValAndNeedSetMat (ref m_shadowIntensity, value);
        }
        public bool useMainLight {
            get => m_useMainLit;
            set => setValAndNeedSetMat (ref m_useMainLit, value);
        }
        public bool useAdditionalLights {
            get => m_useAddLit;
            set => setValAndNeedSetMat (ref m_useAddLit, value);
        }
        public float multiplyWithBaseColor {
            get => m_multiply;
            set => setValAndNeedSetMat (ref m_multiply, Mathf.Clamp (value, 0f, 1f));
        }
        public float normalAttenRate {
            get => m_normalAttenRate;
            set => setValAndNeedSetMat (ref m_normalAttenRate, Mathf.Clamp (value, 1f, 8f));
        }
        public float normalAtten {
            get => m_normalAtten;
            set => setValAndNeedSetMat (ref m_normalAtten, Mathf.Clamp (value, 0f, 1f));
        }
        public float TransparentBack {
            get => m_transparentBackside;
            set => setValAndNeedSetMat (ref m_transparentBackside, Mathf.Clamp (value, 0f, 1f));
        }
        public float backsideShadow {
            get => m_backsideShadow;
            set => setValAndNeedSetMat (ref m_backsideShadow, Mathf.Clamp (value, 0f, 1f));
        }
        public int stencilRef {
            get => m_stencilRef;
            set => setValAndNeedSetMat (ref m_stencilRef, Mathf.Clamp (value, 0, 255));
        }
        public int stencilReadMask {
            get => m_stencilReadMask;
            set => setValAndNeedSetMat (ref m_stencilReadMask, Mathf.Clamp (value, 0, 255));
        }
        public int stencilWriteMask {
            get => m_stencilWriteMask;
            set => setValAndNeedSetMat (ref m_stencilWriteMask, Mathf.Clamp (value, 0, 255));
        }
        public CompareFunction stencilComp {
            get => m_stencilComp;
            set => setValAndNeedSetMat (ref m_stencilComp, value);
        }

        public StencilOp stencilPass {
            get => m_stencilPass;
            set => setValAndNeedSetMat (ref m_stencilPass, value);
        }
        public StencilOp stencilFail {
            get => m_stencilFail;
            set => setValAndNeedSetMat (ref m_stencilFail, value);
        }
        public StencilOp stencilZFail {
            get => m_stencilZFail;
            set => setValAndNeedSetMat (ref m_stencilZFail, value);
        }
        public CullMode cullMode {
            get => m_cullMode;
            set => setValAndNeedSetMat (ref m_cullMode, value);
        }
        public bool zWriteMode {
            get => m_zWriteMode;
            set => setValAndNeedSetMat (ref m_zWriteMode, value);
        }
        public CompareFunction zTestMode {
            get => m_zTestMode;
            set => setValAndNeedSetMat (ref m_zTestMode, value);
        }
        public float depthOffsetFactor {
            get => m_depthOffsetFactor;
            set => setValAndNeedSetMat (ref m_depthOffsetFactor, value);
        }
        public float depthOffsetUnits {
            get => m_depthOffsetUnits;
            set => setValAndNeedSetMat (ref m_depthOffsetUnits, value);
        }
        public bool useImageMask {
            get => m_useImageMask;
            set => setValAndNeedSetMat (ref m_useImageMask, value);
        }
        public Texture imageMaskTexture {
            get => m_imageMaskTexture;
            set => setValAndNeedSetMat (ref m_imageMaskTexture, value);
        }
        public bool supportCustomFunction {
            get => m_useCustomFunc;
            set => m_useCustomFunc = value;
        }
        public RenderPassEvent renderEvent {
            get => m_renderEvent;
            set => setValAndNeedSetMat (ref m_renderEvent, value);
        }
        public int renderEventAdjust {
            get => m_renderEventAdjust;
            set => setValAndNeedSetMat (ref m_renderEventAdjust, value);
        }

        public void ForceApplyChanges () {
#if UNITY_EDITOR
            if (!isActiveAndEnabled) return;
#endif
            _needUpdateMat = false;
            updateMaterialValues ();
        }

        // ----------------------------------------------------------- Tools
        static private float round2Dec (float val) => (float) (System.Math.Round (val * 100f) * 0.01); // 小数点2位で丸め
        static private float round2Dec5 (float val) => (float) (System.Math.Round (val * 20f) * 0.05); // 小数点2位を0か5で丸め
        static private float wrapAngle180 (float angle) => Mathf.Repeat (angle + 180f, 360f) - 180f;
        static private Vector2 dirToVec (float dir) => new Vector2 (Mathf.Sin (dir * Mathf.Deg2Rad), Mathf.Cos (-dir * Mathf.Deg2Rad));
        static private float vecToDir (Vector2 v) => round2Dec5 (wrapAngle180 (Mathf.Atan2 (v.x, v.y) * Mathf.Rad2Deg));

        // ----------------------------------------------------------- Editor
#if UNITY_EDITOR
        internal void OnInspectorChanged () {
            if (isActiveAndEnabled)
                updateMaterialValues ();
        }

        private void onUndoCallback () {
            if (isActiveAndEnabled)
                updateMaterialValues ();
        }

        [ContextMenu ("Open Asset Manual")]
        private void OpenManualURL () {
            Application.OpenURL (Constant.URL_MANUAL);
        }
#endif

        // ----------------------------------------------------------- Init
        private void Reset () {
            // ※ Awakeの後に呼ばれる (Editorのみ Undo登録/SetDirty不要)
#if UNITY_EDITOR
            addFeatureIfNeedManage ();
#endif
            m_scale = Mathf.Clamp (Mathf.Max (transform.lossyScale.x, transform.lossyScale.z), 0.3f, 3f);
            m_useImageMask = (m_imageMaskTexture != null);
        }

        internal void VersionCheck () {
#if UNITY_EDITOR
            if (m_version < Constant.WCE_VERSION_INT) EditorUtility.SetDirty (this);
#endif
            const int VER_20000 = 20000;
            if (m_version < VER_20000) {
                // -- Ver1.2.2以前に作成されている場合、データをv2用にアップデート
                m_useMainLit = (m_version > 0); // m_version は元 m_useMainLight
                m_method = Method.EachMesh;
                if (m_useMultiply == true && m_multiply > 0f && m_multiply < 1f) {
                    m_renderEvent = RenderPassEvent.BeforeRenderingTransparents;
                    m_renderEventAdjust = 0;
                }
                if (m_useMultiply == false)
                    m_multiply = 0f;
                textureRotation = 0f;
                var v = new Vector2 (m_colorShiftU, m_colorShiftV);
                m_colorShift = round2Dec5 (v.magnitude);
                m_colorShiftDir = vecToDir (v);
                if (m_surfFadeStart != 0f) {
                    m_surfFadeEnd = m_surfFadeEnd + m_surfFadeStart;
                    m_surfFadeStart = Mathf.Abs (m_surfFadeStart);
                    m_surfFadeEnd = Mathf.Max (m_surfFadeEnd, m_surfFadeStart);
                }
                m_version = VER_20000; // ※最後に設定
            }
            if (m_version < Constant.WCE_VERSION_INT)
                m_version = Constant.WCE_VERSION_INT;
        }

        private void Awake () {
            VersionCheck ();
        }

        // ----------------------------------------------------------- 
        private void OnEnable () {
#if UNITY_EDITOR
            Undo.undoRedoPerformed += onUndoCallback;
#endif
            onEnableForPass ();
            _needUpdateMat = true;
        }

        private void OnDisable () {
#if UNITY_EDITOR
            Undo.undoRedoPerformed -= onUndoCallback;
#endif
            onDisableForPass ();
            onDisableForCF ();
        }

        private void OnDestroy () {
            onDestroyForAtOnce ();
            destroy (ref __mat);
        }

        private void destroy<T> (ref T o) where T : Object {
            if (o == null) return;
            destroy (o);
            o = null;
        }

        private void destroy (Object o) {
            if (o == null) return;
            if (Application.isPlaying)
                Destroy (o);
            else
                DestroyImmediate (o);
        }


        // ----------------------------------------------------------- Update
        private bool _needUpdateMat;

        private void Update () {
            updateForCF ();
        }

        private void LateUpdate () {
            // --- 設定変更時のみの設定
            if (_needUpdateMat || !Application.isPlaying) {
                _needUpdateMat = false;
                updateMaterialValues ();
            }

            // --- 毎フレームの設定
            if (m_method == Method.EachMesh) {
                // Each Mesh 方式
                if ((m_clipOutside || (m_useImageMask && m_imageMaskTexture)))
                    getMat ().SetMatrix (pID._WCE_WorldToObjMatrix, transform.worldToLocalMatrix);
            }

            // --- Sync with Custom Function
            lateUpdateForCF ();
        }


        // ----------------------------------------------------------- Property ID
        private class pID {
            readonly internal static int _WCE_CausticsTex = Shader.PropertyToID ("_WCE_CausticsTex");
            readonly internal static int _WCE_TexChannels = Shader.PropertyToID ("_WCE_TexChannels");
            readonly internal static int _WCE_TexRotateSinCos = Shader.PropertyToID ("_WCE_TexRotateSinCos");
            readonly internal static int _WCE_TilingSeed = Shader.PropertyToID ("_WCE_TilingSeed");
            readonly internal static int _WCE_TilingRot = Shader.PropertyToID ("_WCE_TilingRot");
            readonly internal static int _WCE_TilingHard = Shader.PropertyToID ("_WCE_TilingHard");
            readonly internal static int _WCE_IntensityMainLit = Shader.PropertyToID ("_WCE_IntensityMainLit");
            readonly internal static int _WCE_IntensityAddLit = Shader.PropertyToID ("_WCE_IntensityAddLit");
            readonly internal static int _WCE_Density = Shader.PropertyToID ("_WCE_Density");
            readonly internal static int _WCE_ColorShift = Shader.PropertyToID ("_WCE_ColorShift");
            readonly internal static int _WCE_SurfaceY = Shader.PropertyToID ("_WCE_SurfaceY");
            readonly internal static int _WCE_SurfFadeStart = Shader.PropertyToID ("_WCE_SurfFadeStart");
            readonly internal static int _WCE_SurfFadeCoef = Shader.PropertyToID ("_WCE_SurfFadeCoef");
            readonly internal static int _WCE_DepthFadeStart = Shader.PropertyToID ("_WCE_DepthFadeStart");
            readonly internal static int _WCE_DepthFadeCoef = Shader.PropertyToID ("_WCE_DepthFadeCoef");
            readonly internal static int _WCE_DistanceFadeStart = Shader.PropertyToID ("_WCE_DistanceFadeStart");
            readonly internal static int _WCE_DistanceFadeCoef = Shader.PropertyToID ("_WCE_DistanceFadeCoef");
            readonly internal static int _WCE_LitSaturation = Shader.PropertyToID ("_WCE_LitSaturation");
            readonly internal static int _WCE_MultiplyByTex = Shader.PropertyToID ("_WCE_MultiplyByTex");
            readonly internal static int _WCE_NormalAtten = Shader.PropertyToID ("_WCE_NormalAtten");
            readonly internal static int _WCE_NormalAttenRate = Shader.PropertyToID ("_WCE_NormalAttenRate");
            readonly internal static int _WCE_TransparentBack = Shader.PropertyToID ("_WCE_TransparentBack");
            readonly internal static int _WCE_BacksideShadow = Shader.PropertyToID ("_WCE_BacksideShadow");
            readonly internal static int _WCE_ShadowIntensity = Shader.PropertyToID ("_WCE_ShadowIntensity");
            readonly internal static int _WCE_ImageMaskTex = Shader.PropertyToID ("_WCE_ImageMaskTex");
            readonly internal static int _WCE_WorldToObjMatrix = Shader.PropertyToID ("_WCE_WorldToObjMatrix");
            readonly internal static int _WCE_ClipOutside = Shader.PropertyToID ("_WCE_ClipOutside");
            readonly internal static int _WCE_UseImageMask = Shader.PropertyToID ("_WCE_UseImageMask");

            readonly internal static int _StencilRef = Shader.PropertyToID ("_StencilRef");
            readonly internal static int _StencilReadMask = Shader.PropertyToID ("_StencilReadMask");
            readonly internal static int _StencilWriteMask = Shader.PropertyToID ("_StencilWriteMask");
            readonly internal static int _StencilComp = Shader.PropertyToID ("_StencilComp");
            readonly internal static int _StencilPass = Shader.PropertyToID ("_StencilPass");
            readonly internal static int _StencilFail = Shader.PropertyToID ("_StencilFail");
            readonly internal static int _StencilZFail = Shader.PropertyToID ("_StencilZFail");
            readonly internal static int _CullMode = Shader.PropertyToID ("_CullMode");
            readonly internal static int _ZWrite = Shader.PropertyToID ("_ZWrite");
            readonly internal static int _ZTest = Shader.PropertyToID ("_ZTest");
            readonly internal static int _OffsetFactor = Shader.PropertyToID ("_OffsetFactor");
            readonly internal static int _OffsetUnits = Shader.PropertyToID ("_OffsetUnits");
            readonly internal static int _BlendSrcFactor = Shader.PropertyToID ("_BlendSrcFactor");
            readonly internal static int _BlendDstFactor = Shader.PropertyToID ("_BlendDstFactor");

            readonly internal static int _WCECF_TexChannels = Shader.PropertyToID ("_WCECF_TexChannels");
            readonly internal static int _WCECF_TexRotateSinCos = Shader.PropertyToID ("_WCECF_TexRotateSinCos");
            readonly internal static int _WCECF_TilingSeed = Shader.PropertyToID ("_WCECF_TilingSeed");
            readonly internal static int _WCECF_TilingRot = Shader.PropertyToID ("_WCECF_TilingRot");
            readonly internal static int _WCECF_TilingHard = Shader.PropertyToID ("_WCECF_TilingHard");
            readonly internal static int _WCECF_IntensityMainLit = Shader.PropertyToID ("_WCECF_IntensityMainLit");
            readonly internal static int _WCECF_Density = Shader.PropertyToID ("_WCECF_Density");
            readonly internal static int _WCECF_ColorShift = Shader.PropertyToID ("_WCECF_ColorShift");
            readonly internal static int _WCECF_SurfaceY = Shader.PropertyToID ("_WCECF_SurfaceY");
            readonly internal static int _WCECF_SurfFadeStart = Shader.PropertyToID ("_WCECF_SurfFadeStart");
            readonly internal static int _WCECF_SurfFadeCoef = Shader.PropertyToID ("_WCECF_SurfFadeCoef");
            readonly internal static int _WCECF_DepthFadeStart = Shader.PropertyToID ("_WCECF_DepthFadeStart");
            readonly internal static int _WCECF_DepthFadeCoef = Shader.PropertyToID ("_WCECF_DepthFadeCoef");
            readonly internal static int _WCECF_DistanceFadeStart = Shader.PropertyToID ("_WCECF_DistanceFadeStart");
            readonly internal static int _WCECF_DistanceFadeCoef = Shader.PropertyToID ("_WCECF_DistanceFadeCoef");
            readonly internal static int _WCECF_LitSaturation = Shader.PropertyToID ("_WCECF_LitSaturation");
            readonly internal static int _WCECF_IntensityAddLit = Shader.PropertyToID ("_WCECF_IntensityAddLit");
            readonly internal static int _WCECF_MultiplyIntensity = Shader.PropertyToID ("_WCECF_MultiplyIntensity");
            readonly internal static int _WCECF_NormalAttenRate = Shader.PropertyToID ("_WCECF_NormalAttenRate");
            readonly internal static int _WCECF_NormalAtten = Shader.PropertyToID ("_WCECF_NormalAtten");
            readonly internal static int _WCECF_TransparentBack = Shader.PropertyToID ("_WCECF_TransparentBack");
            readonly internal static int _WCECF_BacksideShadow = Shader.PropertyToID ("_WCECF_BacksideShadow");
            readonly internal static int _WCECF_ShadowIntensity = Shader.PropertyToID ("_WCECF_ShadowIntensity");
            readonly internal static int _WCECF_WorldToObjMatrix = Shader.PropertyToID ("_WCECF_WorldToObjMatrix");
            readonly internal static int _WCECF_ClipOutside = Shader.PropertyToID ("_WCECF_ClipOutside");
            readonly internal static int _WCECF_UseImageMask = Shader.PropertyToID ("_WCECF_UseImageMask");
            readonly internal static int _WCECF_CausticsTex = Shader.PropertyToID ("_WCECF_CausticsTex");
            readonly internal static int _WCECF_ImageMaskTex = Shader.PropertyToID ("_WCECF_ImageMaskTex");
        }


        // ----------------------------------------------------------- Material
        private Material __mat;
        private Material getMat () {
            return __mat ? __mat : __mat = createMat (m_shader, "WCausticsEffect");
        }

        private Material createMat (Shader shader, string name) {
            _needUpdateMat = true;
            if (shader == null) {
                Debug.LogError ("Shader is null. " + this);
                return null;
            } else {
                Material mat = new Material (shader);
                mat.name = name;
                mat.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
                return mat;
            }
        }

        private float calcFadeCoef (float depth) => 1f / Mathf.Max (depth, 0.0000001f);

        private void updateMaterialValues () {
            Material mat = getMat ();
            if (mat == null) return;
            mat.SetVector (pID._WCE_TexChannels, _texChannelVec [(int) m_textureChannel]);
            mat.SetVector (pID._WCE_TexRotateSinCos, m_texRotSinCos);
            mat.SetInt (pID._WCE_TilingSeed, m_useRandomTiling ? m_tilingSeed : -1);
            mat.SetFloat (pID._WCE_TilingRot, m_tilingRotation);
            mat.SetFloat (pID._WCE_TilingHard, m_tilingHardness);
            mat.SetFloat (pID._WCE_IntensityMainLit, finalMainLit);
            mat.SetFloat (pID._WCE_IntensityAddLit, finalAddLit);
            mat.SetVector (pID._WCE_ColorShift, calcColorShiftVec ());
            mat.SetFloat (pID._WCE_SurfaceY, m_surfaceY);
            mat.SetFloat (pID._WCE_SurfFadeStart, m_surfFadeStart);
            mat.SetFloat (pID._WCE_SurfFadeCoef, calcFadeCoef (m_surfFadeEnd - m_surfFadeStart));
            mat.SetFloat (pID._WCE_DepthFadeStart, m_depthFadeStart);
            mat.SetFloat (pID._WCE_DepthFadeCoef, m_useDepthFade ? calcFadeCoef (m_depthFadeEnd - m_depthFadeStart) : 0f);
            mat.SetFloat (pID._WCE_DistanceFadeStart, m_distanceFadeStart);
            mat.SetFloat (pID._WCE_DistanceFadeCoef, m_useDistanceFade ? calcFadeCoef (m_distanceFadeEnd - m_distanceFadeStart) : 0f);
            mat.SetFloat (pID._WCE_LitSaturation, m_litSaturation);
            mat.SetFloat (pID._WCE_NormalAttenRate, m_normalAttenRate);
            mat.SetFloat (pID._WCE_NormalAtten, m_normalAtten);
            mat.SetFloat (pID._WCE_TransparentBack, m_transparentBackside);
            mat.SetFloat (pID._WCE_BacksideShadow, m_backsideShadow);
            mat.SetInt (pID._WCE_ClipOutside, System.Convert.ToInt32 (useClipOutside));

            bool isNoTex = (m_texture == null);
            mat.SetFloat (pID._WCE_Density, 1f / Mathf.Max (isNoTex ? m_scale * 0.333f : m_scale, 0.0001f));
            mat.SetTexture (pID._WCE_CausticsTex, isNoTex ? m_noTexture : m_texture);
            bool useImgMask = (m_useImageMask && m_imageMaskTexture);
            mat.SetInt (pID._WCE_UseImageMask, System.Convert.ToInt32 (useImgMask));
            mat.SetTexture (pID._WCE_ImageMaskTex, useImgMask ? m_imageMaskTexture : null);
            bool useShadow = m_receiveShadows && (m_shadowIntensity > 0f || (m_backsideShadow > 0f && (m_normalAtten < 1f || m_transparentBackside > 0f)));
            setMatKeyword (mat, !useShadow, "_RECEIVE_SHADOWS_OFF");
            mat.SetFloat (pID._WCE_ShadowIntensity, m_shadowIntensity);
            mat.SetFloat (pID._WCE_MultiplyByTex, multiplyByTex);

            mat.SetInt (pID._BlendSrcFactor, (int) (useBlendMultiply ? BlendMode.DstColor : BlendMode.One));
            mat.SetInt (pID._BlendDstFactor, (int) BlendMode.One);
            mat.SetInt (pID._StencilRef, m_stencilRef);
            mat.SetInt (pID._StencilReadMask, m_stencilReadMask);
            mat.SetInt (pID._StencilWriteMask, m_stencilWriteMask);
            mat.SetInt (pID._StencilComp, (int) m_stencilComp);
            mat.SetInt (pID._StencilPass, (int) m_stencilPass);
            mat.SetInt (pID._StencilFail, (int) m_stencilFail);
            mat.SetInt (pID._StencilZFail, (int) m_stencilZFail);

            if (m_method == Method.EachMesh) {
                // [EachMesh]
                mat.SetInt (pID._CullMode, (int) m_cullMode);
                mat.SetInt (pID._ZWrite, System.Convert.ToInt32 (m_zWriteMode));
                mat.SetInt (pID._ZTest, (int) m_zTestMode);
                mat.SetFloat (pID._OffsetFactor, m_depthOffsetFactor);
                mat.SetFloat (pID._OffsetUnits, m_depthOffsetUnits);
                mat.DisableKeyword ("_WCE_ONE_PASS_NORMAL");
                mat.DisableKeyword ("_WCE_ONE_PASS_DEPTH");
                mat.renderQueue = -1;
            } else {
                // [AtOnce]
                mat.SetInt (pID._CullMode, (int) CullMode.Back);
                mat.SetInt (pID._ZWrite, 0);
                mat.SetInt (pID._ZTest, (int) CompareFunction.LessEqual);
                mat.SetFloat (pID._OffsetFactor, 0f);
                mat.SetFloat (pID._OffsetUnits, 0f);
                mat.renderQueue = (int) eventAdjusted;
                setMatKeyword (mat, m_normalSrc == NormalSrc.DepthTex, "_WCE_ONE_PASS_DEPTH");
                setMatKeyword (mat, m_normalSrc != NormalSrc.DepthTex, "_WCE_ONE_PASS_NORMAL");
                prepAtOnce ();
                _atOnce.render.renderingLayerMask = m_renderLayerMask;
                _atOnce.render.forceRenderingOff = isIntensityZero;
            }
#if UNITY_EDITOR
            // ----- for Debug Info Rendering
            bool isDebugNormal = (m_debugInfo && m_debugMode == DebugMode.Normal);
            bool isDebugDepth = (m_debugInfo && m_debugMode == DebugMode.Depth);
            bool isDebugNormalErr = (m_debugInfo && m_debugMode == DebugMode.Facing);
            bool isDebugCaustics = (m_debugInfo && m_debugMode == DebugMode.Caustics);
            bool isDebugLitArea = (m_debugInfo && m_debugMode == DebugMode.LightArea);
            setMatKeyword (mat, isDebugNormal, "WCE_DEBUG_NORMAL");
            setMatKeyword (mat, isDebugDepth, "WCE_DEBUG_DEPTH");
            setMatKeyword (mat, isDebugNormalErr, "WCE_DEBUG_FACING");
            setMatKeyword (mat, isDebugCaustics, "WCE_DEBUG_CAUSTICS");
            setMatKeyword (mat, isDebugLitArea, "WCE_DEBUG_AREA");
            if (isDebugNormal || isDebugDepth || isDebugNormalErr || isDebugCaustics || isDebugLitArea) {
                mat.SetInt (pID._BlendSrcFactor, (int) BlendMode.One);
                mat.SetInt (pID._BlendDstFactor, (int) BlendMode.Zero);
            }
            if (isDebugCaustics) {
                float m = Mathf.Lerp (1f, 0.5f, multiplyRaw);
                mat.SetFloat (pID._WCE_IntensityMainLit, finalMainLit * m);
                mat.SetFloat (pID._WCE_IntensityAddLit, finalAddLit * m);
            }
#endif
        }

        private void setMatKeyword (Material mat, bool isEnable, string keyword) {
            if (isEnable)
                mat.EnableKeyword (keyword);
            else
                mat.DisableKeyword (keyword);
        }


        // ----------------------------------------------------------- Sync with Custom Functions
        static private WaterCausticsEffect s_nextSync, s_lastSync;
        private void onDisableForCF () {
            if (s_lastSync == this) {
                s_lastSync = null;
                setCFIntensityZero ();
            }
        }
        private void updateForCF () {
            if (m_useCustomFunc) {
                s_nextSync = this; // 最後にupdateが呼ばれたエフェクトが担当となる
            } else if (s_lastSync == this) {
                s_lastSync = null;
                setCFIntensityZero ();
            }
        }

        private void lateUpdateForCF () {
            if (m_useCustomFunc && s_nextSync == this) {
                s_nextSync = null;
                s_lastSync = this;
                setCustomFunc ();
            }
        }

        private void setCustomFunc () {
            setCFIntensity ();
            bool isNoTex = (m_texture == null);
            Shader.SetGlobalFloat (pID._WCECF_Density, 1f / Mathf.Max (isNoTex ? m_scale * 0.333f : m_scale, 0.0001f));
            Shader.SetGlobalFloat (pID._WCECF_SurfaceY, m_surfaceY);
            Shader.SetGlobalFloat (pID._WCECF_SurfFadeStart, m_surfFadeStart);
            Shader.SetGlobalFloat (pID._WCECF_SurfFadeCoef, calcFadeCoef (m_surfFadeEnd - m_surfFadeStart));
            Shader.SetGlobalFloat (pID._WCECF_DepthFadeStart, m_depthFadeStart);
            Shader.SetGlobalFloat (pID._WCECF_DepthFadeCoef, m_useDepthFade ? calcFadeCoef (m_depthFadeEnd - m_depthFadeStart) : 0f);
            Shader.SetGlobalFloat (pID._WCECF_DistanceFadeStart, m_distanceFadeStart);
            Shader.SetGlobalFloat (pID._WCECF_DistanceFadeCoef, m_useDistanceFade ? calcFadeCoef (m_distanceFadeEnd - m_distanceFadeStart) : 0f);
            Shader.SetGlobalVector (pID._WCECF_ColorShift, calcColorShiftVec ());
            Shader.SetGlobalFloat (pID._WCECF_LitSaturation, m_litSaturation);
            Shader.SetGlobalFloat (pID._WCECF_ShadowIntensity, m_shadowIntensity);
            Shader.SetGlobalFloat (pID._WCECF_MultiplyIntensity, multiplyRaw);
            Shader.SetGlobalFloat (pID._WCECF_NormalAttenRate, m_normalAttenRate);
            Shader.SetGlobalFloat (pID._WCECF_NormalAtten, m_normalAtten);
            Shader.SetGlobalFloat (pID._WCECF_TransparentBack, m_transparentBackside);
            Shader.SetGlobalFloat (pID._WCECF_BacksideShadow, m_backsideShadow);
            Shader.SetGlobalMatrix (pID._WCECF_WorldToObjMatrix, transform.worldToLocalMatrix);
            Shader.SetGlobalInt (pID._WCECF_ClipOutside, System.Convert.ToInt32 (useClipOutside));
            Shader.SetGlobalTexture (pID._WCECF_CausticsTex, isNoTex ? m_noTexture : m_texture);
            Shader.SetGlobalVector (pID._WCECF_TexChannels, _texChannelVec [(int) m_textureChannel]);
            Shader.SetGlobalVector (pID._WCECF_TexRotateSinCos, m_texRotSinCos);
            Shader.SetGlobalInt (pID._WCECF_TilingSeed, m_useRandomTiling ? m_tilingSeed : -1);
            Shader.SetGlobalFloat (pID._WCECF_TilingRot, m_tilingRotation);
            Shader.SetGlobalFloat (pID._WCECF_TilingHard, m_tilingHardness);
            bool useImgMask = (m_useImageMask && m_imageMaskTexture);
            Shader.SetGlobalInt (pID._WCECF_UseImageMask, System.Convert.ToInt32 (useImgMask));
            Shader.SetGlobalTexture (pID._WCECF_ImageMaskTex, useImgMask ? m_imageMaskTexture : null);
        }

        private void setCFIntensityZero () {
            Shader.SetGlobalFloat (pID._WCECF_IntensityMainLit, 0f);
            Shader.SetGlobalFloat (pID._WCECF_IntensityAddLit, 0f);
        }

        private void setCFIntensity () {
            Shader.SetGlobalFloat (pID._WCECF_IntensityMainLit, finalMainLit);
            Shader.SetGlobalFloat (pID._WCECF_IntensityAddLit, finalAddLit);
        }

        private void tellCustomFuncNeedDraw (bool needDraw) {
            if (!m_useCustomFunc || s_lastSync != this) return;
            if (needDraw)
                setCFIntensity ();
            else
                setCFIntensityZero ();
        }

        // ----------------------------------------------------------- Enqueue RenderPass
#if UNITY_EDITOR
        static private readonly string tempFilePath = "Temp/WCEInitialized";
        [InitializeOnLoadMethod]
        static private void registerFeature () {
            // エディタ起動時、アセットインポート時
            EditorApplication.delayCall += () => {
                if (!File.Exists (tempFilePath)) {
                    File.Create (tempFilePath);
                    addFeatureIfNeedManage ();
                }
            };
            // シーンを開いた時
            EditorSceneManager.sceneOpened += (Scene scene, OpenSceneMode mode) => {
                if (mode == OpenSceneMode.Single) addFeatureIfNeedManage ();
            };
            // プレイ開始時
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {
                if (state == PlayModeStateChange.EnteredPlayMode) addFeatureIfNeedManage ();
            };
        }

        static private void addFeatureIfNeedManage () {
            // -- 自動管理がOnの場合にRendererFeatureを追加
            if (WaterCausticsEffectData.GetAsset ().AutoManageFeature)
                WaterCausticsEffectFeatureEditor.AddFeatureToAllRenderers (useUndo: false);
        }
#endif

        private void onEnableForPass () {
            WaterCausticsEffectFeature.onEnqueue -= enqueuePass;
            WaterCausticsEffectFeature.onEnqueue += enqueuePass;
        }

        private void onDisableForPass () {
            WaterCausticsEffectFeature.onEnqueue -= enqueuePass;
        }

        private WCEEachMeshPass _eachMeshPass;
        private WCEAtOncePass _atOncePass;
        private void enqueuePass (ScriptableRenderer renderer, Camera cam) {
            if (isIntensityZero) return;

            bool needDraw = checkNeedDraw (cam);
            tellCustomFuncNeedDraw (needDraw);
            if (!needDraw) return;

            if (m_method == Method.EachMesh) {
                // [EachMesh]
                if (m_layerMask == 0) return;
                if (_eachMeshPass == null) _eachMeshPass = new WCEEachMeshPass ();
                bool useOpaqueTex = multiplyByTex > 0f;
                _eachMeshPass.Setup (eventAdjusted, getMat (), m_layerMask, useOpaqueTex);
                renderer.EnqueuePass (_eachMeshPass);
            } else {
                // [AtOnce]
                prepAtOnce ();
                if (_atOncePass == null) _atOncePass = new WCEAtOncePass ();
                bool useNormalTex = (m_normalSrc == NormalSrc.NormalTex);
                bool useOpaqueTex = multiplyByTex > 0f;
                _atOncePass.Setup (eventAdjusted, useNormalTex, useOpaqueTex);
                renderer.EnqueuePass (_atOncePass);
            }
        }

        // ----------------------------------------------------------- カメラ毎に描画するかどうか判別
        readonly Vector3 [] _points = { new Vector3 (-.5f, -.5f, -.5f), new Vector3 (.5f, -.5f, -.5f), new Vector3 (-.5f, .5f, -.5f), new Vector3 (.5f, .5f, -.5f), new Vector3 (-.5f, -.5f, .5f), new Vector3 (.5f, -.5f, .5f), new Vector3 (-.5f, .5f, .5f), new Vector3 (.5f, .5f, .5f), };
        private Plane [] _planes = new Plane [6];
        private bool insightCheck (Camera cam) {
            Bounds bounds = GeometryUtility.CalculateBounds (_points, transform.localToWorldMatrix);
            GeometryUtility.CalculateFrustumPlanes (cam, _planes);
            return GeometryUtility.TestPlanesAABB (_planes, bounds);
        }

        private bool checkNeedDraw (Camera cam) {
#if UNITY_EDITOR
            if (cam.cameraType == CameraType.Preview) return false;
            if (cam.cameraType == CameraType.SceneView && !UnityEditor.SceneView.currentDrawingSceneView.sceneLighting) return false;
#endif
            // レイヤーマスクチェック
            if (((1 << gameObject.layer) & cam.cullingMask) == 0) return false;
            // 視界内チェック
            if (useClipOutside && !insightCheck (cam)) return false;
            return true;
        }

        // ----------------------------------------------------------- AtOnce Method
        private AtOnce _atOnce;
        internal AtOnce atOnce => _atOnce;

        private void onDestroyForAtOnce () {
            AtOnce.OnSummonerDestroyed (ref _atOnce);
        }

        private void prepAtOnce () {
            if (!_atOnce) _atOnce = AtOnce.Create (this, getMat ());
        }

        // ----------------------------------------------------------- ScriptableRenderPass AtOnce
        internal class WCEAtOncePass : ScriptableRenderPass {
            private ShaderTagId _shaderTagId;
            private FilteringSettings _filteringSettings;
            private bool _useNormalTex, _useOpaqueTex;
            static private HashSet<RenderPassEvent> s_hashSet = new HashSet<RenderPassEvent> ();

            private RenderPassEvent __evt = (RenderPassEvent) 9999;
            private void setRenderEvt (RenderPassEvent v) {
                if (__evt != v) {
                    _filteringSettings = new FilteringSettings (new RenderQueueRange ((int) v, (int) v), layerMask: ~0);
                    this.renderPassEvent = __evt = v;
                }
            }

            internal WCEAtOncePass () {
                base.profilingSampler = new ProfilingSampler (nameof (WCEAtOncePass));
                _shaderTagId = new ShaderTagId ("WCE_EffectPass");
            }

            internal void Setup (RenderPassEvent evt, bool useNormalTex, bool useOpaqueTex) {
                setRenderEvt (evt);
                _useNormalTex = useNormalTex;
                _useOpaqueTex = useOpaqueTex;
            }

            public override void OnCameraSetup (CommandBuffer cmd, ref RenderingData rendData) {
                s_hashSet.Add (this.renderPassEvent);
                ScriptableRenderPassInput request = ScriptableRenderPassInput.Depth;
                if (_useNormalTex) request |= ScriptableRenderPassInput.Normal;
                if (_useOpaqueTex) request |= ScriptableRenderPassInput.Color;
                ConfigureInput (request);
            }

            public override void Execute (ScriptableRenderContext context, ref RenderingData rendData) {
                // ※ renderPassEvent値でまとめて描画
                if (!s_hashSet.Contains (this.renderPassEvent)) return;
                s_hashSet.Remove (this.renderPassEvent);

                DrawingSettings drawingSettings = CreateDrawingSettings (_shaderTagId, ref rendData, SortingCriteria.None);
                CommandBuffer cmd = CommandBufferPool.Get ();
                using (new ProfilingScope (cmd, base.profilingSampler)) {
                    context.ExecuteCommandBuffer (cmd);
                    cmd.Clear ();
                    context.DrawRenderers (rendData.cullResults, ref drawingSettings, ref _filteringSettings);
                }
                context.ExecuteCommandBuffer (cmd);
                CommandBufferPool.Release (cmd);
            }

            public override void OnCameraCleanup (CommandBuffer cmd) {
                s_hashSet.Clear ();
            }
        }

        // ----------------------------------------------------------- ScriptableRenderPass EachMesh Effect
        internal class WCEEachMeshPass : ScriptableRenderPass {
            private FilteringSettings _filteringSettings;
            private Material _mat;
            private List<ShaderTagId> _shaderTagIdList;
            private bool _useOpaqueTex;

            private LayerMask __layerMask = -2;
            private LayerMask layerMask {
                get => __layerMask;
                set {
                    if (__layerMask == value) return;
                    __layerMask = value;
                    _filteringSettings = new FilteringSettings (RenderQueueRange.opaque, value);
                }
            }

            internal WCEEachMeshPass () {
                base.profilingSampler = new ProfilingSampler (nameof (WCEEachMeshPass));
                _shaderTagIdList = new List<ShaderTagId> () {
                    new ShaderTagId ("SRPDefaultUnlit"),
                    new ShaderTagId ("UniversalForward"),
                    new ShaderTagId ("UniversalForwardOnly"),
                    new ShaderTagId ("LightweightForward"),
                };
            }

            internal void Setup (RenderPassEvent evt, Material mat, LayerMask layerMask, bool useOpaqueTex) {
                this.renderPassEvent = evt;
                this.layerMask = layerMask;
                _mat = mat;
                _useOpaqueTex = useOpaqueTex;
            }

            public override void OnCameraSetup (CommandBuffer cmd, ref RenderingData rendData) {
                if (_useOpaqueTex)
                    ConfigureInput (ScriptableRenderPassInput.Color);
            }

            public override void Execute (ScriptableRenderContext context, ref RenderingData rendData) {
                if (!_mat) return;
                DrawingSettings drawingSettings = CreateDrawingSettings (_shaderTagIdList, ref rendData, SortingCriteria.None);
                drawingSettings.overrideMaterial = _mat;
                drawingSettings.overrideMaterialPassIndex = 0;
                CommandBuffer cmd = CommandBufferPool.Get ();
                using (new ProfilingScope (cmd, base.profilingSampler)) {
                    context.ExecuteCommandBuffer (cmd);
                    cmd.Clear ();
                    context.DrawRenderers (rendData.cullResults, ref drawingSettings, ref _filteringSettings);
                }
                context.ExecuteCommandBuffer (cmd);
                CommandBufferPool.Release (cmd);
            }
        }

        // ----------------------------------------------------------- Gizmo
#if UNITY_EDITOR
        private void OnDrawGizmosSelected () {
            if (Selection.gameObjects.Length != 1 || Selection.activeGameObject != gameObject) {
                drawGizmoAACube (1f, new Color (0.8f, 0.4f, 0.0f, 0.5f), true);
            } else {
                drawGizmoAACube (2f, new Color (0.8f, 0.4f, 0.0f, 1f), true);
                drawGizmoAACube (2f, new Color (0.8f, 0.4f, 0.0f, 0.08f), false);
                Gizmos.color = new Color (0.8f, 0.4f, 0.0f, 0.6f);
                Vector3 surfYPos = transform.position + Vector3.up * (m_surfaceY - transform.position.y);
                var tmp = Gizmos.matrix;
                float alpha = 0.5f;
                Gizmos.matrix = Matrix4x4.TRS (surfYPos, Quaternion.Euler (0f, m_textureRotation, 0f), new Vector3 (m_scale, 1f, m_scale));
                if (m_surfFadeEnd > 0f) {
                    Gizmos.color = new Color (0f, 1f, 0.35f, 0.35f * alpha);
                    Gizmos.DrawWireCube (Vector3.up * m_surfFadeEnd, new Vector3 (1, 0, 1));
                    Gizmos.DrawWireCube (Vector3.up * -m_surfFadeEnd, new Vector3 (1, 0, 1));
                    if (m_surfFadeStart > 0f) {
                        Gizmos.color = new Color (0f, 1f, 0.35f, 0.35f * alpha);
                        Gizmos.DrawWireCube (Vector3.up * m_surfFadeStart, new Vector3 (1, 0, 1));
                        Gizmos.DrawWireCube (Vector3.up * -m_surfFadeStart, new Vector3 (1, 0, 1));
                    }
                }
                if (m_useDepthFade) {
                    Gizmos.color = new Color (1f, 0.35f, 0.6f, 0.45f * alpha);
                    Gizmos.DrawWireCube (Vector3.up * (m_depthFadeEnd), new Vector3 (1, 0, 1));
                    Gizmos.DrawWireCube (Vector3.up * (-m_depthFadeEnd), new Vector3 (1, 0, 1));
                    if (m_depthFadeStart > 0f) {
                        Gizmos.color = new Color (1f, 0.35f, 0.6f, 0.45f * alpha);
                        Gizmos.DrawWireCube (Vector3.up * m_depthFadeStart, new Vector3 (1, 0, 1));
                        Gizmos.DrawWireCube (Vector3.up * -m_depthFadeStart, new Vector3 (1, 0, 1));
                    }
                }
                Gizmos.color = new Color (0.3f, 0.76f, 1f, 0.65f * alpha);
                Gizmos.DrawWireCube (Vector3.zero, new Vector3 (1, 0, 1));
                Gizmos.color = new Color (0.3f, 0.76f, 1f, 1.3f * alpha);
                Gizmos.DrawLine (Vector3.forward * 0.48f, Vector3.forward * 0.4f + Vector3.right * 0.03f);
                Gizmos.DrawLine (Vector3.forward * 0.48f, Vector3.forward * 0.4f - Vector3.right * 0.03f);
                Gizmos.DrawLine (Vector3.forward * 0.4f + Vector3.right * 0.03f, Vector3.forward * 0.4f - Vector3.right * 0.03f);
                Gizmos.matrix = tmp;
            }
        }

        private void drawGizmoAACube (float width, Color color, bool zTest) {
            void drawQuart (float rot) {
                Handles.matrix = transform.localToWorldMatrix * Matrix4x4.Rotate (Quaternion.Euler (rot, 0f, 0f));
                Handles.DrawAAPolyLine (Texture2D.whiteTexture, width, new Vector3 (-.5f, -.5f, -.5f), new Vector3 (-.5f, .5f, -.5f), new Vector3 (.5f, .5f, -.5f), new Vector3 (.5f, -.5f, -.5f));
            }
            Handles.color = color;
            Handles.zTest = zTest? CompareFunction.LessEqual : CompareFunction.Greater;
            var tmp = Handles.matrix;
            for (int i = 0; i < 4; i++) drawQuart (i * 90f);
            Handles.matrix = tmp;
        }
#endif // End of UNITY_EDITOR
    }
}

#endif // End of WCE_URP
