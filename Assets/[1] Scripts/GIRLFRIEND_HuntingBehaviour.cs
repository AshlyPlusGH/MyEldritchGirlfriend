using UnityEngine;
using UnityEngine.AI;

public class GIRLFRIEND_HuntingBehaviour : MonoBehaviour
{
    [SerializeField] private bool debug;
        private void Log(string contents){ if (debug){ Debug.Log(contents + " at: " + name); }}

    [Space(10)]

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    void Awake(){ Setup(); }
    public void Setup(){  }
    
    void Update(){ agent.SetDestination(player.position); }
}