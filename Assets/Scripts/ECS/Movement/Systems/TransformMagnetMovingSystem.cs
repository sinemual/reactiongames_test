using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class TransformMagnetMovingSystem : IEcsRunSystem
    {
        private EcsFilter<TransformMagnetMoving> _movingFilter;

        public void Run()
        {
            if (_movingFilter.IsEmpty())
                return;

            foreach (var movingObject in _movingFilter)
            {
                ref var movingEntity = ref _movingFilter.GetEntity(movingObject);
                ref GameObjectProvider movingEntityGo = ref movingEntity.Get<GameObjectProvider>();

                ref var moving = ref movingEntity.Get<TransformMagnetMoving>();

                moving.Speed = moving.Speed == 0 ? 2 : moving.Speed;
                moving.Accuracy = moving.Accuracy == 0 ? 0.1f : moving.Accuracy;

                movingEntityGo.Value.transform.position = Vector3.MoveTowards(movingEntityGo.Value.transform.position,
                    moving.Target.transform.position, moving.Speed * Time.deltaTime);

                movingEntityGo.Value.transform.LookAt(moving.Target.transform.position);
                
                if (Vector3.Distance(movingEntityGo.Value.transform.position, moving.Target.transform.position) < moving.Accuracy)
                {
                    movingEntity.Del<TransformMoving>();
                    movingEntity.Get<MovingCompleteEvent>();
                }
            }
        }
    }
}