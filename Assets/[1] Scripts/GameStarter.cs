using UnityEngine;
using UnityEngine.Events;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private UnityEvent unityEventOnGameStart;

    void Awake(){ Setup(); }
    public void Setup(){  }

    public void StartGame()
    {
        unityEventOnGameStart.Invoke();
    }
}