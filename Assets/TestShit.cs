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
    private void ChangeColor() => ChangeText(new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f)));

    [ClientRpc]
    private void ChangeText(Color color) => text.color = color;
}