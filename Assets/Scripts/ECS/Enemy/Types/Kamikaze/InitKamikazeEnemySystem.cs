using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitKamikazeEnemySystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private EcsFilter<EnemyKamikazeProvider, RangeStat>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var detectColliders = ref entity.Get<EnemyKamikazeProvider>().DetectCollidersRoot;
                ref var rangeStat = ref entity.Get<RangeStat>();
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                var scale = detectColliders.transform.localScale;
                var zScale = (Vector3.one * rangeStat.Value).z;
                detectColliders.transform.localScale = new Vector3(scale.x, scale.y, zScale);
                
                entityGo.GetComponent<MonoEntitiesContainer>().ProvideMonoEntityChildren(_world);
            }
        }
    }
}