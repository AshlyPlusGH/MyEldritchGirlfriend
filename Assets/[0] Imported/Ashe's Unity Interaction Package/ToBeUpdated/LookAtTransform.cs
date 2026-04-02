using UnityEngine;

[ExecuteInEditMode]
public class LookAtTransform : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private Transform target;

    void Awake(){ Setup(); }
    public void Setup(){  }

    void Update()
    {
        transform.LookAt(target);
    }
}