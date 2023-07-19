using Client.Data;
using Client.Data.Core;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitPathMovementSystem : IEcsRunSystem
    {
        private EcsFilter<PathMovementProvider, SpeedStat>.Exclude<HasPath> _moverFilter;
        private EcsFilter<PathProvider>.Exclude<InUseMarker> _pathFilter;

        public void Run()
        {
            foreach (var mover in _moverFilter)
            {
                ref var moverEntity = ref _moverFilter.GetEntity(mover);
                ref var moverGo = ref moverEntity.Get<GameObjectProvider>();
                ref var moverSpawnPointProvider = ref moverEntity.Get<SpawnData>();
                ref var speed = ref moverEntity.Get<SpeedStat>().Value;
                
                foreach (var path in _pathFilter)
                {
                    ref EcsEntity pathEntity = ref _pathFilter.GetEntity(path);
                    ref PathProvider pathProvider = ref pathEntity.Get<PathProvider>();

                    if(pathProvider.SpawnPointMonoProvider.Value.Value != moverSpawnPointProvider.SpawnPointProvider.Value)
                        continue;
                    
                    moverGo.Value.transform.position = pathProvider.Value[0].position;
                    
                    moverEntity.Get<HasPath>() = new HasPath()
                    {
                        CurrentPathPointIndex = 0,
                        Path = pathProvider,
                        CompleteRadius = 0.1f,
                        MovingSpeed = speed
                    };
                    
                    moverEntity.Get<StartMovingRequest>();
                    pathEntity.Get<InUseMarker>();
                    break;;
                }
            }
        }
    }
}