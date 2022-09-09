using Game.TickController.Interfaces;
using Multiplayer.Snake.Interfaces;
using Multiplayer.UI.Interfaces;
using System.Collections.Generic;
using Multiplayer.UI.Enums;
using Zenject;
using Mirror;

namespace Multiplayer
{
    public class NetReferee : NetworkBehaviour, IRefereeService
    {
        private int _loserCount;
        private ITimeService _timeService;
        private INetManager _netManager;
        private ILosePanel _losePanel;
        private uint _playerId;
        private readonly List<uint> _playersNames = new();
        private readonly List<uint> _loserNames = new();

        [Inject]
        private void Construct(ITimeService timeService, ILosePanel losePanel, INetManager netManager)
        {
            _netManager = netManager;
            _timeService = timeService;
            _losePanel = losePanel;
        }

        public override void OnStartServer()
        {
            _timeService.OnRealtimeTick += AnnounceResult;
        }

        [Server]
        public void Add(NetworkIdentity player)
        {
            var netId = player.netId;
            _playersNames.Add(netId);
            RpcAdd(netId);
        }

        [ClientRpc]
        private void RpcAdd(uint id)
        {
            if (_playerId == default)
            {
                _playerId = id;
            }
        }

        [Server]
        public void Print(uint id)
        {
            _loserNames.Add(id);
            _loserCount += 1;
            _timeService.ChangeState(false);
        }

        private void AnnounceResult()
        {
            if (_loserCount == 0) return;
            RpcAnnounce(_loserNames, _netManager.PlayerCount);
            _loserNames.Clear();
            _loserCount = 0;
        }

        [ClientRpc]
        private void RpcAnnounce(List<uint> losers, int count)
        {
            if (count == 2 && losers.Count == count)
            {
                _losePanel.Open(EPlayerStates.Draw);
                return;
            }

            _losePanel.Open(losers.Contains(_playerId) ? EPlayerStates.Lose : EPlayerStates.Win);
        }
    }
}