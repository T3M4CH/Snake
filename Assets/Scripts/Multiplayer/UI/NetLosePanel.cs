using System;
using Game.TickController.Interfaces;
using Mirror;
using Multiplayer.Snake.Interfaces;
using Multiplayer.UI.Interfaces;
using Multiplayer.UI.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Multiplayer.UI
{
    public class NetLosePanel : NetworkBehaviour, ILosePanel
    {
        [SerializeField] private Image panel;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextMeshProUGUI restartText;

        private int _playersConfirm;
        private INetManager _netManager;

        public event Action Restart = () => { };

        [Inject]
        public void Construct(INetManager netManager)
        {
            _netManager = netManager;
        }

        public void Open(EPlayerStates stateValue)
        {
            panel.gameObject.SetActive(true);
            text.text = $"You're {stateValue}";
        }

        public override void OnStartServer()
        {
            _netManager.OnClientInteract += RestartCheck;
        }

        public override void OnStartClient()
        {
            exitButton.onClick.AddListener(() =>
            {
                if (NetworkServer.active && NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopHost();
                }
                else if (NetworkClient.isConnected)
                {
                    NetworkManager.singleton.StopClient();
                }
                else if (NetworkServer.active)
                {
                    NetworkManager.singleton.StopServer();
                }
                
                panel.gameObject.SetActive(false);
            });

            restartButton.onClick.AddListener(() =>
            {
                exitButton.interactable = false;
                restartButton.interactable = false;
                CmdRestart();
            });
        }

        [Command(requiresAuthority = false)]
        private void CmdRestart()
        {
            _playersConfirm += 1;
            RpcSyncText(_playersConfirm, PlayerCount);
            RestartCheck();
        }

        [Server]
        private void RestartCheck()
        {
            if (_playersConfirm != PlayerCount) return;
            Restart.Invoke();
            RpcRestartCall();
            _playersConfirm = 0;
        }

        [ClientRpc]
        private void RpcSyncText(int count, int playerCount)
        {
            restartText.text = $"{count} / {playerCount}";
        }

        [ClientRpc]
        private void RpcRestartCall()
        {
            if (isClientOnly)
            {
                Restart.Invoke();
            }

            exitButton.interactable = true;
            restartButton.interactable = true;
            panel.gameObject.SetActive(false);
            restartText.text = "Restart";
        }

        private int PlayerCount => _netManager.PlayerCount;
    }
}