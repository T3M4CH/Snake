using System;
using Multiplayer.Snake.Interfaces;
using UnityEngine;

namespace Multiplayer.Snake
{
    [Serializable]
    public class SerializableSpawnSettings : ISpawnSettings
    {
        [field: SerializeField]
        public Vector2[] SpawnPositions
        {
            get;
            private set;
        }
    }
}
