// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [RequireComponent (typeof (ReflectionProbe))]
    [AddComponentMenu ("")]
    public class DEMO_ReflectionProbeController : MonoBehaviour {
        private ReflectionProbe _probe;

        private void OnEnable () {
            _probe = GetComponent<ReflectionProbe> ();
            _probe.mode = ReflectionProbeMode.Realtime;
            _probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            _probe.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
            _probe.resolution = 512; // SSAOがOnの場合512サイズ以下だと暗くなるので対策
            StartCoroutine (coroutine ());
            if (Application.isPlaying)
                QualitySettings.realtimeReflectionProbes = true;
        }

        IEnumerator coroutine () {
            _probe.RenderProbe ();
            for (int i = 0; i < 10; i++) yield return null;
            _probe.RenderProbe ();
            yield return new WaitForSeconds (2f);
            _probe.RenderProbe ();
        }

    }
}
