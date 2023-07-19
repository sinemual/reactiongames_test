using Client.Data.Core;
using Client.DevTools.MyTools;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private CameraService _cameraService;
        private PrefabFactory _prefabFactory;
        private UserInterfaceEventBus _uiEventBus;
        
        private EcsFilter<LevelProvider>.Exclude<InitedMarker> _levelFilter;
        
        private EcsFilter<PlayerUnitProvider> _unitFilter;

        public void Run()
        {
            foreach (var currentLevel in _levelFilter)
            {
                ref var entity = ref _levelFilter.GetEntity(currentLevel);
                ref var entityGo = ref entity.Get<GameObjectProvider>().Value;
                ref var level = ref entity.Get<LevelProvider>();

                entityGo.GetComponent<MonoEntitiesContainer>().ProvideMonoEntityChildren(_world);

                _ui.UnitsScreen.SetShowState(true);
                _data.PlayerData.SavedUnitsCounter = 0;
                _data.PlayerData.NeededSaveUnitsCount = 0;
                
                entity.Get<InitedMarker>();
                _world.NewEntity().Get<LevelInitEvent>();
            }
        }
    }
}