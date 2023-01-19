// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

using UnityEngine;

namespace MH.WaterCausticsModules {
    [ExecuteAlways]
    [AddComponentMenu ("")]
    public class DEMO_CamController : MonoBehaviour {
        public Camera m_Camera;
        public Transform m_LookTarget;
        public float m_Damping = 0.2f;

        [Header ("Rotate")]
        public float m_SpeedRotDrag = 3f;
        public float m_SpeedRotKey = 0.5f;
        [Range (0f, 89f)] public float m_LookDownMax = 89f;
        [Range (0f, 89f)] public float m_LookUpMax = 89f;

        [Header ("Dolly")]
        public float m_DistanceMin = 0.1f;
        public float m_DistanceMax = 100f;
        public float m_SpeedDollyScroll = 1f;
        public float m_SpeedDollyPinch = 2f;
        public float m_SpeedDollyKey = 1f;

        [Header ("Pan")]
        public float m_SpeedPanDrag = 1f;
        public float m_SpeedPanKey = 1f;


        private bool _validDrag;
        private float _preTchDist;

        private void OnEnable () {
            if (!Application.isPlaying) return;
            if (!m_Camera) return;
            resetData ();
            m_Camera.transform.LookAt (m_LookTarget ? m_LookTarget : transform);
        }

        private Vector3 _tarPos, _veloPos;
        private float _tarRotY, _tarRotX, _tarDist;
        private float _curRotY, _curRotX, _curDist;
        private float _veloRotX, _veloRotY, _veloDist;

        private void resetData () {
            Vector3 v = transform.position - m_Camera.transform.position;
            var rot = Quaternion.LookRotation (v).eulerAngles;
            rot.x = Mathf.Repeat (rot.x + 180f, 360f) - 180f;
            _curRotY = _tarRotY = rot.y;
            _curRotX = _tarRotX = rot.x;
            _curDist = _tarDist = v.magnitude;
            _veloRotX = _veloRotY = _veloDist = 0f;
            _tarPos = transform.position;
            _veloPos = Vector3.zero;
        }

        private Vector2 _preMousePos;
        private int _lastGetPosFrame;
        private Vector2 getMouseDelta (float divX, float divY) {
            if (_lastGetPosFrame != Time.frameCount - 1) {
                _preMousePos = Input.mousePosition;
                _lastGetPosFrame = Time.frameCount;
                return Vector2.zero;
            } else {
                Vector3 mousePos = Input.mousePosition;
                float deltaX = (mousePos.x - _preMousePos.x) / divX;
                float deltaY = (mousePos.y - _preMousePos.y) / divY;
                _preMousePos = mousePos;
                _lastGetPosFrame = Time.frameCount;
                return new Vector2 (deltaX, deltaY);
            }
        }

        void rot (Vector2 deltaRot) {
            _tarRotX += -deltaRot.x;
            _tarRotY += deltaRot.y;
            _tarRotX = (_tarRotX <= 0f) ? Mathf.Max (_tarRotX, -m_LookUpMax) : Mathf.Min (_tarRotX, m_LookDownMax);
        }

        void dolly (float scroll, float speed) {
            _tarDist = Mathf.Clamp (_tarDist + scroll * speed * _tarDist, m_DistanceMin, m_DistanceMax);
        }

        void pan (Vector3 vec) {
            _tarPos = _tarPos + m_Camera.transform.TransformVector (vec);
        }

        private bool pointerOnGameView => new Rect (0f, 0f, Screen.width, Screen.height).Contains (Input.mousePosition);

        private static float smoothDampSafe (float current, float target, ref float currentVelocity, float smoothTime, float deltaTime) {
            // Mathf.SmoothDampはTimeScale0のときNanになるので対策
            if (deltaTime == 0f) {
                return current;
            } else if (smoothTime <= float.Epsilon) {
                currentVelocity = 0f;
                return target;
            }
            return Mathf.SmoothDamp (current, target, ref currentVelocity, smoothTime, Mathf.Infinity, deltaTime);
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

        private Vector3 _lastPos;
        private void LateUpdate () {
            if (!m_Camera) return;
            if (!Application.isPlaying) {
                m_Camera.transform.LookAt (m_LookTarget ? m_LookTarget : transform);
                return;
            }
            bool didChanged = false;
            if (_lastPos != transform.position) {
                didChanged = true;
                _lastPos = transform.position;
            }

            bool pressBtn0 = Input.touchCount <= 1 && Input.GetMouseButton (0);
            bool pressBtnMid = Input.GetMouseButton (2);
            bool pressAlt = Input.GetKey (KeyCode.LeftAlt);
            bool pressCtrl = Input.GetKey (KeyCode.LeftCommand) || Input.GetKey (KeyCode.LeftControl);
            bool pressShift = Input.GetKey (KeyCode.LeftShift);
            if (!pressBtn0 && !pressBtnMid) _validDrag = false;
            float inputH = Input.GetAxis ("Horizontal");
            float inputV = Input.GetAxis ("Vertical");

            // Rotate
            {
                if (!(pressAlt && pressCtrl) && pressBtn0) {
                    if (!_validDrag) {
                        if (pointerOnGameView) _validDrag = true;
                        _preMousePos = Input.mousePosition;
                    } else {
                        Vector2 mouseDelta = getMouseDelta (Screen.width, Screen.height);
                        if (pressShift) mouseDelta *= 0.1f;
                        rot (new Vector2 (mouseDelta.y, mouseDelta.x) * 90f * m_SpeedRotDrag);
                    }
                }

                // if (inputH != 0f || inputV != 0) {
                //     var vec = new Vector3 (-inputV, -inputH, 0) * m_SpeedRotKey;
                //     rot (vec);
                // }
            }

            // Dolly
            {
                if (Input.touchCount == 2) {
                    var tch0 = Input.GetTouch (0);
                    var tch1 = Input.GetTouch (1);
                    if (tch1.phase == TouchPhase.Began) {
                        _preTchDist = Vector2.Distance (tch0.position, tch1.position);
                    } else if (tch0.phase == TouchPhase.Moved || tch1.phase == TouchPhase.Moved) {
                        float curTchDist = Vector2.Distance (tch0.position, tch1.position);
                        float pinch = (curTchDist - _preTchDist) / (float) Mathf.Min (Screen.width, Screen.height);
                        _preTchDist = curTchDist;
                        dolly (-pinch, m_SpeedDollyPinch);
                    }
                } else if (Input.GetKey (KeyCode.X)) {
                    dolly (-1f, m_SpeedDollyKey * 0.001f);
                } else if (Input.GetKey (KeyCode.Z)) {
                    dolly (1f, m_SpeedDollyKey * 0.001f);
                } else {
                    float scroll = Input.GetAxis ("Mouse ScrollWheel");
                    if (scroll != 0f && pointerOnGameView) {
                        if (pressShift) scroll *= 0.1f;
                        dolly (-scroll, m_SpeedDollyScroll);
                    }
                }
            }

            // Pan
            {
                if (((pressAlt && pressCtrl) && pressBtn0) || pressBtnMid) {
                    if (!_validDrag) {
                        if (pointerOnGameView) _validDrag = true;
                        _preMousePos = Input.mousePosition;
                    } else {
                        Vector2 mouseDelta = getMouseDelta (Screen.height, Screen.height);
                        float hight = _tarDist * 2f / m_Camera.projectionMatrix.m11;
                        if (pressShift) mouseDelta *= 0.1f;
                        pan (mouseDelta * -m_SpeedPanDrag * hight);
                    }
                }

                if (inputH != 0f || inputV != 0) {
                    pan (new Vector3 (inputH, inputV, 0) * m_SpeedPanKey * 0.02f);
                }
            }

            // Follow
            float deltaTime = Time.unscaledDeltaTime;
            if (Mathf.Abs (_tarRotX - _curRotX) > 0.000000001f) {
                didChanged = true;
                _curRotX = smoothDampSafe (_curRotX, _tarRotX, ref _veloRotX, m_Damping, deltaTime);
            }
            if (Mathf.Abs (_tarRotY - _curRotY) > 0.000000001f) {
                didChanged = true;
                _curRotY = smoothDampSafe (_curRotY, _tarRotY, ref _veloRotY, m_Damping, deltaTime);
            }
            if (Mathf.Abs (_tarDist - _curDist) > 0.000000001f) {
                didChanged = true;
                _curDist = smoothDampSafe (_curDist, _tarDist, ref _veloDist, m_Damping, deltaTime);
            }
            if (Vector3.SqrMagnitude (_tarPos - transform.position) > 0.000001f) {
                didChanged = true;
                transform.position = smoothDampSafe (transform.position, _tarPos, ref _veloPos, m_Damping, deltaTime);
            }

            if (didChanged) {
                Vector3 dir = Quaternion.Euler (_curRotX, _curRotY, 0f) * Vector3.forward;
                Vector3 newPos = transform.position - dir * _curDist;
                m_Camera.transform.position = newPos;
                m_Camera.transform.LookAt (m_LookTarget ? m_LookTarget : transform);
            }
        }

    }
}
