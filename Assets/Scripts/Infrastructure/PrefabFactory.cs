using System.Security.Cryptography;
using Client;
using Leopotam.Ecs;
using UnityEngine;

public class PrefabFactory
{
    private readonly EcsWorld _world;
    private Transform _defaultParent;
    private PoolService _poolService;
    private CleanService _cleanService;

    public PrefabFactory(EcsWorld world, Transform defaultParent, PoolService poolService, CleanService cleanService)
    {
        _world = world;
        _defaultParent = defaultParent;
        _poolService = poolService;
        _cleanService = cleanService;
    }

    public void SetDefaultParent(Transform parent)
    {
        _defaultParent = parent;
    }

    public EcsEntity Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        EcsEntity entity = _world.NewEntity();
        
        entity.Get<FromCurrentLevelTag>();
        
        if (parent == null)
            parent = _defaultParent;

        GameObject go = _poolService.IsPrefabHavePool(prefab) ? _poolService.GetGameObjectFromPool(prefab) : Object.Instantiate(prefab);

        go.transform.SetPositionAndRotation(position, rotation);
        go.transform.parent = parent;

        if (go.TryGetComponent(out MonoEntity monoEntity))
            monoEntity.Provide(ref entity);

        return entity;
    }

    public void SpawnWithEntity(ref EcsEntity entity, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (parent == null)
            parent = _defaultParent;

        GameObject go = _poolService.IsPrefabHavePool(prefab) ? _poolService.GetGameObjectFromPool(prefab) : Object.Instantiate(prefab);    

        go.transform.SetPositionAndRotation(position, rotation);
        go.transform.parent = parent;

        if (go.TryGetComponent(out MonoEntity monoEntity))
            monoEntity.Provide(ref entity);
    }

    public void Despawn(ref EcsEntity entity)
    {
        ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
        if (_poolService.IsGameObjectHavePool(entityGo))
            _cleanService.DespawnGameObject(entityGo);
        else
            _cleanService.DestroyGameObject(entityGo);
    }
    
    public void Despawn(GameObject go)
    {
        if (_poolService.IsGameObjectHavePool(go))
            _cleanService.DespawnGameObject(go);
        else
            _cleanService.DestroyGameObject(go);
    }
}