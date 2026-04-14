using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class GirlfriendAI : MonoBehaviour
{
    public enum AIState {Patrol, Chase, Investigate, QTE, Stun} //Stun is kinda part of QTE -the pass outcome-
    public UnityEvent onCatchPlayer; //did not really use events like this before but seems like a good way?

    [Header("References")]
    public Transform player;
    public Transform[] waypoints;
    public QTEManager qteManager;

    [Header("Detection")]
    public float sightRange = 10f;
    public float sightAngle = 200f;
    public float catchDistance = 1.6f;
    public LayerMask obstacleMask;

    [Header("Speeds")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 5f;
    public float investigateSpeed = 3f;

    [Header("Timers")]
    public float lostPlayerTimeout = 3f;
    public float investigateTimeout = 6f;
    public float lookInterval = 0.969f;
    public float stunDuration = 3f;

    bool isLookingAround = false;
    float lookTimer = 0f;
    Quaternion lookTarget;
    float stunTimer;
    NavMeshAgent agent;
    int waypointIndex = 0;
    float lostTimer;
    float investigateTimer;
    Vector3 investigateTarget;
    [SerializeField] AIState currentState = AIState.Patrol;
    public AIState CurrentState => currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = catchDistance;
        
        GoToNextWaypoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol: UpdatePatrol(); break;
            case AIState.Chase: UpdateChase(); break;
            case AIState.Investigate: UpdateInvestigate(); break;
            case AIState.QTE: break;
            case AIState.Stun: UpdateStun(); break;
        }
    }

    #region -- PATROL ----------------------------------------------
    void UpdatePatrol()
    {
        agent.speed = patrolSpeed;

        if (CanSeePlayer()) { EnterChase(); return; }

        if (!agent.pathPending && agent.hasPath && agent.remainingDistance <= 1.69f)
            GoToNextWaypoint();
    }

    void GoToNextWaypoint()  //-------------------------------------------------------------------------- Not sure on the waypoint system something like that is what i tried before but not with navmesh
    {
        isLookingAround = false;
        agent.isStopped = false;
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[waypointIndex].position);
        waypointIndex = (waypointIndex + 1) % waypoints.Length; //the modulus loops it *** we can consider randomness as well ***
    }
    #endregion
    #region -- CHASE ------------------------------------------------
    void EnterChase()
    {
        isLookingAround = false;
        agent.isStopped = false;
        currentState = AIState.Chase; // Maybe here a ! or like a you are being chased FX sound etc. *******************
        agent.speed = chaseSpeed;
        lostTimer = lostPlayerTimeout;
    }

    void UpdateChase()
    {
        if (Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            EnterQTE(); return;  // Here we can have some transition sound or animation (maybe forcing player camera to turn etc) ******************
        }

        if (CanSeePlayer())
        {
            lostTimer = lostPlayerTimeout;
            agent.SetDestination(player.position);
        }
        else
        {
            lostTimer -= Time.deltaTime;
            if (lostTimer <= 0f)
            {
                investigateTarget = agent.destination;
                EnterInvestigate(); // it will just wait at the last sighted pos, some minor moving and looking around can be added (perhaps another state OR just to the investigate state) ******************
            }
        }
    }
    #endregion
    #region -- INVESTIGATE -------------------------------------------
    public void HearNoise(Vector3 noisePosition)
    {
        if (currentState == AIState.Chase || currentState == AIState.QTE) return;
        investigateTarget = noisePosition;
        EnterInvestigate();
    }

    void EnterInvestigate()
    {
        currentState = AIState.Investigate;
        agent.speed = investigateSpeed;
        investigateTimer = investigateTimeout;
        agent.SetDestination(investigateTarget);
    }

    void UpdateInvestigate()
    {
        if (CanSeePlayer()) { EnterChase(); return; }

        investigateTimer -= Time.deltaTime;
        if (investigateTimer <= 0f)
        {
            isLookingAround = false;
            currentState = AIState.Patrol;
            GoToNextWaypoint();
            return;
        }
        if (!agent.pathPending && agent.hasPath && agent.remainingDistance < 0.5f) // GF looks around at the noise spot (or the last pos if coming from chase)
        {
            agent.isStopped = true;
            isLookingAround = true;
        }
        if (isLookingAround)
        {
            lookTimer -= Time.deltaTime;
            if (lookTimer <= 0f)
            {
                lookTarget = Quaternion.Euler(0, Random.Range(0, 360), 0); //made it int bcs dont need this to be random float, but also maybe should make it better as this is too random***
                lookTimer = lookInterval;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, Time.deltaTime * 3f); //maybe can expose the 3f it is kinda the look rot spd
        }
    }
    #endregion
    #region -- QTE ------------------------------------------------
    void EnterQTE()
    {
        currentState = AIState.QTE;
        agent.isStopped = true;
        onCatchPlayer?.Invoke();
        qteManager.StartQTE(this);
    }

    public void OnPlayerEscapedQTE()
    {
        agent.isStopped = true;
        currentState = AIState.Stun;
        stunTimer = stunDuration;
        agent.enabled = false;
        StartCoroutine(StunBounce());
    }

    IEnumerator StunBounce()  //messy little effect - maybe cancel all coroutines somewhere idk, should be fine here but IF we will hard end the day somewhere else there we should kill this just incase mayhaps?
    {
        Vector3 bounceDir = (transform.position - player.position).normalized; //also vars not exposed***
        float elapsed = 0f;
        float bounceDuration = 0.4f;
        float bounceForce = 4f;

        while (elapsed < bounceDuration)
        {
            transform.position += bounceForce * Time.deltaTime * bounceDir;
            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.enabled = true;
        agent.isStopped = true;
    }
    void UpdateStun()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            agent.isStopped = false;
            currentState = AIState.Patrol;
            GoToNextWaypoint();
        }
    }
    #endregion

    bool CanSeePlayer()
    {
        Vector3 dir = player.position - transform.position;
        float dist = dir.magnitude;

        if (dist > sightRange) return false;
        if (Vector3.Angle(transform.forward, dir) > sightAngle * 0.5f) return false;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir.normalized, dist, obstacleMask)) return false;
        return true;
    }

    // -- GIZMOS -----------------------------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchDistance);
    }
}