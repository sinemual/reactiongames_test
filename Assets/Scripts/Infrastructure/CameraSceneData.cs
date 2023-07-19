using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Client.Data
{
    [Serializable]
    public class CameraSceneData
    {
        public CinemachineVirtualCamera MainCamera;
        public CinemachineImpulseSource ShakeSource;
    }
}