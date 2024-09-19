using System.ComponentModel;
using TandC.RpgAdventure.Config;
using TandC.RpgAdventure.Config.Player;
using TandC.RpgAdventure.Core.Item;
using TandC.RpgAdventure.Core.Map;
using TandC.RpgAdventure.Core.Map.MapObject;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Core.Player.Effect;
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
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private GameObject _playerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterComponents(builder);
            RegisterConfigs(builder);
            RegisterFactories(builder);
            RegisterServices(builder);
            RegisterViewModels(builder);
            RegisterEntryPoint(builder);
        }

        private void RegisterComponents(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tilemapView).AsSelf();
            builder.RegisterComponent(_fogOfWar).AsSelf();
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelConfig).AsSelf();
            builder.RegisterInstance(_itemConfig).AsSelf();
            builder.RegisterInstance(_structureConfig).AsSelf();
            builder.RegisterInstance(_characterConfig).AsSelf();
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.RegisterInstance(new TilemapFactory(_tilemapPrefab, _grid)).AsSelf();
            builder.Register<ItemEffectFactory>(Lifetime.Scoped);
            builder.Register<ItemFactory>(Lifetime.Scoped);
            builder.Register<PlayerFactory>(Lifetime.Scoped);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<CameraService>(Lifetime.Scoped).As<ICameraService>();
            builder.Register<UIService>(Lifetime.Scoped).As<IUIService>();
            builder.Register<LoadingService>(Lifetime.Scoped);
        }

        private void RegisterViewModels(IContainerBuilder builder)
        {
            builder.Register<PlayerViewModel>(Lifetime.Scoped);
            builder.Register<ClickDetector2D>(Lifetime.Scoped);
            builder.Register<TilemapViewModel>(Lifetime.Scoped);
            builder.Register<MapObjectViewModel>(Lifetime.Scoped);
            builder.Register<MapObjectView>(Lifetime.Scoped);
        }

        private void RegisterEntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}