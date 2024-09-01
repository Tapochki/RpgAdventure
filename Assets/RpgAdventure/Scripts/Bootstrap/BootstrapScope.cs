using TandC.RpgAdventure.Services;
using TandC.RpgAdventure.Utilities;
using VContainer;
using VContainer.Unity;

namespace TandC.RpgAdventure.Bootstrap
{
    public sealed class BootstrapScope : LifetimeScope
    {
        protected override void Awake()
        {
          //  IsRoot = true;
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LoadingService>(Lifetime.Scoped);
            builder.Register<SceneManager>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}