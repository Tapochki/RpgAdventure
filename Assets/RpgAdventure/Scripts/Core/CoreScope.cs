using TandC.RpgAdventure.Core.HexGrid;
using TandC.RpgAdventure.Core.Player;
using TandC.RpgAdventure.Services;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;
using VContainer.Unity;

namespace TandC.RpgAdventure.Core
{
    public sealed class CoreScope : LifetimeScope
    {
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private TilemapView _tileMapView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_tileMapView).AsSelf();

            builder.Register<ClickDetector2D>(Lifetime.Scoped);
            builder.Register<PlayerSpawner>(Lifetime.Scoped);
            builder.Register<LoadingService>(Lifetime.Scoped);
            builder.Register<TilemapViewModel>(Lifetime.Scoped).WithParameter(_tileMap);

            builder.RegisterEntryPoint<CoreFlow>();
        }
    }
}