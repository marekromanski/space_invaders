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

        public override void InstallBindings()
        {
            Container.DeclareSignal<PlayerMovedSignal>();
            Container.DeclareSignal<ShotAttemptSignal>();
            Container.DeclareSignal<SpawnProjectileSignal>();
            Container.DeclareSignal<ProjectileDestroyedSignal>();
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<EnemyDestroyedSignal>();

            Container.BindInterfacesAndSelfTo<EditorControls>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ProjectileSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle().NonLazy();

            Container.Bind<IPlayerConfiguration>().FromInstance(playerConfiguration).AsSingle();
            Container.Bind<IEnemiesConfiguration>().FromInstance(enemiesConfiguration).AsSingle();
        }
    }
}