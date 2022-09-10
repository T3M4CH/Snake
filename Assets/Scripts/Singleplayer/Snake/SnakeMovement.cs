using Game.Input.Interfaces;
using UnityEngine;
using Zenject;

namespace Singleplayer.Snake
{
    public class SnakeMovement : MonoBehaviour
    {
        [Inject]
        public void Construct(ISwipeHandler swipeHandler)
        {
            _swipeHandler = swipeHandler;
        }

        private Vector2Int _direction;

        private ISwipeHandler _swipeHandler;

        public Vector3Int GetMovePoint(Vector2Int position)
        {
            if (position.x is > 5 or < -5) position.x *= -1;
            if (position.y is > 5 or < -5) position.y *= -1;
            return new Vector3Int(position.x + _direction.x, position.y + _direction.y);
        }

        private void CmdChangeDirection(Vector2Int direction)
        {
            Debug.Log("Change");
            if (_direction == direction * -1) return;
            _direction = direction;
        }

        private void Start()
        {
            _swipeHandler.OnSwipe += CmdChangeDirection;
            _direction = Vector2Int.up;
        }
    }
}