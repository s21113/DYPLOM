using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOSS : MonoBehaviour
{
    private int playersCollectibles;
    private float speed;
    private bool startedMoving = false;
    private float yPos;
    [SerializeField] private bool isDistracted;
    public GameObject eqSystem;
    private NavMeshAgent agencik;

    // definicja faz bossa
    #region PHASES
    public enum BossPhases
    {
        Idle,
        Roaming,
        Searching,
        Chasing,
        Unknown,
        Distracted
    }
    public BossPhases currentPhase;

    public void TriggerDistraction()
    {

    }

    /// <summary>
    /// Zmiana fazy bossa w zależności od ilości punktów gracza
    /// </summary>
    /// <returns>nowa faza bossa</returns>
    private BossPhases ChangePhase()
    {
        var distanceToPlayer = Vector3.Distance(this.transform.position, potentialPlayer.transform.position);
        if (playersCollectibles == 0)
        {
            return BossPhases.Idle;
        }
        else if (playersCollectibles >= 0 && playersCollectibles < 3)
        {
            StopCoroutine(MovementIdle());
            return BossPhases.Roaming;
        }
        else if (playersCollectibles >= 3 && playersCollectibles < 7)
        {
            StopCoroutine(MovementRoam());
            return BossPhases.Searching;
        }
        else if (playersCollectibles >= 7 || distanceToPlayer <= 16f)
        {
            StopCoroutine(MovementIdle());
            StopCoroutine(MovementRoam());
            StopCoroutine(MovementSearch());
            return BossPhases.Chasing;
        }
        else return BossPhases.Unknown;
    }
    #endregion

    #region WAYPOINTS
    private GameObject bossIdleWaypoint;
    private GameObject[] bossRoamWaypoints;
    private GameObject[] bossSearchWaypoints;
    #endregion

    #region MOVEMENTS
    /// <summary>
    /// ruch bezczynny, czyli poruszanie się w pobliżu pewnego punktu
    /// </summary>
    IEnumerator MovementIdle()
    {
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;
        agencik.speed = 0f;
        agencik.Warp(bossIdleWaypoint.transform.position);

        if (currentPhase != BossPhases.Idle) yield break;

        yield return new WaitForSeconds(20);
        startedMoving = false;
    }

    IEnumerator MovementRoam()
    {
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;
        agencik.speed = 1.5f;

        if (currentPhase != BossPhases.Roaming) yield break;

        var RWI = Random.Range(0, bossRoamWaypoints.Length); // Random Waypoint Index
        Vector3 randomMovement = new Vector3
        {
            x = Random.Range(0,2) + bossRoamWaypoints[RWI].transform.position.x,
            y = yPos,
            z = Random.Range(0,2) + bossRoamWaypoints[RWI].transform.position.z
        };
        agencik.SetDestination(randomMovement);
        yield return new WaitForSeconds(6);
        startedMoving = false;
    }

    IEnumerator MovementSearch()
    {
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;

        agencik.speed = 1.9f;

        if (currentPhase != BossPhases.Searching) yield break;

        var RWI = Random.Range(0, bossSearchWaypoints.Length); // Random Waypoint Index
        Vector3 randomMovement = new Vector3
        {
            x = Random.Range(-3,3) + bossSearchWaypoints[RWI].transform.position.x,
            y = yPos,
            z = Random.Range(-3,3) + bossSearchWaypoints[RWI].transform.position.z
        };

        /*agencik.SetDestination(randomMovement);
        Debug.Log("Search phase Finished first move");
        yield return new WaitForSeconds(9);
        {
            Vector3 moveAround = new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 5));
            moveAround += transform.position;
            agencik.SetDestination(moveAround);
            Debug.Log("Search phase Finished second move");
            yield return new WaitForSeconds(4);
        }
        {
            Vector3 moveAround = new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 5));
            moveAround += transform.position;
            agencik.SetDestination(moveAround);
            Debug.Log("Search phase Finished third move");
            yield return new WaitForSeconds(4);
        }*/

        {
            var oldPos = transform.position;
            float dist = Vector3.Distance(oldPos, randomMovement);
            Debug.Log($"Search phase Attempting to move to ({randomMovement.x}, {randomMovement.z})... distance: {dist}");
            while (dist >= 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, randomMovement, speed * Time.deltaTime);
                dist = Vector3.Distance(transform.position, randomMovement);
                yield return null;
            }
            Debug.Log("Search phase Finished first move");
            yield return new WaitForSeconds(9);
            startedMoving = false;
        }

        {
            Vector3 moveAround = new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 5));
            var oldPos = transform.position;
            moveAround += oldPos;
            float dist = Vector3.Distance(oldPos, moveAround);
            Debug.Log($"Search phase #2 Attempting to move to ({moveAround.x}, {moveAround.z})... distance: {dist}");
            while (dist >= 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveAround, speed * Time.deltaTime);
                dist = Vector3.Distance(transform.position, moveAround);
                yield return null;
            }
            Debug.Log("Search phase Finished second move");
            yield return new WaitForSeconds(4);
            startedMoving = false;
        }

        {
            Vector3 moveAround = new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 5));
            var oldPos = transform.position;
            moveAround += oldPos;
            float dist = Vector3.Distance(oldPos, moveAround);
            Debug.Log($"Search phase Attempting to move to ({moveAround.x}, {moveAround.z})... distance: {dist}");
            while (dist >= 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveAround, speed * Time.deltaTime);
                dist = Vector3.Distance(transform.position, moveAround);
                yield return null;
            }
            Debug.Log("Search phase Finished third move");
            yield return new WaitForSeconds(4);
            startedMoving = false;
        }
    }

    // preparados na szukanie
    [SerializeField] private GameObject potentialPlayer;
    // po prostu szukanie gracza i nagły boost do szybkości
    IEnumerator MovementChase()
    {
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;

        agencik.speed = 3f;

        AudioManager.instance.SwitchToChaseMusic();
        Vector3 playerPos = potentialPlayer.transform.position;
        var distanceToPlayer = Vector3.Distance(this.transform.position, potentialPlayer.transform.position);
        playerPos = potentialPlayer.transform.position;
        agencik.SetDestination(playerPos);
        yield return null;
        startedMoving = false;
    }

    /*IEnumerator MovementScared()
    {

    }*/
    #endregion

    private void SetMovement()
    {
        switch (currentPhase)
        {
            case BossPhases.Idle:
                startedMoving = true;
                StartCoroutine(MovementIdle());
                return;
            case BossPhases.Roaming:
                startedMoving = true;
                StartCoroutine(MovementRoam());
                return;
            case BossPhases.Searching:
                startedMoving = true;
                StartCoroutine(MovementSearch());
                return;
            case BossPhases.Chasing:
                startedMoving = true;
                StartCoroutine(MovementChase());
                return;
            default:
                startedMoving = false;
                return;
        }
    }



    private void Awake()
    {
        //potentialPlayer = GameObject.FindGameObjectWithTag("PlayerBody");
        bossIdleWaypoint = GameObject.FindGameObjectWithTag("BossIdleWaypoint");
        bossRoamWaypoints = GameObject.FindGameObjectsWithTag("BossRoamWaypoint");
        bossSearchWaypoints = GameObject.FindGameObjectsWithTag("BossSearchWaypoint");
        agencik = GetComponent<NavMeshAgent>();
    }


    private new Renderer renderer;
    public Shader shader;
    public Texture Texture, Texture2;
    public Texture m_MainTexture, m_Normal, m_Metal;

    void Start()
    {
        currentPhase = BossPhases.Unknown;
        yPos = transform.position.y;
        eqSystem = GameObject.Find("EqSystem");
    }

    void Update()
    {
        playersCollectibles = eqSystem.GetComponent<EqSystem>().GetImportantPoints();
        if (currentPhase != BossPhases.Distracted) currentPhase = ChangePhase();
        if (startedMoving == false)
        {
            SetMovement();
        }
        if (currentPhase == BossPhases.Chasing)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bambus"))
        {
            currentPhase = BossPhases.Distracted;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bambus"))
        {
            currentPhase = ChangePhase();
        }
    }

}
