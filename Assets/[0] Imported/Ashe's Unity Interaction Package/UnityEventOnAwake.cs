using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ash {
    public class UnityEventOnAwake : MonoBehaviour
    {
        [SerializeField] private bool debug;
            private void Log(string contents){ if (debug){ Debug.Log(contents); }}

        [Space(10)]

        [SerializeField] private float delay;
        [SerializeField] private UnityEvent unityEvent;

        [SerializeField] private bool runOnce;
        private static GenericDictionary<GUID,bool> hasRanData = new();

        private GUID guid;

        private void Awake()
        {
            StartCoroutine(COROUTINE_TriggerEvent());
        }

        private IEnumerator COROUTINE_TriggerEvent()
        {
                if (!hasRanData.ContainsKey(guid)){}
                else if (runOnce && hasRanData[guid]){ yield break; }

            if (hasRanData.ContainsKey(guid)){ hasRanData[guid] = true; }
            else { hasRanData.Add(new KeyValuePair<GUID, bool>(guid,true)); }

            yield return new WaitForSecondsRealtime(delay);

            unityEvent.Invoke();

            if (guid == null){ guid = GUID.Generate(); }
        }
    }
}