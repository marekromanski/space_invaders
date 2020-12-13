using AssetManagement;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField]
        private AssetCache assetCache;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<StartGameSignal>();
            Container.DeclareSignal<DependenciesLoadedSignal>();

            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle().NonLazy();
            Container.Bind<IAssetCache>().FromInstance(assetCache).AsSingle();
            Container.BindInterfacesAndSelfTo<AssetFactory>().AsSingle().NonLazy();

            Debug.Log("Installed project bindings");
        }
    }
}