using TandC.RpgAdventure.Services;
using VContainer;
using VContainer.Unity;

namespace TandC.RpgAdventure.MainMenu
{
    public sealed class MainMenuScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<UIService>(Lifetime.Scoped).As<IUIService>();

            builder.RegisterEntryPoint<MainMenuFlow>();
        }
    }
}