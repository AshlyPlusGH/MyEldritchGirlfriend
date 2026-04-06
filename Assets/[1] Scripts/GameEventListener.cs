using Ash;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private soDATA_GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    void Awake()   => gameEvent.RegisterListener(response.Invoke);
    void OnDestroy() => gameEvent.UnregisterListener(response.Invoke);
}