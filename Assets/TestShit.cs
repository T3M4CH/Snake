using Game.TimeService.Interfaces;
using Mirror;
using TMPro;
using UnityEngine;
using Zenject;

public class TestShit : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private ITimeService _timeService;

    [Inject]
    private void Construct(ITimeService timeService)
    {
        _timeService = timeService;
    }

    public override void OnStartServer()
    {
        _timeService.OnTick += ChangeColor;
    }

    [Server]
    private void ChangeColor() => ChangeText(Random.ColorHSV());

    [ClientRpc]
    private void ChangeText(Color color) => text.color = color;
}