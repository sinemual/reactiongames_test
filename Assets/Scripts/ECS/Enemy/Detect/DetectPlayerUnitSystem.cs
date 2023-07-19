using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class DetectPlayerUnitSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;

        private EcsFilter<DetectColliderProvider, OnTriggerEnterEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var detectColliderProvider = ref entity.Get<DetectColliderProvider>();
                ref var rootEntity = ref entity.Get<DetectColliderProvider>().RootEntity;
                ref var rootGo = ref rootEntity.Get<GameObjectProvider>().Value.Value;
                ref var onTriggerEnterEvent = ref entity.Get<OnTriggerEnterEvent>();

                if (onTriggerEnterEvent.Collider.TryGetComponent(out PlayerUnitMonoProvider _))
                {
                    var direction = onTriggerEnterEvent.Collider.transform.position - rootGo.transform.position;
                    if (Physics.Raycast(rootGo.transform.position, direction, out var hit, 100.0f))
                    {
                        if (hit.transform.TryGetComponent(out MonoEntity mono))
                        {
                            if (mono.Entity.Has<PlayerUnitProvider>())
                                detectColliderProvider.RootEntity.Entity.Get<PlayerUnitDetectedEvent>().PlayerUnitGo =
                                    onTriggerEnterEvent.Collider.gameObject;
                        }
                    }
                }
            }
        }
    }
}