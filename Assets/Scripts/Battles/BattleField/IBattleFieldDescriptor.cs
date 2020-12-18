using UnityEngine;

namespace Battles.BattleField
{
    public interface IBattleFieldDescriptor
    {
        float LeftBorder {get;}
        float RightBorder {get;}
        float TopSpawnBorder {get;}
        float BotSpawnBorder {get;}

        Vector3 PlayerSpawnPosition { get; }
    }
}