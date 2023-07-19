using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class PathMovementSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<HasPath, StartMovingRequest> _startRequestFilter;
        private EcsFilter<HasPath, MovingCompleteEvent> _completeFilter;
        private EcsFilter<HasPath, StopMovingRequest> _stopRequestFilter;

        public void Run()
        {
            foreach (var idx in _startRequestFilter)
            {
                ref var entity = ref _startRequestFilter.GetEntity(idx);
                ref var hasPath = ref entity.Get<HasPath>();
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                entity.Get<TransformMoving>() = new TransformMoving()
                {
                    Accuracy = hasPath.CompleteRadius,
                    Speed = hasPath.MovingSpeed,
                    Target = hasPath.Path.Value[hasPath.CurrentPathPointIndex].position
                };
                
                entityGo.transform.LookAt(hasPath.Path.Value[hasPath.CurrentPathPointIndex].position);

                entity.Del<StartMovingRequest>();
                entity.Get<MovingState>();
            }

            foreach (var idx in _stopRequestFilter)
            {
                ref var entity = ref _stopRequestFilter.GetEntity(idx);

                StopMoving(ref entity);
                entity.Del<StopMovingRequest>();
            }

            foreach (var idx in _completeFilter)
            {
                ref var entity = ref _completeFilter.GetEntity(idx);
                ref var entityGo = ref entity.Get<GameObjectProvider>();
                ref var hasPath = ref entity.Get<HasPath>();


                hasPath.CurrentPathPointIndex++;
                StopMoving(ref entity);
                if (hasPath.CurrentPathPointIndex > hasPath.Path.Value.Count - 1)
                {
                    if (hasPath.Path.IsLoop)
                    {
                        hasPath.CurrentPathPointIndex = 0;
                        entity.Get<StartMovingRequest>();
                    }
                    else
                    {
                        continue;
                    }
                }

                entity.Get<StartMovingRequest>();
            }
        }

        private void StopMoving(ref EcsEntity entity)
        {
            entity.Del<TransformMoving>();
            entity.Del<MovingState>();
        }
    }
}