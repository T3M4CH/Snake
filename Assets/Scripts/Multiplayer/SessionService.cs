using Game.TickController.Interfaces;
using Mirror;
using Multiplayer.Snake;
using Multiplayer.UI.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

public class SessionService : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private NetManager netManager;
    private ITimeService _timeService;
    private ILosePanel _losePanel;
    private const string Tick = "Tick";
    private bool _isTicking;
    private int _secondsLeft;

    [Inject]
    private void Construct(ITimeService timeService, ILosePanel losePanel)
    {
        _timeService = timeService;
        _losePanel = losePanel;
    }

    public override void OnStartServer()
    {
        _timeService.OnTick += ChangeColor;
        _losePanel.Restart += Restart;
        _timeService.OnRealtimeTick += UpdateTimer;
        netManager.OnClientInteract += CheckTimerState;
    }

    [Server]
    private void UpdateTimer()
    {
        if (!_isTicking) return;
        _secondsLeft -= 1;
        if (_secondsLeft < 1)
        {
            _isTicking = false;
            _timeService.ChangeState(true);
            text.text = Tick;
            RpcChangeText(Tick);
            return;
        }

        RpcChangeText(_secondsLeft.ToString());
    }

    private void CheckTimerState()
    {
        if (IsStarted) return;
        string value;
        switch (netManager.PlayerCount)
        {
            case > 1 when NetworkServer.connections.Count != netManager.PlayerCount:
                value = "10";
                _isTicking = true;
                _secondsLeft = 10;
                text.text = value;
                _timeService.ChangeState(false);
                break;
            case > 1 when NetworkServer.connections.Count == netManager.PlayerCount:
                value = Tick;
                text.text = value;
                _isTicking = false;
                if (IsStarted) return;
                _timeService.ChangeState(true);
                IsStarted = true;
                break;
            default:
                value = "Wait for players";
                text.text = value;
                break;
        }

        RpcChangeText(value);
    }

    private void Restart()
    {
        _timeService.ChangeState(false);
        IsStarted = false;
        CheckTimerState();
    }

    [Server]
    private void ChangeColor() => RpcChangeText(Random.ColorHSV());

    [ClientRpc]
    private void RpcChangeText(string value) => text.text = value;

    [ClientRpc]
    private void RpcChangeText(Color color) => text.color = color;

    public bool IsStarted { get; private set; }
}