using UnityEngine;
using System;

namespace Game.Input.Interfaces
{
    public interface ISwipeHandler
    {
        public event Action<Vector2Int> OnSwipe;
    }
}
