using System;
using System.Collections;
using Cinemachine;
using Client.Data;
using UnityEngine;
using CameraType = UnityEngine.CameraType;

namespace Client.Infrastructure.Services
{
    public class CameraService
    {
        private readonly CameraSceneData _cameraSceneData;
        private readonly ICoroutineRunner _coroutineRunner;

        private Transform _currentFollowTarget;
        private Transform _currentLookAtTarget;

        public CameraService(CameraSceneData cameraSceneData, ICoroutineRunner coroutineRunner)
        {
            _cameraSceneData = cameraSceneData;
            _coroutineRunner = coroutineRunner;
        }

        public void SetTarget(Transform followT, Transform lookAtT)
        {
            _currentFollowTarget = followT;
            _currentLookAtTarget = lookAtT;
                _cameraSceneData.MainCamera.Follow = _currentFollowTarget;
                _cameraSceneData.MainCamera.LookAt = _currentLookAtTarget;
        }
        public void Shake() => _cameraSceneData.ShakeSource.GenerateImpulse(_cameraSceneData.MainCamera.transform.forward);
        public Camera GetCamera() => Camera.main;
    }

    [Serializable]
    public class VirtualCamera
    {
        public string name;
        public CameraType cameraType;
        public CinemachineVirtualCamera cinemachineVirtualCamera;
    }
}