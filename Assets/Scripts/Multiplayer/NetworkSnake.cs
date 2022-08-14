using System;
using Game.Apple.Interfaces;
using Game.Scene;
using UnityEngine;
using Game.Snake;
using Mirror;
using Zenject;

namespace Multiplayer
{
    public class NetworkSnake : NetworkBehaviour
    {
        [SerializeField] private TailMovement tailMovement;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        [ServerCallback]
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IPickable pickable))
            {
                pickable.Pick();
                tailMovement.CmdAdd(gameObject);
            }

            if (col.CompareTag("Snake"))
            {
                Debug.Log("Collision");
            }
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