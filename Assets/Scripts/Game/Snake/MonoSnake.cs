using Game.Apple.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class MonoSnake : MonoBehaviour
    {
        private TailMovement _tailMovement;
        
        [Inject]
        private void Construct(SnakeMovement.Factory movementFactory, TailMovement.Factory tailFactory)
        {
            var snakeMovement = movementFactory.Create(transform);
            _tailMovement = tailFactory.Create(transform,snakeMovement);
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IPickable pickable))
            {
                pickable.Pick();
                _tailMovement.Add();
            }

            if (col.CompareTag("Snake"))
            {
                _tailMovement.Dispose();
            }
        }
    }
}
