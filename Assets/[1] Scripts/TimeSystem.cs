using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private soDATA_TimeSystemBalanceData balanceData;

    [SerializeField] private UnityEvent unityEventOnNightStart;
    [SerializeField] private UnityEvent unityEventOnNightEnd;

    private event Action onNightStart;
    private event Action onNightEnd;

    private IEnumerator currentNightTimer;

    void Awake(){ Setup(); }
    public void Setup(){  }

    [Button]
    public void StartNight()
    {
        if (currentNightTimer != null){ Log("Night Ongoing"); return; }

        StartCoroutine(COROUTINE_Night(balanceData.STAT_nightLength));
    }

    private IEnumerator COROUTINE_Night(float length)
    {
        unityEventOnNightStart.Invoke();
        onNightStart?.Invoke();

        yield return new WaitForSeconds(length);

        unityEventOnNightEnd.Invoke();
        onNightEnd?.Invoke();
        yield break;
    }
}