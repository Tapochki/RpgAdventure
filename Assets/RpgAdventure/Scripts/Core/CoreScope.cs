using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Core.Map;
using TandC.RpgAdventure.Core.Map.MapObject;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace TandC.RpgAdventure.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        [SerializeField] private GameObject _tilemapPrefab;
        [SerializeField] private TilemapView _tilemapView;
        [SerializeField] private FogOfWar _fogOfWar;
        [SerializeField] private Grid _grid;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private StructureConfig _structureConfig;
        [SerializeField] private GameObject _playerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(new TilemapFactory(_tilemapPrefab, _grid)).AsSelf();
            builder.RegisterInstance(new PlayerFactory(_playerPrefab)).AsSelf();

            builder.RegisterComponent(_tilemapView).AsSelf();
            builder.RegisterComponent(_fogOfWar).AsSelf();
            builder.RegisterInstance(_levelConfig).AsSelf();
            builder.RegisterInstance(_structureConfig).AsSelf();

            builder.Register<CameraService>(Lifetime.Scoped).As<ICameraService>();
            builder.Register<UIService>(Lifetime.Scoped).As<IUIService>();
            builder.Register<PlayerController>(Lifetime.Scoped);
            builder.Register<ClickDetector2D>(Lifetime.Scoped);
            builder.Register<LoadingService>(Lifetime.Scoped);
            builder.Register<TilemapViewModel>(Lifetime.Scoped);
            builder.Register<MapObjectViewModel>(Lifetime.Scoped);
            builder.Register<MapObjectView>(Lifetime.Scoped);

            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}