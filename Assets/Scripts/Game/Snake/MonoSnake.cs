using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class MonoSnake : MonoBehaviour
    {
        [Inject]
        private void Construct(SnakeMovement.Factory movementFactory)
        {
            movementFactory.Create(transform);
        }
    }
}
