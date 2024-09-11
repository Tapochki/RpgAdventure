using System;
using UniRx;
using UnityEngine;

namespace TandC.RpgAdventure.Services
{

    public class CameraService: ICameraService
    {
        private Camera _camera;
        private Transform _playerTransform;
        private Vector3 _offset = new Vector3(0, 0, -10);
        private float _minZoom = 5f;
        private float _maxZoom = 10f;
        private float _zoomSpeed = 2f;
        private float _moveSpeed = 5f;

        private bool _isFollowingPlayer = true;

#if UNITY_ANDROID || UNITY_IOS
        private Vector2 _lastTouchPosition;
        private bool _isTouching = false;
        private float _touchDuration = 0f;
        private float _touchTimer = 0.5f;
#endif

        public void Init(Transform playerTransform)
        {
            _camera = Camera.main;
            _playerTransform = playerTransform;

            TrackPlayer();
            HandleZoom();
            HandleMovement();
        }

        private void TrackPlayer()
        {
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if (_isFollowingPlayer)
                    {
                        var targetPosition = _playerTransform.position + _offset;
                        _camera.transform.position = new Vector3(targetPosition.x, targetPosition.y, _camera.transform.position.z);
                    }
                }).AddTo(_camera.gameObject);
        }

        private void HandleZoom()
        {
#if UNITY_ANDROID || UNITY_IOS
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (Input.touchCount == 2)
                {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    float newSize = Mathf.Clamp(_camera.orthographicSize + deltaMagnitudeDiff * _zoomSpeed * Time.deltaTime, _minZoom, _maxZoom);
                    _camera.orthographicSize = newSize;
                }
            }).AddTo(_camera.gameObject);
#else
            Observable.EveryUpdate()
                .Select(_ => Input.GetAxis("Mouse ScrollWheel"))
                .Where(scroll => Math.Abs(scroll) > 0.01f)
                .Subscribe(scroll =>
                {
                    float newSize = Mathf.Clamp(_camera.orthographicSize - scroll * _zoomSpeed, _minZoom, _maxZoom);
                    _camera.orthographicSize = newSize;
                }).AddTo(_camera.gameObject);
#endif
        }

        private void HandleMovement()
        {
#if UNITY_ANDROID || UNITY_IOS
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        _lastTouchPosition = touch.position;
                        _isTouching = true;
                        _touchDuration = 0f;
                    }
                    else if (touch.phase == TouchPhase.Moved && _isTouching)
                    {
                        _touchDuration += Time.deltaTime;

                        if (_touchDuration > _touchTimer)
                        {
                            Vector2 touchDelta = touch.deltaPosition;
                            _camera.transform.Translate(-touchDelta.x * _moveSpeed * Time.deltaTime, -touchDelta.y * _moveSpeed * Time.deltaTime, 0);
                            _isFollowingPlayer = false;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        _isTouching = false;
                    }
                }

                if (Input.touchCount == 1 && Input.GetTouch(0).tapCount == 2)
                {
                    BackToPlayer();
                }
            }).AddTo(_camera.gameObject);
#else
            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    Vector3 direction = Vector3.zero;

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                        direction.y += 1;
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                        direction.y -= 1;
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                        direction.x -= 1;
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                        direction.x += 1;

                    if (direction != Vector3.zero)
                    {
                        _isFollowingPlayer = false;
                        _camera.transform.Translate(direction * _moveSpeed * Time.deltaTime);
                    }

                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        BackToPlayer();
                    }
                }).AddTo(_camera.gameObject);
#endif
        }

        public void BackToPlayer()
        {
            _isFollowingPlayer = true;
        }
    }
}
