// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_DummyCamera : MonoBehaviour {
        private GameObject _camGO;
        private void Awake () {
            _camGO = new GameObject ("DummyCamera");
            _camGO.hideFlags = HideFlags.HideAndDontSave;
            var cam = _camGO.AddComponent<Camera> ();
            cam.cullingMask = 0;
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
        }

        private void OnDestroy () {
            destroy (ref _camGO);
        }

        static private void destroy<T> (ref T o) where T : Object {
            if (o == null) return;
            if (Application.isPlaying)
                Destroy (o);
            else
                DestroyImmediate (o);
            o = null;
        }

    }
}
