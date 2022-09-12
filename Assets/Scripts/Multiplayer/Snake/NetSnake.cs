using Game.TickController.Interfaces;
using Mirror;
using Multiplayer.Apple.Interfaces;
using Multiplayer.UI.Interfaces;
using UnityEngine;
using Zenject;

namespace Multiplayer.Snake
{
    public class NetSnake : NetworkBehaviour
    {
        [SerializeField] private NetTailMovement netTailMovement;

        [SyncVar(hook = nameof(SetColor))] private Color _color;

        [SyncVar(hook = nameof(SpawnPosition))]
        private Vector2 _spawnPosition;

        private int _losersCount;
        private NetManager _netManager;
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
            if (col.CompareTag("Snake"))
            {
                if (!isServer) return;
                _losersCount += 1;
                _refereeService.Print(_networkIdentity.netId);
                netTailMovement.RpcStopMovement();
            }
            
            if (col.TryGetComponent(out IConsumable pickable))
            {
                if (!isServer) return;
                if (_losersCount > 0) return;
                pickable.Pick();
                netTailMovement.Add(gameObject);
            }
        }

        private void Restart()
        {
            _losersCount = 0;
            netTailMovement.RemoveTail();
            transform.position = _spawnPosition;
        }

        public override void OnStartServer()
        {
            _networkIdentity = GetComponent<NetworkIdentity>();
            _refereeService.Add(_networkIdentity);
            _losePanel.Restart += Restart;
        }
    }
}