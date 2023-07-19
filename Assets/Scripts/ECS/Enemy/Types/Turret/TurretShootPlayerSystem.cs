using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.ECS.CurrentGame.Mining
{
    public class TurretShootPlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;
        
        private EcsFilter<EnemyTurretProvider, ShootingState>.Exclude<Timer<ReloadingTimer>> _shootFilter;

        public void Run()
        {
            foreach (var idx in _shootFilter)
            {
                ref var entity = ref _shootFilter.GetEntity(idx);
                ref var shootPoint = ref entity.Get<EnemyTurretProvider>().ShootPoint;
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var speed = ref entity.Get<SpeedStat>().Value;
                ref var damage = ref entity.Get<DamageStat>().Value;
                ref var targetEntity = ref entity.Get<ShootingState>().TargetEntity;
                
                var playerGo = targetEntity.Get<GameObjectProvider>().Value;
                var direction = (playerGo.transform.position) - shootPoint.position;
                entityGo.transform.LookAt(playerGo.transform.position);
                EcsEntity bulletEntity = _prefabFactory.Spawn(_data.StaticData.PrefabData.TurretBulletPrefab, shootPoint.position, Quaternion.LookRotation(direction, Vector3.up));
                bulletEntity.Get<AddForce>() = new AddForce()
                {
                    Direction = direction * 3.0f,
                    ForceMode = ForceMode.Impulse
                };
                bulletEntity.Get<DamageStat>().Value = damage;
                entity.Get<Timer<ReloadingTimer>>().Value = speed;
            }
        }
    }
}