using Battles.Entities.Enemies;
using Battles.Entities.Player;
using Battles.Entities.Projectiles;
using UnityEngine;
using Zenject;

namespace Battles
{
    public class BattleContextInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerConfiguration playerConfiguration;

        [SerializeField]
        private EnemiesConfiguration enemiesConfiguration;

        [SerializeField]
        private BattleConfig battleConfig;

        [SerializeField]
        private BattleFieldDescriptor battleFieldDescriptor;

        public override void InstallBindings()
        {
            DeclareSignals();

            Container.BindInterfacesAndSelfTo<EditorControls>().AsSingle();

            Container.Bind<IBattleFieldDescriptor>().FromInstance(battleFieldDescriptor).AsSingle();

            BindSpawners();

            Container.BindInterfacesAndSelfTo<EnemiesManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle().NonLazy();

            BindConfigurations();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<PlayerMovedSignal>();
            Container.DeclareSignal<ShotAttemptSignal>();
            Container.DeclareSignal<SpawnProjectileSignal>();
            Container.DeclareSignal<ProjectileDestroyedSignal>();
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDestroyedSignal>();
        }

        private void BindSpawners()
        {
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<MotherShipSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<EliteEnemySpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<RegularEnemySpawner>().AsSingle();
        }

        private void BindConfigurations()
        {
            Container.Bind<IPlayerConfiguration>().FromInstance(playerConfiguration).AsSingle();
            Container.Bind<IEnemiesConfiguration>().FromInstance(enemiesConfiguration).AsSingle();
            Container.Bind<IBattleConfig>().FromInstance(battleConfig).AsSingle();
        }
    }
}