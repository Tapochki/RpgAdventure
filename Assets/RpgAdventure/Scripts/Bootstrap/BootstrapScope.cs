using TandC.RpgAdventure.Services;
using VContainer;
using VContainer.Unity;

namespace TandC.RpgAdventure.Bootstrap
{
    public sealed class BootstrapScope : LifetimeScope
    {
        protected override void Awake()
        {
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Scoped);
            builder.Register<DataService>(Lifetime.Singleton).AsSelf();
            builder.Register<SceneManager>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}