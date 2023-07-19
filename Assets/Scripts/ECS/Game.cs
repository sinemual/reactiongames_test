using System.Collections;
using System.Collections.Generic;
using Client.Data.Core;
using Client.ECS.CurrentGame.Mining;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;
#if UNITY_EDITOR
using Leopotam.Ecs.UnityIntegration;
#endif

namespace Client
{
    internal sealed class Game : MonoBehaviour, ICoroutineRunner
    {
        [Header("Data")] [SerializeField] private SharedData _data;
        [Header("UI")] [SerializeField] private GameUI _ui;
        
        [SerializeField] private SceneData _sceneData;

        [Header("Services")] 
        private CameraService _cameraService;
        private ISaveLoadService _saveLoadService;
        private PrefabFactory _prefabFactory;
        private UserInterfaceEventBus _uiEventBus;
        private PoolService _poolService;
        private CleanService _cleanService;
        
        private EcsWorld _ecsWorld;

        private EcsSystems _updateSystems;
        private EcsSystems _lateUpdateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Awake()
        {
            StartCoroutine(ManualStart());
        }

        private IEnumerator ManualStart()
        {
            _ecsWorld = new EcsWorld();
            _updateSystems = new EcsSystems(_ecsWorld, " - UPDATE");
            _lateUpdateSystems = new EcsSystems(_ecsWorld, " - LATE UPDATE");
            _fixedUpdateSystems = new EcsSystems(_ecsWorld, " - FIXED UPDATE");

#if UNITY_EDITOR
            EcsWorldObserver.Create(_ecsWorld);
            EcsSystemsObserver.Create(_updateSystems);
            EcsSystemsObserver.Create(_lateUpdateSystems);
            EcsSystemsObserver.Create(_fixedUpdateSystems);
#endif
            //---Services---
            _poolService = new PoolService(_data);
            _cleanService = new CleanService(_poolService);
            _cameraService = new CameraService(_sceneData.CameraSceneData, this);
            _prefabFactory = new PrefabFactory(_ecsWorld, null, _poolService, _cleanService);
            _uiEventBus = new UserInterfaceEventBus();
            _saveLoadService = new JsonSaveLoadService();
            _data.ManualStart(_saveLoadService);

            //---Injects---
            _ui.Inject(_data, _uiEventBus);

            SetTargetFrameRate();
            ProvideMonoEntitiesFromScene();

            //---SystemGroups---
            var spawnSystems = SpawnSystems();
            var movementSystems = MovementSystems();
            var timerSystems = TimerSystems();
            var userInterfaceSystems = UserInterfaceSystems();

            _updateSystems
                //---GameState---
                .Add(new InitGameSystem())
                //---General---
                .Add(new DisposeLevelSystem())
                .Add(timerSystems)
                .Add(spawnSystems)
                .Add(movementSystems)
                .Add(userInterfaceSystems)
                //---Init---
                .Add(new InitLevelSystem())
                .Add(new InitPlayerUnitStatsSystem())
                .Add(new CompleteInitPlayerUnitSystem())
                .Add(new InitEnemyBaseStatSystem())
                .Add(new InitKamikazeEnemySystem())
                .Add(new InitTurretEnemySystem())
                //---Enemies---
                .Add(new DetectPlayerUnitSystem())
                .Add(new CompleteInitEnemySystem())
                .Add(new KamikazeChasePlayerSystem())
                .Add(new KamikazeDestroySystem())
                .Add(new PatrolDestroySystem())
                .Add(new TurretDetectPlayerSystem())
                .Add(new TurretShootPlayerSystem())
                .Add(new BulletDamagePlayerSystem())
                //---Player---
                .Add(new PlayerUnitTakeDamageSystem())
                .Add(new PlayerUnitExitSystem())
                .Add(new SelectNextPlayerUnitSystem())
                .Add(new NextSelectPlayerUnitSystem())
                .Add(new SelectPlayerUnitSystem())
                .Add(new InputUnityAxisSystem())
                //---LevelState---
                .Add(new DeathSystem())
                .Add(new LevelCompleteCheckSystem())
                .Add(new LevelFailSystem())
                .Add(new PauseSystem())
                //---SaveLoad---
                .Add(new LoadUnitsSystem())
                .Add(new SaveLoadSystem())
                //---OneFrames---
                .OneFrame<PlayerUnitDetectedEvent>()
                .OneFrame<LevelCompleteEvent>()
                .OneFrame<LevelFailedEvent>()
                .OneFrame<DeadEvent>()
                .OneFrame<MovingCompleteEvent>()
                //---Injects---
                .Inject(this) // for coroutine runner
                .Inject(_uiEventBus)
                .Inject(_data)
                .Inject(_ui)
                .Inject(_cameraService)
                .Inject(_prefabFactory)
                .Init();

            _lateUpdateSystems
                .Init();

            _fixedUpdateSystems
                .Add(new PlayerCharacterMovementControllerSystem())
                .Add(new PhysicForceAddSystem())
                //---OneFrames---
                .OneFrame<OnCollisionEnterEvent>()
                .OneFrame<OnTriggerEnterEvent>()
                .OneFrame<OnTriggerExitEvent>()
                .OneFrame<RaycastEvent>()
                //---Injects---
                .Inject(_ui)
                .Inject(_data)
                .Init();

            yield return null;
        }

        private void Update() => _updateSystems?.Run();

        private void LateUpdate() => _lateUpdateSystems?.Run();

        private void FixedUpdate() => _fixedUpdateSystems?.Run();

        private void OnDestroy()
        {
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;

                _lateUpdateSystems.Destroy();
                _lateUpdateSystems = null;

                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;

                _ecsWorld.Destroy();
                _ecsWorld = null;
            }
        }

        //------------------SYSTEM GROUPS---------------
        private EcsSystems SpawnSystems()
        {
            return new EcsSystems(_ecsWorld, "SpawnSystems")
                .Add(new DespawnAtTimerSystem())
                .Add(new SpawnLevelSystem())
                .Add(new SpawnPointSystem());
        }

        private EcsSystems MovementSystems()
        {
            return new EcsSystems(_ecsWorld, "MovementSystems")
                .Add(new InitPathMovementSystem())
                .Add(new TransformMagnetMovingSystem())
                .Add(new TransformMovingSystem())
                .Add(new PathMovementSystem());
        }

        private EcsSystems TimerSystems()
        {
            return new EcsSystems(_ecsWorld, "TimerSystems")
                .Add(new TimerSystem<TimerToEnable>())
                .Add(new TimerSystem<TimerToDisable>())
                .Add(new TimerSystem<TimerToDeadDespawn>())
                .Add(new TimerSystem<ReloadingTimer>())
                .Add(new TimerSystem<DespawnTimer>());
        }

        private EcsSystems UserInterfaceSystems()
        {
            return new EcsSystems(_ecsWorld, "UserInterfaceSystems")
                .Add(new PauseScreenInputSystem())
                .Add(new UnitsScreenInputSystem())
                .Add(new LevelCompleteScreenInputSystem())
                .Add(new LevelFailedScreenInputSystem());
        }

        private static void SetTargetFrameRate() => Application.targetFrameRate = 60;

        private void ProvideMonoEntitiesFromScene()
        {
            List<MonoEntity> monoEntities = new List<MonoEntity>();
            monoEntities.AddRange(FindObjectsOfType<MonoEntity>(true));
            foreach (var monoEntity in monoEntities)
            {
                var ecsEntity = _ecsWorld.NewEntity();
                monoEntity.Provide(ref ecsEntity);
            }
        }
    }
}