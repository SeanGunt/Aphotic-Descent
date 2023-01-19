// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using System.Collections.Generic;
using MH.WaterCausticsModules.TexGen;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MH.WaterCausticsModules {
    namespace TexGen {
        // ----------------------------------------------------------- Enum
        public enum Style { StyleA, StyleB, StyleC, }
        public enum RefractedRay { Normalize, Extend, }
        public enum MSAASamples { None = 1, MSAA2x = 2, MSAA4x = 4, MSAA8x = 8 }
        public enum LightRay { Vector = 0, Transform, LitSettingSun, Auto, Direction, }
        public enum CalcRes { x64 = 64, x96 = 96, x128 = 128, x160 = 160, x256 = 256, x320 = 320, x512 = 512, }
    }

    [ExecuteAlways]
    [HelpURL (Constant.URL_MANUAL)]
    [AddComponentMenu ("WaterCausticsModules/WaterCausticsTexGenerator")]
    public class WaterCausticsTexGenerator : MonoBehaviour {

#if UNITY_2020_3_OR_NEWER
        // ----------------------------------------------------------- Constant
        private const int THREAD_SIZE = 16;
        internal const int WAVE_MAX_CNT = 4;
        readonly private string [] _lcStyleStr = { "STYLE_A", "STYLE_B", "STYLE_C", };
        readonly private float [] _lcStyleBright = { 1f, 1.3f, 1.3f };
        readonly private float [] _lcStyleGamma = { 1f, 1.25f, 1.02f };

        // ----------------------------------------------------------- SerializeField
        [SerializeField] private bool m_generateInEditMode = true;
        [SerializeField] private bool m_animateInEditMode = true;
        [SerializeField] private bool m_pause;
        [SerializeField, Range (0.1f, 3f)] private float m_density = 1f;
        [SerializeField, Range (0f, 4f)] private float m_height = 1f;
        [SerializeField, Range (0f, 4f)] private float m_speed = 1f;
        [SerializeField, Range (0f, 1.5f)] private float m_flow = 0f;
        [SerializeField, Range (-180f, 180f)] private float m_flowDirection = 0f;
        [SerializeField] private List<Wave> m_waves = new List<Wave> () {
            new Wave (7.3f, 0.55f, 0.85f, 0.11f, 100f),
            new Wave (3.7f, 0.4f, 0.55f, 0.2f, -60f)
        };
        [SerializeField] private CalcRes m_calcResolution = CalcRes.x160;
        [SerializeField] private RenderTexture m_renderTexture;
        [FormerlySerializedAs ("m_FillGapAmount")] [SerializeField, Range (0f, 0.5f)] private float m_FillGap = 0.08f;
        [FormerlySerializedAs ("m_lightDirectionType")] [SerializeField] private LightRay m_lightRay = LightRay.Direction;
        [SerializeField] private Transform m_lightTransform;
        [FormerlySerializedAs ("m_lightDir")] [SerializeField] private Vector3 m_lightVector = Vector3.down;
        [SerializeField, Range (-180f, 180f)] private float m_lightDirection = 0f;
        [SerializeField, Range (0f, 90f)] private float m_lightIncidentAngle = 0f;
        [FormerlySerializedAs ("m_lightCondensingStyle")] [SerializeField] private int m_version = Constant.WCE_VERSION_INT;
        [SerializeField] private Style m_style = Style.StyleA;
        [FormerlySerializedAs ("m_rayStyle")] [SerializeField] private RefractedRay m_refractedRay = RefractedRay.Normalize;
        [SerializeField, Range (0f, 3f)] private float m_brightness = 1f;
        [SerializeField, Range (0.0001f, 2f)] private float m_gamma = 1f;
        [SerializeField, Range (0f, 3f)] private float m_clamp = 1f;
        [Range (1f, 3f), SerializeField] private float m_refractionIndex = 1.33f;
        [SerializeField] private bool m_useChromaticAberration = false;
        [Range (0f, 0.3f), SerializeField] private float m_chromaticAberration = 0.005f;
        [SerializeField] private bool m_usePostProcessing = true;
        [SerializeField] private bool m_useBlur = true;
        [SerializeField, Range (1, 20)] private int m_blurIterations = 1;
        [SerializeField, Range (0f, 1f)] private float m_blurSpread = 0.5f;
        [SerializeField] private MSAASamples m_msaa = MSAASamples.MSAA8x;
        [SerializeField, Range (0f, 1f)] private float m_blurDirectional = 0f;
        [SerializeField, Range (-180f, 180f)] private float m_blurDirection = 0f;
        [SerializeField, Range (0f, 3f)] private float m_colorShift = 0f;
        [SerializeField, Range (-180f, 180f)] private float m_colorShiftDir = 0f;
        [SerializeField] private bool m_useSyncDirection = true;
        [SerializeField, Range (0f, 2f)] private float m_postBrightness = 1f;
        [SerializeField, Range (0.0001f, 2f)] private float m_postContrast = 1f;
        [SerializeField] private ComputeShader m_computeShader;
        [SerializeField] private Shader m_shader;
#if WCE_DEVELOPMENT
        [SerializeField] private bool m_useSpecifiedTime;
        [SerializeField] private float m_specifiedTime;
        [SerializeField, Range (0f, 1f)] private float m_specifiedTimeMultiplier = 0.05f;
#endif

        // ----------------------------------------------------------- private property
        private bool useChromAbe => (m_useChromaticAberration && m_chromaticAberration > 0f);
        private int bufSize3or1 => useChromAbe? 3 : 1;

        // ----------------------------------------------------------- Public property
        public bool generateInEditMode {
            get => m_generateInEditMode;
            set => m_generateInEditMode = value;
        }
        public bool animateInEditMode {
            get => m_animateInEditMode;
            set => m_animateInEditMode = value;
        }
        public bool pause {
            get => m_pause;
            set => m_pause = value;
        }
        public float density {
            get => m_density;
            set => m_density = Mathf.Clamp (value, 0.1f, 4f);
        }
        public float height {
            get => m_height;
            set => m_height = Mathf.Max (value, 0f);
        }
        public float speed {
            get => m_speed;
            set => m_speed = Mathf.Max (value, 0f);
        }
        public float flow {
            get => m_flow;
            set => m_flow = value;
        }
        public float flowDirection {
            get => m_flowDirection;
            set => m_flowDirection = wrapAngle180 (value);
        }
        public List<Wave> waves {
            get => m_waves;
        }
        public CalcRes calculateResolution {
            get => m_calcResolution;
            set => m_calcResolution = value;
        }
        public RenderTexture renderTexture {
            get => m_renderTexture;
            set => m_renderTexture = value;
        }
        public float fillGapAmount {
            get => m_FillGap;
            set => m_FillGap = Mathf.Clamp (value, 0f, 0.5f);
        }
        public LightRay lightRayType {
            get => m_lightRay;
            set => m_lightRay = value;
        }
        public Transform lightTransform {
            get => m_lightTransform;
            set => m_lightTransform = value;
        }
        public Vector3 lightVector {
            get => m_lightVector;
            set => m_lightVector = value;
        }
        public float lightDirection {
            get => m_lightDirection;
            set => syncDir (ref m_lightDirection, value);
        }
        public float lightIncidentAngle {
            get => m_lightIncidentAngle;
            set => m_lightIncidentAngle = Mathf.Clamp (value, 0f, 90f);
        }
        public Style style {
            get => m_style;
            set => m_style = value;
        }
        public RefractedRay refractedRayProcessing {
            get => m_refractedRay;
            set => m_refractedRay = value;
        }
        public float brightness {
            get => m_brightness;
            set => m_brightness = Mathf.Max (value, 0f);
        }
        public float gamma {
            get => m_gamma;
            set => m_gamma = Mathf.Max (value, 0.0001f);
        }
        public float clamp {
            get => m_clamp;
            set => m_clamp = Mathf.Max (value, 0f);
        }
        public float refractionIndex {
            get => m_refractionIndex;
            set => m_refractionIndex = Mathf.Clamp (value, 1f, 3f);
        }
        public bool useChromaticAberration {
            get => m_useChromaticAberration;
            set => m_useChromaticAberration = value;
        }
        public float chromaticAberration {
            get => m_chromaticAberration;
            set => m_chromaticAberration = Mathf.Clamp (value, 0f, 0.5f);
        }
        public bool usePostProcessing {
            get => m_usePostProcessing;
            set => m_usePostProcessing = value;
        }
        public bool useBlur {
            get => m_useBlur;
            set => m_useBlur = value;
        }
        public int blurIterations {
            get => m_blurIterations;
            set => m_blurIterations = Mathf.Clamp (value, 1, 10);
        }
        public float blurSpread {
            get => m_blurSpread;
            set => m_blurSpread = Mathf.Clamp (value, 0f, 1f);
        }
        public MSAASamples msaa {
            get => m_msaa;
            set => m_msaa = value;
        }
        public float blurDirectional {
            get => m_blurDirectional;
            set => m_blurDirectional = Mathf.Clamp (value, 0f, 1f);
        }
        public float blurDirection {
            get => m_blurDirection;
            set => syncDir (ref m_blurDirection, value);
        }
        public float colorShift {
            get => m_colorShift;
            set => m_colorShift = Mathf.Max (value, 0f);
        }
        public float colorShiftDirection {
            get => m_colorShiftDir;
            set => syncDir (ref m_colorShiftDir, value);
        }

        public bool useSyncDirection {
            get => m_useSyncDirection;
            set {
                m_useSyncDirection = value;
                if (value) m_blurDirection = m_colorShiftDir = m_lightDirection;
            }
        }
        public float postContrast {
            get => m_postContrast;
            set => m_postContrast = Mathf.Max (value, 0.0001f);
        }
        public float postBrightness {
            get => m_postBrightness;
            set => m_postBrightness = Mathf.Max (value, 0f);
        }

        // ----------------------------------------------------------- Tools
        private void syncDir (ref float prop, float dir) {
            if (m_useSyncDirection)
                m_lightDirection = m_blurDirection = m_colorShiftDir = wrapAngle180 (dir);
            else
                prop = wrapAngle180 (dir);
        }
        static private float round2Dec (float val) => (float) (System.Math.Round (val * 100f) * 0.01); // 小数点2位で丸め
        static private float round2Dec5 (float val) => (float) (System.Math.Round (val * 20f) * 0.05); // 小数点2位を0か5で丸め
        static private float wrapAngle180 (float angle) => Mathf.Repeat (angle + 180f, 360f) - 180f;
        static private Vector2 dirToVec (float dir) => new Vector2 (Mathf.Sin (dir * Mathf.Deg2Rad), Mathf.Cos (-dir * Mathf.Deg2Rad));
        static private float vecToDir (Vector2 v) => round2Dec5 (wrapAngle180 (Mathf.Atan2 (v.x, v.y) * Mathf.Rad2Deg));

        // ----------------------------------------------------------- MenuItem
#if UNITY_EDITOR
        [ContextMenu ("Apply Overall Wave Adjustment To Each Waves")]
        private void ApplyOverallWaveAdjustment () {
            Undo.RecordObject (this, "WCEffect Version Update");
            EditorUtility.SetDirty (this);
            foreach (var wave in m_waves) {
                wave.density = round2Dec5 (wave.density * m_density);
                wave.height = round2Dec5 (wave.height * m_height);
                wave.fluctuation = round2Dec5 (wave.fluctuation * m_speed);
                wave.flow = round2Dec (wave.flow / m_density * m_speed);
            }
            m_density = 1f;
            m_height = 1f;
            m_speed = 1f;
        }

        [ContextMenu ("Open Asset Manual")]
        private void OpenManualURL () {
            Application.OpenURL (Constant.URL_MANUAL);
        }
#endif

        // ----------------------------------------------------------- Init
        private void Reset () { }

        internal void VersionCheck () {
#if UNITY_EDITOR
            if (m_version < Constant.WCE_VERSION_INT) EditorUtility.SetDirty (this);
#endif
            const int VER_20000 = 20000;
            if (m_version < VER_20000) {
                // -- Ver1.2.2以前に作成されている場合、データをv2用にアップデート
                m_style = (Style) m_version; // m_version は元 m_lightCondensingStyle
                if (m_lightRay == LightRay.Vector && m_lightVector == Vector3.down) {
                    m_lightRay = LightRay.Direction;
                    m_lightIncidentAngle = 0f;
                }
                foreach (var w in m_waves)
                    w.renewFlowData (m_density);
                m_usePostProcessing = false;
                m_version = VER_20000; // ※最後に設定
            }
            if (m_version < Constant.WCE_VERSION_INT)
                m_version = Constant.WCE_VERSION_INT;
        }

#if UNITY_EDITOR
        internal void CheckRenderTex () {
            var rt = m_renderTexture;
            if (rt && rt.depth != 0) {
                if (rt.IsCreated ()) {
                    bool isActive = (RenderTexture.active == rt);
                    if (isActive) RenderTexture.active = null;
                    rt.Release ();
                    rt.depth = 0;
                    rt.anisoLevel = 4;
                    rt.Create ();
                    if (isActive) RenderTexture.active = rt;
                } else {
                    rt.depth = 0;
                    rt.anisoLevel = 4;
                }
                EditorUtility.SetDirty (rt);
                Debug.Log ($"DepthBuffer of RenderTexture ({rt.name}) in use in WaterCausticsTexGenerator has been removed as it is not used.");
            }
        }
#endif

        // -----------------------------------------------------------

        private void Awake () {
            VersionCheck ();
            storeSystemMaxMSAA ();
        }

        private void OnEnable () {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= constantUpdate;
            UnityEditor.EditorApplication.update += constantUpdate;
            CheckRenderTex ();
#endif
        }

        private void OnDisable () {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= constantUpdate;
#endif
            if (!Application.isPlaying)
                destroyAllBuffers ();
        }

        void OnDestroy () {
            destroyAllBuffers ();
        }

        private void destroyAllBuffers () {
            releaseGraphicsBuffers ();
            destroy (ref __mesh);
            destroy (ref __mat);
            destroy (ref __computeShader);
        }

        private void destroy<T> (ref T o) where T : Object {
            if (o == null) return;
            if (Application.isPlaying)
                Destroy (o);
            else
                DestroyImmediate (o);
            o = null;
        }

        // ----------------------------------------------------------- PropertyToID
        private class pID {
            readonly internal static int _WaveCnt = Shader.PropertyToID ("_WaveCnt");
            readonly internal static int _WaveData = Shader.PropertyToID ("_WaveData");
            readonly internal static int _WaveUVShift = Shader.PropertyToID ("_WaveUVShift");
            readonly internal static int _WaveNoiseDir = Shader.PropertyToID ("_WaveNoiseDir");
            readonly internal static int _CalcResUI = Shader.PropertyToID ("_CalcResUI");
            readonly internal static int _CalcTexel = Shader.PropertyToID ("_CalcTexel");
            readonly internal static int _CalcTexelInv = Shader.PropertyToID ("_CalcTexelInv");
            readonly internal static int _LightDir = Shader.PropertyToID ("_LightDir");
            readonly internal static int _Eta = Shader.PropertyToID ("_Eta");
            readonly internal static int _Brightness = Shader.PropertyToID ("_Brightness");
            readonly internal static int _Gamma = Shader.PropertyToID ("_Gamma");
            readonly internal static int _Clamp = Shader.PropertyToID ("_Clamp");
            readonly internal static int _IdxStride = Shader.PropertyToID ("_IdxStride");
            readonly internal static int _DrawOffset = Shader.PropertyToID ("_DrawOffset");
            readonly internal static int _BufNoiseRW = Shader.PropertyToID ("_BufNoiseRW");
            readonly internal static int _BufNoise = Shader.PropertyToID ("_BufNoise");
            readonly internal static int _BufRefractRW = Shader.PropertyToID ("_BufRefractRW");
            readonly internal static int _BufRefract = Shader.PropertyToID ("_BufRefract");
            readonly internal static int _LightDirection = Shader.PropertyToID ("_LightDirection");
            readonly internal static int _Offset = Shader.PropertyToID ("_Offset");
            readonly internal static int _OffsetColor = Shader.PropertyToID ("_OffsetColor");
            readonly internal static int _SSLinearTex = Shader.PropertyToID ("_SSLinearTex");
        }

        // ----------------------------------------------------------- ComputeShader
        private ComputeShader __computeShader;
        private ComputeShader getComputeShader () {
            if (__computeShader != null)
                return __computeShader;

            if (m_computeShader == null) {
                Debug.LogError ("Compute Shader is null. " + this);
                return null;
            } else {
                __computeShader = (ComputeShader) Instantiate (m_computeShader);
                kID.setKernelID (__computeShader);
                return __computeShader;
            }
        }

        private class kID {
            internal static int NoiseCS;
            internal static int RefractCS;
            internal static int ColorCS;
            internal static void setKernelID (ComputeShader cs) {
                NoiseCS = cs.FindKernel ("NoiseCS");
                RefractCS = cs.FindKernel ("RefractCS");
                ColorCS = cs.FindKernel ("ColorCS");
            }
        }

        // ----------------------------------------------------------- Material
        private Material __mat;
        private Material getMat () {
            if (__mat != null) return __mat;
            if (m_shader == null) {
                Debug.LogError ("Shader is null. " + this);
                return null;
            } else {
                __mat = new Material (m_shader);
                __mat.hideFlags = HideFlags.HideAndDontSave;
                return __mat;
            }
        }

        private void setMaterialKeyword (Material mat, bool isEnable, string keyword) {
            if (isEnable)
                mat.EnableKeyword (keyword);
            else
                mat.DisableKeyword (keyword);
        }

        // ----------------------------------------------------------- Mesh
        private Mesh __mesh;
        private int _meshVertsCnt;
        private int calcVerticesCnt (int res) {
            int wholeWidth = (int) res + (int) ((float) res * m_FillGap) * 2;
            return (wholeWidth + 1) * (wholeWidth + 1) * bufSize3or1;
        }
        private Mesh getMesh () {
            if (!__mesh || _meshVertsCnt != calcVerticesCnt ((int) m_calcResolution))
                setupMesh ();
            return __mesh;
        }

        private void setupMesh () {
            int width = (int) m_calcResolution;
            float widthF = (float) width;
            int bufArea = width * width;
            float cellSz = 1f / widthF;
            int over = (int) (widthF * m_FillGap);
            int wholeWidth = width + over * 2;
            int bufSz3or1 = bufSize3or1;
            int verticesCnt = (wholeWidth + 1) * (wholeWidth + 1) * bufSz3or1;
            Vector3 [] vertices = new Vector3 [verticesCnt];
            for (int vi = 0, i = 0; i < bufSz3or1; i++) {
                for (int y = 0; y <= wholeWidth; y++) {
                    for (int x = 0; x <= wholeWidth; x++, vi++) {
                        int pX = x - over;
                        int pY = y - over;
                        float vX = (float) pX * cellSz;
                        float vY = (float) pY * cellSz;
                        int idx = ((pY + width * 10) % width) * width + ((pX + width * 10) % width);
                        float vZ = (float) (i * bufArea + idx) + 0.1f;
                        vertices [vi] = new Vector3 (vX * 2f - 1f, vY * 2f - 1f, vZ);
                    }
                }
            }
            int [] triangles = new int [wholeWidth * wholeWidth * 6 * bufSz3or1];
            for (int ti = 0, vi = 0, i = 0; i < bufSz3or1; i++, vi += wholeWidth + 1) {
                for (int y = 0; y < wholeWidth; y++, vi++) {
                    for (int x = 0; x < wholeWidth; x++, ti += 6, vi++) {
                        triangles [ti] = vi;
                        triangles [ti + 2] = triangles [ti + 3] = vi + 1;
                        triangles [ti + 1] = triangles [ti + 4] = vi + wholeWidth + 1;
                        triangles [ti + 5] = vi + wholeWidth + 2;
                    }
                }
            }
            if (__mesh == null)
                __mesh = new Mesh ();
            __mesh.Clear ();
            __mesh.indexFormat = (verticesCnt >= 65536) ? IndexFormat.UInt32 : IndexFormat.UInt16;
            __mesh.vertices = vertices;
            __mesh.triangles = triangles;
            __mesh.name = "WaterCausticsTexGen";
            _meshVertsCnt = verticesCnt;
        }

        // ----------------------------------------------------------- GraphicsBuffer
        private GraphicsBuffer _bufNoise;
        private GraphicsBuffer _bufRefract;
        void prepGraphicsBuffers () {
            int res = (int) m_calcResolution;
            int resSq = res * res;
            checkAndRemakeCBuffer (ref _bufNoise, resSq, 1);
            checkAndRemakeCBuffer (ref _bufRefract, resSq * bufSize3or1, 5);
        }

        private void checkAndRemakeCBuffer (ref GraphicsBuffer buf, int count, int stride) {
            if (buf == null || buf.count != count) {
                if (buf != null) buf.Release ();
                buf = new GraphicsBuffer (GraphicsBuffer.Target.Structured, count, sizeof (float) * stride);
            }
        }

        private void releaseGraphicsBuffers () {
            release (ref _bufNoise);
            release (ref _bufRefract);
        }

        private void release (ref GraphicsBuffer cb) {
            if (cb == null) return;
            cb.Release ();
            cb = null;
        }

        // ----------------------------------------------------------- Preview
#if UNITY_EDITOR
        private bool _isPreviewTarget;
        internal void Preview (float deltaTime, RenderTexture rt) {
            _isPreviewTarget = true;
            if (isActiveAndEnabled && isDrawer (this))
                Graphics.Blit (m_renderTexture, rt);
            else {
                generate (deltaTime, rt);
            }
        }

        internal void FinishPreview () {
            _isPreviewTarget = false;
            if (!isActiveAndEnabled)
                destroyAllBuffers ();
        }
#endif

        // ----------------------------------------------------------- Update
        static private Dictionary<RenderTexture, WaterCausticsTexGenerator> s_drawer = new Dictionary<RenderTexture, WaterCausticsTexGenerator> ();
        static private bool isDrawer (WaterCausticsTexGenerator texGen) {
            var rt = texGen.m_renderTexture;
            return (rt != null && s_drawer.ContainsKey (rt) && s_drawer [rt] == texGen);
        }


#if UNITY_EDITOR
        private bool needAnimInEditMode => ((m_animateInEditMode && m_generateInEditMode) || _isPreviewTarget);
        private void constantUpdate () {
            if (!UnityEditorInternal.InternalEditorUtility.isApplicationActive) return;
            if (Application.isPlaying) return;
            if (needAnimInEditMode) {
                EditorApplication.QueuePlayerLoopUpdate ();
                // UnityEditorInternal.InternalEditorUtility.RepaintAllViews ();
            }
        }
#endif


        void Update () {
#if UNITY_EDITOR
            if (!Application.isPlaying && !m_generateInEditMode && !_isPreviewTarget) return;
#endif
            if (m_renderTexture)
                s_drawer [m_renderTexture] = this;
        }


        private void LateUpdate () {
            if (!isDrawer (this)) return;
#if UNITY_EDITOR
            float deltaTime = (Application.isPlaying || needAnimInEditMode) ? Time.deltaTime : 0f;
            generate (deltaTime, m_renderTexture);
#else
            generate (Time.deltaTime, m_renderTexture);
#endif         
        }


        private void generate (float deltaTime, RenderTexture rt) {
            if (rt == null) return;

            // Prepare
            prepGraphicsBuffers ();

            // Set data for compute shader 
            setConstantBuffer (deltaTime);

            // Compute shader
            calcComputeShader ();

            // Draw mesh
            if (m_usePostProcessing) {
                var baseRT = getPPTmpRT (rt, useMSAA : true);
                drawMesh (baseRT);
                postProcessing (baseRT, rt);
            } else {
                drawMesh (rt);
            }

            // MipMap
            if (rt.useMipMap && rt.antiAliasing == 1 && !rt.autoGenerateMips)
                rt.GenerateMips ();
        }


        Vector3 refract (Vector3 I, float eta) {
            float dot = -I.z;
            float k = 1f - eta * eta * (1f - dot * dot);
            return (k < 0f) ? Vector3.zero : eta * I + Vector3.forward * (eta * dot + Mathf.Sqrt (k));
        }


        private Vector4 [] _tmpDataAry = new Vector4 [WAVE_MAX_CNT];
        private Vector4 [] _tmpUVAry = new Vector4 [WAVE_MAX_CNT];
        private Vector4 [] _tmpDirAry = new Vector4 [WAVE_MAX_CNT];


        private float getDecimal (float v) {
            return v - Mathf.Floor (v);
        }

        const float NOISE_RADIUS = 100f;
        const float NOISE_CIRCUMFERENCE_INV = 1f / (NOISE_RADIUS * 2f * Mathf.PI);

        private void setConstantBuffer (float delteTime) {
            var cs = getComputeShader ();
            if (cs == null) return;

            // Waves
            int waveIdx = 0;
            float delta = delteTime * m_speed * (m_pause? 0f : 1f);
#if WCE_DEVELOPMENT
            if (m_useSpecifiedTime) {
                foreach (Wave w in m_waves)
                    w.pos = Vector3.zero;
                delta = m_specifiedTime * m_specifiedTimeMultiplier;
            }
#endif
            Vector2 overallFlow = dirToVec (m_flowDirection) * m_flow * delta;
            for (int i = 0; i < m_waves.Count; i++) {
                Wave w = m_waves [i];
                if (!w.active) continue;
                Vector2 flowV = dirToVec (w.direction) * w.flow * delta / m_density + overallFlow;
                w.pos.x = getDecimal (w.pos.x - flowV.x);
                w.pos.y = getDecimal (w.pos.y - flowV.y);
                w.pos.z = getDecimal (w.pos.z + w.fluctuation * NOISE_CIRCUMFERENCE_INV * delta);
                float rad = w.pos.z * Mathf.PI * 2f;
                float cos = Mathf.Cos (rad);
                float sin = Mathf.Sin (rad);
                _tmpDirAry [waveIdx] = new Vector2 (cos, sin);
                _tmpUVAry [waveIdx] = new Vector2 (w.pos.x, w.pos.y);
                _tmpDataAry [waveIdx] = w.getData (m_density, m_height, i);
                if (++waveIdx >= WAVE_MAX_CNT) break;
            }
            cs.SetInt (pID._WaveCnt, waveIdx);
            cs.SetVectorArray (pID._WaveData, _tmpDataAry);
            cs.SetVectorArray (pID._WaveUVShift, _tmpUVAry);
            cs.SetVectorArray (pID._WaveNoiseDir, _tmpDirAry);

            // Resolution
            int calcRes = (int) m_calcResolution;
            float texel = 1f / (float) calcRes;
            cs.SetFloat (pID._CalcTexel, texel);
            cs.SetFloat (pID._CalcTexelInv, (float) calcRes);
            cs.SetInt (pID._CalcResUI, calcRes);
            cs.SetInt (pID._IdxStride, calcRes * calcRes);

            // light Direction
            Vector3 litDir;
            switch (m_lightRay) {
                case LightRay.Vector:
                    litDir = (m_lightVector != Vector3.zero) ? m_lightVector.normalized : Vector3.down;
                    break;
                case LightRay.Transform:
                    litDir = (m_lightTransform != null) ? m_lightTransform.forward : Vector3.down;
                    break;
                case LightRay.LitSettingSun:
                    litDir = (RenderSettings.sun != null) ? RenderSettings.sun.transform.forward : Vector3.down;
                    break;
                case LightRay.Auto:
                    litDir = -Shader.GetGlobalVector (pID._LightDirection);
                    break;
                case LightRay.Direction:
                default:
                    if (m_lightIncidentAngle == 0f) {
                        litDir = Vector3.down;
                    } else {
                        Vector2 dirV = dirToVec (m_lightDirection);
                        litDir = Vector3.Slerp (Vector3.down, new Vector3 (dirV.x, 0f, dirV.y), m_lightIncidentAngle / 90f);
                    }
                    break;
            }
            if (litDir.y >= 0f) {
                litDir.y = -0.01f;
                litDir = litDir.normalized;
            }
            litDir = new Vector3 (litDir.x, litDir.z, -litDir.y);
            cs.SetVector (pID._LightDir, litDir);

            // Brightness
            cs.SetFloat (pID._Brightness, m_brightness * _lcStyleBright [(int) m_style] * 0.1f);
            cs.SetFloat (pID._Gamma, m_gamma * _lcStyleGamma [(int) m_style]);
            cs.SetFloat (pID._Clamp, m_clamp);

            // Refraction index
            float eta = 1f / m_refractionIndex;
            float chrAb = 1f + m_chromaticAberration;
            Vector3 refractIdx = new Vector3 (eta * chrAb, eta, eta / chrAb);
            cs.SetVector (pID._Eta, refractIdx);

            // Offset drawing position
            Vector3 refractG = refract (litDir, eta);
            if (m_refractedRay == RefractedRay.Normalize) {
                cs.DisableKeyword ("EXTEND_RAY");
                cs.SetVector (pID._DrawOffset, -(Vector2) refractG);
            } else {
                cs.EnableKeyword ("EXTEND_RAY");
                cs.SetVector (pID._DrawOffset, -(Vector2) refractG / refractG.z);
            }
        }


        private void calcComputeShader () {
            var cs = getComputeShader ();
            if (cs == null) return;

            cs.EnableKeyword (_lcStyleStr [(int) m_style]);

            int kernel;
            int sc = (int) m_calcResolution / THREAD_SIZE;
            int shift = useChromAbe ? 1 : 0;

            cs.SetBuffer (kID.NoiseCS, pID._BufNoiseRW, _bufNoise);
            cs.Dispatch (kID.NoiseCS, sc, sc, 1);

            kernel = kID.RefractCS + shift;
            cs.SetBuffer (kernel, pID._BufNoise, _bufNoise);
            cs.SetBuffer (kernel, pID._BufRefractRW, _bufRefract);
            cs.Dispatch (kernel, sc, sc, 1);

            kernel = kID.ColorCS + shift;
            cs.SetBuffer (kernel, pID._BufRefractRW, _bufRefract);
            cs.Dispatch (kernel, sc, sc, 1);

            // // ---- for Debug
            // cs.SetBuffer (kID.ColorCS + 2, pID._BufNoise, _bufNoise);
            // cs.SetBuffer (kID.ColorCS + 2, pID._BufRefractRW, _bufRefract);
            // cs.Dispatch (kID.ColorCS + 2, sc, sc, 1);
            // // ----

            // Keywords
            cs.DisableKeyword (_lcStyleStr [(int) m_style]);
        }


        private void drawMesh (RenderTexture rt) {
            var mat = getMat ();
            if (mat == null) return;

            var temp = RenderTexture.active;
            RenderTexture.active = rt;
            rt.DiscardContents ();
            GL.Clear (false, true, Color.clear);
            setMaterialKeyword (mat, useChromAbe, "_USE_RGB");
            mat.SetBuffer (pID._BufRefract, _bufRefract);
            mat.SetPass (0);
            Graphics.DrawMeshNow (getMesh (), Matrix4x4.identity);
            RenderTexture.active = temp;

            rt.IncrementUpdateCount ();
        }


        // ----------------------------------------------------------- PostProcessing
        private int _sysMaxMSAA = 8;
        private void storeSystemMaxMSAA () {
            var desc = new RenderTextureDescriptor (128, 128);
            desc.msaaSamples = 8;
            _sysMaxMSAA = SystemInfo.GetRenderTextureSupportedMSAASampleCount (desc);
        }

        private RenderTexture getPPTmpRT (RenderTexture rt, bool useMSAA) {
            var desc = new RenderTextureDescriptor (rt.width, rt.height, rt.graphicsFormat, 0); // rt.formatだとSNORMテクスチャでエラー
            desc.useDynamicScale = rt.useDynamicScale;
            desc.msaaSamples = useMSAA ? Mathf.Min ((int) m_msaa, _sysMaxMSAA) : 1;
            return RenderTexture.GetTemporary (desc);
        }

        private void postProcessing (RenderTexture src, RenderTexture dst) {
#if !UNITY_EDITOR // GLES3でアーティファクトが発生するので対応
            if (dst.antiAliasing > 1) {
                dst.Release ();
                dst.antiAliasing = 1;
            }
#endif
            Vector4 offsetBaseV;
            int iteration = m_useBlur ? m_blurIterations : 1;
            float spread = m_useBlur ? m_blurSpread : 0f;
            bool useBlur = m_useBlur && (iteration > 1 || spread > 0f);
            if (useBlur && m_blurDirectional > 0f) {
                var rot = Quaternion.Euler (0f, 0f, m_blurDirection - 90f);
                var rotInv = Quaternion.Inverse (rot);
                Vector2 offA = rot * Vector2.one;
                Vector2 offB = rot * new Vector2 (1f, -1f);
                float oblateness = 1f - m_blurDirectional;
                offA.y *= oblateness;
                offB.y *= oblateness;
                offA = rotInv * offA;
                offB = rotInv * offB;
                offsetBaseV = new Vector4 (offA.x, offA.y, offB.x, offB.y);
            } else {
                offsetBaseV = new Vector4 (1f, 1f, 1f, -1f);
            }

            var mat = getMat ();
            setMaterialKeyword (mat, useChromAbe, "_USE_RGB");
            mat.SetVector (pID._OffsetColor, Vector2.zero);
            float colShift = 1f;
            bool useColAdjust = (m_postContrast != 1f || m_postBrightness != 1f || QualitySettings.activeColorSpace != ColorSpace.Linear);
            setMaterialKeyword (mat, false, "_BRIGHTNESS_ADJ");

            var temp = RenderTexture.active;
            RenderTexture.active = dst; // ← エディタでGLES3使用時 DstがMSAA使用の場合必須
            RenderTexture buf = src;

            void sampling (RenderTexture src, RenderTexture dst, Vector4 offV, bool useColorShift = false) {
                bool canDiv (float v) { return 0.5f - Mathf.Abs (v - Mathf.Floor (v) - 0.5f) <= 0.001f; }
                src.filterMode = (!useColorShift && canDiv (offV.x) && canDiv (offV.y) && canDiv (offV.z) && canDiv (offV.w)) ? FilterMode.Point : FilterMode.Bilinear;
                src.wrapMode = TextureWrapMode.Repeat;
                mat.SetVector (pID._Offset, offV);
                Graphics.Blit (src, dst, mat, 1);
            }

            for (int i = 0; i < Mathf.Max (1, iteration); i++) {
                bool isLast = (i == iteration - 1);
                RenderTexture buf2 = isLast ? dst : getPPTmpRT (dst, useMSAA : false);
                float off = (iteration == 1) ? spread : 0.5f + i * spread;
                colShift += off * 0.25f;
                setMaterialKeyword (mat, off != 0f, "_SAMPLE4");
                if (isLast && useColAdjust) {
                    setMaterialKeyword (mat, true, "_BRIGHTNESS_ADJ");
                    mat.SetFloat (pID._Gamma, m_postContrast * (QualitySettings.activeColorSpace == ColorSpace.Gamma ? 1f / 2.2f : 1f));
                    mat.SetFloat (pID._Brightness, m_postBrightness * m_postContrast);
                }
                if (isLast && m_colorShift > 0f) {
                    Vector2 offCol = dirToVec (m_colorShiftDir) * (m_colorShift * colShift);
                    setMaterialKeyword (mat, true, "_USE_RGB");
                    mat.SetVector (pID._OffsetColor, offCol * 0.01f);
                    sampling (buf, buf2, offsetBaseV * off, true);
                } else {
                    sampling (buf, buf2, offsetBaseV * off);
                }
                RenderTexture.ReleaseTemporary (buf);
                buf = buf2;
            }

            dst.IncrementUpdateCount ();
            RenderTexture.active = temp;
        }


        // ----------------------------------------------------------- Wave class
        [System.Serializable]
        public class Wave {
            [SerializeField, FormerlySerializedAs ("Active")] private bool m_active = true;
            [SerializeField, Range (1f, 20f)] private float m_density = 3f;
            [SerializeField, Range (0f, 1f)] private float m_height = 0.5f;
            [SerializeField, Range (0f, 4f)] private float m_fluctuation = 0.6f;
            [SerializeField, Range (0f, 1.5f)] private float m_flow;
            [SerializeField, Range (-180f, 180f)] private float m_direction;
            [SerializeField, HideInInspector] private bool Pause; // Legacy
            [SerializeField, HideInInspector] private float m_flowU; // Legacy
            [SerializeField, HideInInspector] private float m_flowV; // Legacy

            public bool active {
                get => m_active;
                set => m_active = value;
            }
            public float density {
                get => m_density;
                set => m_density = Mathf.Clamp (value, 1f, 20f);
            }
            public float height {
                get => m_height;
                set => m_height = Mathf.Clamp (value, 0f, 1f);
            }
            public float fluctuation {
                get => m_fluctuation;
                set => m_fluctuation = Mathf.Clamp (value, 0f, 4f);
            }
            public float flow {
                get => m_flow;
                set => m_flow = value;
            }
            public float direction {
                get => m_direction;
                set => m_direction = wrapAngle180 (value);
            }

            [System.NonSerialized] internal Vector3 pos;

            public Wave (float density, float height, float fluct, float flow, float dir) {
                m_density = density;
                m_height = height;
                m_fluctuation = fluct;
                m_flow = flow;
                m_direction = dir;
            }

            internal Vector3 getData (float adjustDensity, float adjustHeight, int idx) {
                float d = m_density * adjustDensity;
                return new Vector3 (d, m_height * adjustHeight / (d * d) * 0.5f, idx);
            }

            internal void renewFlowData (float scale) {
                Vector2 v = new Vector2 (m_flowU, m_flowV) * scale / m_density;
                float dir = Mathf.Atan2 (-v.y, v.x) * Mathf.Rad2Deg;
                m_flow = round2Dec (v.magnitude);
                m_direction = vecToDir (v);
            }

        }

        // -----------------------------------------------------------
#endif // End of UNITY_2020_3_OR_NEWER
    }
}
