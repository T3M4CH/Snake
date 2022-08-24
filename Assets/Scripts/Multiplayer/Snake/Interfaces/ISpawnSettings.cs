using UnityEngine;

namespace Multiplayer.Snake.Interfaces
{
    public interface ISpawnSettings
    {
        Vector2[] SpawnPositions
        {
            get;
        }
    }
}
