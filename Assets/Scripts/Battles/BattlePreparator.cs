using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles
{
    public class BattlePreparator : MonoBehaviour
    {
        [SerializeField]
        private Transform playerSpawnPosition;
        
        private IPlayerFactory factory;
        private DiContainer diContainer;

        [Inject, UsedImplicitly]
        private void Construct(IPlayerFactory factory, DiContainer diContainer)
        { 
            this.factory = factory;
            this.diContainer = diContainer;
        }

        private void Start()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            factory.InstantiatePlayer(diContainer, playerSpawnPosition.position, Quaternion.identity);
        }
    }
}