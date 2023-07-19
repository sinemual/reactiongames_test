using Client.Data.Core;
using Client.ECS.CurrentGame.Mining;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class DeathSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;

        private EcsFilter<DeadRequest> _deathFilter;

        public void Run()
        {
            foreach (var idx in _deathFilter)
            {
                ref var entity = ref _deathFilter.GetEntity(idx);

                entity.Get<DeadState>();
                _world.NewEntity().Get<DeadEvent>().Entity = entity;
                entity.Del<DeadRequest>();
            }
        }
    }
}