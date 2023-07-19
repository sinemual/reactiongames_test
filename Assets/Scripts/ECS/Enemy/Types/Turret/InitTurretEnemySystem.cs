using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitTurretEnemySystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private EcsFilter<EnemyTurretProvider, RangeStat>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var detectCollider = ref entity.Get<EnemyTurretProvider>().DetectCollider;
                ref var rangeStat = ref entity.Get<RangeStat>();
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                
                var scale = detectCollider.transform.localScale;
                var zScale = (Vector3.one * rangeStat.Value).z;
                var xScale = (Vector3.one * rangeStat.Value).x;
                detectCollider.transform.localScale = new Vector3(xScale, scale.y, zScale);
                
                entityGo.GetComponent<MonoEntitiesContainer>().ProvideMonoEntityChildren(_world);
            }
        }
    }
}