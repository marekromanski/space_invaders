using AssetManagement;
using Battles;
using Battles.UI;
using Highscores;
using Leaderboards;
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
            Container.DeclareSignal<LoadMainMenuSignal>();
            Container.DeclareSignal<NewHighScoreInsertedSignal>();
            Container.DeclareSignal<NewHighScoreSignal>();

            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HighScoresKeeper>().AsSingle().NonLazy();
            Container.Bind<IAssetCache>().FromInstance(assetCache).AsSingle();
            Container.BindInterfacesAndSelfTo<AssetFactory>().AsSingle().NonLazy();

            Debug.Log("Installed project bindings");
        }
    }
}