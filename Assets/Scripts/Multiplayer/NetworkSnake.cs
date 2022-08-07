using Game.Apple.Interfaces;
using Game.Scene;
using Game.Snake;
using Mirror;
using UnityEngine;
using Zenject;

namespace Multiplayer
{
    public class NetworkSnake : NetworkBehaviour
    {
        private TailMovement _tailMovement;
        private SnakeMovement.Factory _movementFactory;
        private TailMovement.Factory _tailFactory;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus, SnakeMovement.Factory movementFactory,
            TailMovement.Factory tailFactory)
        {
            _movementFactory = movementFactory;
            _tailFactory = tailFactory;
            _signalBus = signalBus;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IPickable pickable))
            {
                pickable.Pick();
            }

            if (col.CompareTag("Snake"))
            {
                _signalBus.AbstractFire<PlayerDiedSignal>();
                //_tailMovement.Dispose();
            }
        }

        private void Start()
        {
            var snakeMovement = _movementFactory.Create(isLocalPlayer, transform);
            _tailMovement = _tailFactory.Create(transform, isLocalPlayer, snakeMovement);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _signalBus.AbstractFire<PlayerDiedSignal>();
            }
        }
    }
}