using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class TurretDetectPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        private EcsFilter<EnemyTurretProvider, PlayerUnitDetectedEvent>.Exclude<DeadState, ShootingState> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var playerUnitGo = ref entity.Get<PlayerUnitDetectedEvent>().PlayerUnitGo;

                entityGo.transform.LookAt(playerUnitGo.transform.position, Vector3.up);
                entity.Get<ShootingState>().TargetEntity = playerUnitGo.GetComponent<MonoEntity>().Entity;
            }
        }
    }

    internal struct ReloadingTimer
    {
        public float Value;
    }
}