using UnityEngine;

namespace Battles
{
    public class BattleFieldDescriptor : MonoBehaviour, IBattleFieldDescriptor
    {
        [SerializeField]
        private Transform leftBorder;

        [SerializeField]
        private Transform rightBorder;

        [SerializeField]
        private Transform topSpawnBorder;

        [SerializeField]
        private Transform botSpawnBorder;

        [SerializeField]
        private Transform playerSpawnPosition;

        public float LeftBorder => leftBorder.position.x;
        public float RightBorder => rightBorder.position.x;
        public float TopSpawnBorder => topSpawnBorder.position.y;
        public float BotSpawnBorder => botSpawnBorder.position.y;
        public Vector3 PlayerSpawnPosition => playerSpawnPosition.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(new Vector3(LeftBorder, TopSpawnBorder, 0), new Vector3(LeftBorder, BotSpawnBorder, 0) );
            Gizmos.DrawLine(new Vector3(RightBorder, TopSpawnBorder, 0), new Vector3(RightBorder, BotSpawnBorder, 0) );
            Gizmos.DrawLine(new Vector3(LeftBorder, TopSpawnBorder, 0), new Vector3(RightBorder, TopSpawnBorder, 0) );
            Gizmos.DrawLine(new Vector3(LeftBorder, BotSpawnBorder, 0), new Vector3(RightBorder, BotSpawnBorder, 0) );
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(playerSpawnPosition.position, 0.5f);
        }
    }
}