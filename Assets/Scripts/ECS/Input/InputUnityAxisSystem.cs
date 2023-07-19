using Client;
using Leopotam.Ecs;
using UnityEngine;

public class InputUnityAxisSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
        
    private EcsFilter<PlayerInput, MovementInputTag> _filter;

    public void Init()
    {
        EcsEntity axisInputEntity = _world.NewEntity();
        axisInputEntity.Get<PlayerInput>() = new PlayerInput
        {
            xPosition = 0.0f,
            yPosition = 0.0f,
            IsDown = false
        };
        axisInputEntity.Get<MovementInputTag>();
    }

    public void Run()
    {
        foreach (var idx in _filter)
        {
            _filter.Get1(idx) = new PlayerInput
            {
                xPosition = Input.GetAxis("Horizontal"),
                yPosition = Input.GetAxis("Vertical"),
                IsDown = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f
            };
        }
    }
}