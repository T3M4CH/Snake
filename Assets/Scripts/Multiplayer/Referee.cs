using System.Collections.Generic;
using Game.TickController.Interfaces;
using Mirror;
using Multiplayer.Snake.Interfaces;
using Multiplayer.UI.Enums;
using Multiplayer.UI.Interfaces;
using UnityEngine;
using Zenject;

namespace Multiplayer
{
    public class Referee : NetworkBehaviour, IRefereeService
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
            _timeService.OnTick += AnnounceResult;
            _timeService.OnChangeState += AnnounceCall;
        }

        [Server]
        private void AnnounceCall(bool value)
        {
            if (value)
            {
                _timeService.OnTick += AnnounceResult;
            }
            else
            {
                _timeService.OnTick -= AnnounceResult;
                _loserNames.Clear();
                _loserCount = 0;
            }
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
            Debug.Log(id);
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
        }

        private void AnnounceResult()
        {
            if (_loserCount == 0) return;
            _timeService.OnTick -= AnnounceResult; 
            RpcAnnounce(_loserNames, _netManager.PlayerCount);
            _timeService.ChangeState(false);
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