using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class PlayerCharacterMovementControllerSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<PlayerUnitProvider, SelectedUnitTag, RigidbodyProvider>.Exclude<DeadState> _filter;
        private EcsFilter<PlayerInput, MovementInputTag> _movementFilter;

        public void Run()
        {
            foreach (var player in _filter)
            foreach (var idx in _movementFilter)
            {
                ref var entity = ref _movementFilter.GetEntity(idx);

                ref var playerEntity = ref _filter.GetEntity(player);
                ref var playerGo = ref playerEntity.Get<GameObjectProvider>();
                ref var playerRb = ref playerEntity.Get<RigidbodyProvider>();
                
                ref var speed = ref playerEntity.Get<SpeedStat>().Value;
                
                var velX = entity.Get<PlayerInput>().xPosition;
                var velZ = entity.Get<PlayerInput>().yPosition;

                var isGettingInput = Mathf.Abs(velX) > 0.0f || Mathf.Abs(velZ) > 0.0f;
                
                var force = Vector3.forward * velZ + Vector3.right * velX + Vector3.up * playerRb.Value.velocity.y;

                playerRb.Value.velocity = force.normalized * speed * Time.deltaTime;

                Vector3 target = playerRb.Value.velocity;
                
                if (target == Vector3.zero)
                    continue;
                
                if (target.sqrMagnitude > 0.0f && isGettingInput)
                    playerGo.Value.transform.rotation = Quaternion.RotateTowards(playerGo.Value.transform.rotation,
                        Quaternion.LookRotation(target), _data.BalanceData.CharactersRotateSpeed * 100 * Time.deltaTime);
            }
        }
    }
}