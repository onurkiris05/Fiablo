using RPG.Interfaces;
using RPG.Managers;
using Zenject;

public class DefaultInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IUIManager>().To<UIManager>().FromComponentInHierarchy().AsSingle();
    }
}