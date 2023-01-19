// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;

namespace MH.WaterCausticsModules {
    [AddComponentMenu ("")]
    public class DEMO_AutoSwing : MonoBehaviour {
        public Vector3 m_Axis = Vector3.right;
        public float m_Amplitude = 5f;
        public float m_WaveLength = 1f;
        [Range (0f, 1f)] public float m_InitValue = 0f;

        private Quaternion _initRot;
        private float _t;

        private void Start () {
            _initRot = transform.localRotation;
            _t = m_InitValue * Mathf.PI * 2f;
        }

        private void Update () {
            _t += (Time.deltaTime * m_WaveLength) % (Mathf.PI * 2f);
            float r = Mathf.Sin (_t) * m_Amplitude;
            transform.localRotation = _initRot * Quaternion.AngleAxis (r, m_Axis);
        }

    }
}
