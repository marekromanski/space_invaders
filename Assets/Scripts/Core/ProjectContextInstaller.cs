using UnityEngine;
using Zenject;

namespace Core
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<DependenciesLoadedSignal>();

            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle().NonLazy();

            Debug.Log("Installed project bindings");
        }
    }
}