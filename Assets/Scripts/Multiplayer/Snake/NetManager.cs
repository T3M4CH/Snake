using System;
using Game.TickController.Interfaces;
using Multiplayer.Snake.Interfaces;
using Multiplayer.UI;
using UnityEngine;
using Zenject;
using Mirror;

namespace Multiplayer.Snake
{
    public class NetManager : NetworkManager, INetManager
    {
        public event Action OnClientInteract = () => { };

        [SerializeField] private SessionService sessionService;
        private ISpawnSettings _spawnSettings;
        private ITimeService _timeService;
        private SetupSelector _selector;
        private CanvasHUD _canvasHUD;

        [Inject]
        private void Construct
        (
            ISpawnSettings spawnSettings,
            SetupSelector selector,
            ITimeService timeService,
            CanvasHUD canvasHUD
        )
        {
            _canvasHUD = canvasHUD;
            _spawnSettings = spawnSettings;
            _timeService = timeService;
            _selector = selector;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<CreateSnakeMessage>(OnCreateSnake);

            _timeService.OnChangeState += (value) => SendGameState();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

            OnClientInteract.Invoke();
            SendGameState();
        }

        private void SendGameState()
        {
            GameStateMessage message = new GameStateMessage()
            {
                Active = sessionService.IsStarted
            };
            NetworkServer.SendToAll(message);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            _canvasHUD.gameObject.SetActive(false);
            _canvasHUD.AddressText.text = singleton.networkAddress;
            NetworkClient.RegisterHandler<GameStateMessage>(ChangeReadyButton);
            _selector.SetActive(true);
            _selector.ReadyButton.onClick.AddListener(() =>
            {
                CreateSnakeMessage snakeMessage = new CreateSnakeMessage
                {
                    Color = _selector.SelectedColor
                };

                NetworkClient.connection.Send(snakeMessage);

                _selector.SetActive(false);
            });
        }

        public override void OnClientDisconnect()
        {
            _canvasHUD.gameObject.SetActive(true);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null) PlayerCount -= 1;
            if (!conn.identity.isClientOnly) return;

            OnClientInteract.Invoke();
            base.OnServerDisconnect(conn);
        }

        private void ChangeReadyButton(GameStateMessage stateMessage)
        {
            _selector.ReadyButton.interactable = !stateMessage.Active;
        }

        private void OnCreateSnake(NetworkConnectionToClient connection, CreateSnakeMessage message)
        {
            var position = _spawnSettings.SpawnPositions[PlayerCount];
            var player = Instantiate(playerPrefab, position, Quaternion.identity);

            var snake = player.GetComponent<NetSnake>();
            snake.SetColor(Color.black, message.Color);
            snake.SpawnPosition(Vector2.zero, position);

            NetworkServer.AddPlayerForConnection(connection, player);

            PlayerCount += 1;

            OnClientInteract.Invoke();
        }

        public int PlayerCount { get; private set; }
    }

    public struct CreateSnakeMessage : NetworkMessage
    {
        public Color Color;
    }

    public struct GameStateMessage : NetworkMessage
    {
        public bool Active;
    }
}