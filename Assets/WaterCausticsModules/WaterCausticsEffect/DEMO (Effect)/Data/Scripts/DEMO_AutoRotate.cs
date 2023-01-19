// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;

namespace MH.WaterCausticsModules {
    [AddComponentMenu ("")]
    public class DEMO_AutoRotate : MonoBehaviour {
        public Vector3 m_Rotate = new Vector3 (0f, 0f, 0f);

        public float m_Speed = 1;

        public bool m_WorldPivot = false;

        private Space pivot = Space.Self;


        void Start () {
            if (m_WorldPivot) pivot = Space.World;
        }

        void Update () {
            var r = m_Rotate * m_Speed * Time.deltaTime * 60f;
            this.transform.Rotate (r, pivot);
        }
    }
}
