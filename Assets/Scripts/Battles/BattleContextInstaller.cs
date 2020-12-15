﻿using Battles.Entities;
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

        public override void InstallBindings()
        {
            Container.DeclareSignal<PlayerMovedSignal>();
            Container.DeclareSignal<ShotAttemptSignal>();
            Container.DeclareSignal<SpawnProjectileSignal>();

            Container.BindInterfacesAndSelfTo<EditorControls>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileManager>().AsSingle();

            Container.Bind<IPlayerConfiguration>().FromInstance(playerConfiguration).AsSingle();
        }
    }
}