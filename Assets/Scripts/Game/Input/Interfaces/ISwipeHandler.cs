using System;

namespace Game.Input.Interfaces
{
    public interface ISwipeHandler
    {
        public event Action OnSwipeUp;
        public event Action OnSwipeDown;
        public event Action OnSwipeRight;
        public event Action OnSwipeLeft;
    }
}
