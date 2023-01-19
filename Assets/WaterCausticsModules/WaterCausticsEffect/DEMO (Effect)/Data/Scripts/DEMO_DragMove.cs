// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;

namespace MH.WaterCausticsModules {

    [DisallowMultipleComponent]
    [AddComponentMenu ("")]
    public class DEMO_DragMove : MonoBehaviour {
#if UNITY_EDITOR
        readonly private float DAMPING = 0.1f;
        private Vector3 _tarPos, _veloPos, _offset;
        private Plane _plane;
        private bool _isTouch;

        private void OnEnable () {
            _tarPos = transform.position;
            _veloPos = Vector3.zero;
        }

        private void OnMouseOver () {
            if (Input.GetMouseButtonDown (1)) {
                _isTouch = true;
                Vector3 planeNorm = getAxis (Camera.main.transform.forward);
                _plane = new Plane (planeNorm, transform.position);
                _offset = getMousePtOnPlane (out var mousePt) ? transform.position - mousePt : Vector3.zero;
                _tarPos = transform.position;
                _veloPos = Vector3.zero;
            }
        }

        private void Update () {
            if (_isTouch) {
                if (Input.GetMouseButton (1) && getMousePtOnPlane (out var mousePt)) {
                    _tarPos = mousePt + _offset;
                }
                if (Input.GetMouseButtonUp (1)) {
                    _isTouch = false;
                }
            }
            if (transform.position != _tarPos) {
                transform.position = smoothDampSafe (transform.position, _tarPos, ref _veloPos, DAMPING, Time.unscaledDeltaTime);
            }
        }

        private static Vector3 smoothDampSafe (Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime, float deltaTime) {
            if (deltaTime == 0f) {
                return current;
            } else if (smoothTime <= float.Epsilon) {
                currentVelocity = Vector3.zero;
                return target;
            }
            return Vector3.SmoothDamp (current, target, ref currentVelocity, smoothTime, Mathf.Infinity, deltaTime);
        }

        private Vector3 getAxis (Vector3 v) {
            float x = Mathf.Abs (v.x);
            float y = Mathf.Abs (v.y);
            float z = Mathf.Abs (v.z);
            if (z >= x && z >= y) {
                return Vector3.forward;
            } else if (x >= y) {
                return Vector3.right;
            } else {
                return Vector3.up;
            }
        }

        private bool getMousePtOnPlane (out Vector3 mousePt) {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            bool result = _plane.Raycast (ray, out float enter);
            mousePt = result ? ray.GetPoint (enter) : Vector3.zero;
            return result;
        }
#endif
    }
}
