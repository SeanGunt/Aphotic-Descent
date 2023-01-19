// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#if WCE_URP
using System.Collections.Generic;
using MH.WaterCausticsModules.Effect;
using UnityEngine;
using UnityEngine.Rendering;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu ("")]
    internal class AtOnce : MonoBehaviour {
        private WaterCausticsEffect _summoner;
        private MeshRenderer _render;
        internal MeshRenderer render => _render;
        private void setRenderEnable (bool isOn) {
            if (_render && _render.enabled != isOn) _render.enabled = isOn;
        }

        private bool _inited;
        private AtOnce init (WaterCausticsEffect summoner, Material mat) {
            _inited = true;
            _summoner = summoner;
            var mf = gameObject.AddComponent<MeshFilter> ();
            mf.sharedMesh = getMesh ();
            _render = gameObject.AddComponent<MeshRenderer> ();
            _render.sharedMaterial = mat;
            _render.shadowCastingMode = ShadowCastingMode.Off;
            _render.lightProbeUsage = LightProbeUsage.Off;
            _render.reflectionProbeUsage = ReflectionProbeUsage.Off;
            _render.allowOcclusionWhenDynamic = true;
            _render.receiveShadows = true;
            updateTransform ();
            // -- HideFlags --
            gameObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
            // gameObject.hideFlags &= (~HideFlags.HideInHierarchy); // デバッグ用 Hierarchyで表示
            // ---------------
            return this;
        }

        private void OnDisable () {
            setRenderEnable (false);
        }

        private void OnDestroy () {
            destroy (ref __mesh);
        }

        private void LateUpdate () {
            if (!_summoner || !_summoner.isActiveAndEnabled || _summoner.method != Method.AtOnce) {
                setRenderEnable (false);
            } else {
                setRenderEnable (true);
                updateTransform ();
            }
        }

        private void updateTransform () {
            if (gameObject.layer != _summoner.gameObject.layer)
                gameObject.layer = _summoner.gameObject.layer;
            var sumTra = _summoner.transform;
            if (transform.parent != sumTra.parent)
                transform.SetParent (sumTra.parent, worldPositionStays : false);
            if (transform.localToWorldMatrix != sumTra.localToWorldMatrix) {
                transform.localPosition = sumTra.localPosition;
                transform.localRotation = sumTra.localRotation;
                transform.localScale = sumTra.localScale;
            }
        }

        private void Update () {
            bool isLeaked = (!_inited || !_summoner || _summoner.atOnce != this);
            if (isLeaked) destroy (gameObject);
        }

        // ----------------------------------------------------------- Mesh
        private Mesh __mesh;
        private Mesh getMesh () {
            if (!__mesh) {
                Mesh m = new Mesh ();
                m.name = "WCEMeshForAtOnce";
                m.vertices = new Vector3 [] { new Vector3 (-.5f, -.5f, -.5f), new Vector3 (.5f, -.5f, -.5f), new Vector3 (-.5f, .5f, -.5f), new Vector3 (.5f, .5f, -.5f), new Vector3 (-.5f, -.5f, .5f), new Vector3 (.5f, -.5f, .5f), new Vector3 (-.5f, .5f, .5f), new Vector3 (.5f, .5f, .5f), /* */ Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
                m.triangles = new int [] { 2, 6, 7, 2, 7, 3, 0, 2, 3, 0, 3, 1, 1, 3, 7, 1, 7, 5, 0, 4, 6, 0, 6, 2, 4, 5, 7, 4, 7, 6, 0, 1, 5, 0, 5, 4, /* */ 15, 8, 9, 15, 9, 10, 15, 10, 11, 15, 11, 12, 15, 12, 13, 15, 13, 14 };
                m.bounds = new Bounds (Vector3.zero, Vector3.one);
                m.hideFlags = HideFlags.HideAndDontSave;
                m.UploadMeshData (markNoLongerReadable: true);
                __mesh = m;
            }
            return __mesh;
        }

        // ----------------------------------------------------------- Static

        static internal AtOnce Create (WaterCausticsEffect summoner, Material mat) {
            var name = "(WCE Renderer) (Deletable)";
            var go = new GameObject (name);
            go.SetActive (false); // ← Flags設定時にOnEnable,OnDisableが呼ばれる不具合の回避
            var a = go.AddComponent<AtOnce> ().init (summoner, mat);
            go.SetActive (true);
            return a;
        }

        static internal void OnSummonerDestroyed (ref AtOnce a) {
            if (a == null) return;
            destroy (a.gameObject);
            a = null;
        }

        static private void destroy<T> (ref T o) where T : Object {
            if (o == null) return;
            destroy (o);
            o = null;
        }

        static private void destroy (Object o) {
            if (o == null) return;
            if (Application.isPlaying)
                Destroy (o);
            else
                DestroyImmediate (o);
        }

        // ----------------------------------------------------------- 
    }
}
#endif // End of WCE_URP
