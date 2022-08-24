using System;
using Game.Apple.Interfaces;
using Game.Snake;
using Game.TickController.Interfaces;
using Mirror;
using Multiplayer.UI.Interfaces;
using UnityEngine;
using Zenject;

namespace Multiplayer.Snake
{
    public class NetSnake : NetworkBehaviour
    {
        [SerializeField] private TailMovement tailMovement;

        [SyncVar(hook = nameof(SetColor))] private Color _color;

        [SyncVar(hook = nameof(SpawnPosition))]
        private Vector2 _spawnPosition;

        private NetManager _netManager;
        private int _losersCount;
        private ILosePanel _losePanel;
        private ITimeService _timeService;
        private IRefereeService _refereeService;
        private NetworkIdentity _networkIdentity;

        [Inject]
        private void Construct(ILosePanel losePanel, ITimeService timeService, IRefereeService refereeService)
        {
            _losePanel = losePanel;
            _timeService = timeService;
            _refereeService = refereeService;
        }

        public void SetColor(Color oldColor, Color color)
        {
            _color = color;
            GetComponent<SpriteRenderer>().color = _color;
        }

        public void SpawnPosition(Vector2 oldPosition, Vector2 newPosition)
        {
            _spawnPosition = newPosition;
        }

        [ServerCallback]
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out IPickable pickable))
            {
                pickable.Pick();
                tailMovement.Add(gameObject);
            }

            if (col.CompareTag("Snake"))
            {
                if (!isServer) return;
                _refereeService.Print(_networkIdentity.netId);
                tailMovement.StopMovement();
            }
        }

        private void Restart()
        {
            tailMovement.RemoveTail();
            transform.position = _spawnPosition;
        }

        public override void OnStartServer()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
            _refereeService.Add(_networkIdentity);
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                _losePanel.Restart += Restart;
            }
        }
    }
}