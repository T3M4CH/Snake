using System;
using Game.Apple.Interfaces;
using Game.Scene;
using Mirror;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class MonoSnake : MonoBehaviour
    {
        private TailMovement _tailMovement;
        private SignalBus _signalBus;

        // [Inject]
        // private void Construct(SignalBus signalBus, SnakeMovement.Factory movementFactory,
        //     TailMovement.Factory tailFactory)
        // {
        //     var snakeMovement = movementFactory.Create(true,transform);
        //     _tailMovement = tailFactory.Create(transform, new NetworkConnectionToClient(0), snakeMovement);
        //     _signalBus = signalBus;
        // }

        [ServerCallback]
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IPickable pickable))
            {
                pickable.Pick();
                //_tailMovement.Add(this);
            }

            if (col.CompareTag("Snake"))
            {
                _signalBus.AbstractFire<PlayerDiedSignal>();
                //_tailMovement.Dispose();
            }
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                _signalBus.AbstractFire<PlayerDiedSignal>();
            }
        }
    }
}