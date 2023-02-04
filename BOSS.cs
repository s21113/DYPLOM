using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOSS : MonoBehaviour
{
    private int playersCollectibles;
    private float speed;
    [SerializeField] private bool startedMoving = false;
    [SerializeField] private bool isDistracted;

    public GameObject eqSystem;
    public AudioSource chaseMusic;
    public AudioSource roarPlayer;
    private AudioSource soundPlayer;
    private NavMeshAgent agencik;
    private Animator pandaAnimator;
    private Camera bossCam;



    private bool PlayerInCloseRange()
    {
        if (GameSettings.ReadSettingsFromFile().invisible)
            return false;
        var scrP = bossCam.WorldToViewportPoint(potentialPlayer.transform.position);
        return (scrP.x > 0 && scrP.x < 0.8f && scrP.y > 0 && scrP.y < 0.5f && scrP.z >= 0 && scrP.z < 100);
    }

    private bool PlayerCanSeeMe()
    {
        var scrP = potentialPlayer.GetComponentInChildren<Camera>().WorldToViewportPoint(transform.position);
        return (scrP.x > 0 && scrP.x < 0.8f && scrP.y > 0 && scrP.y < 0.5f && scrP.z >= 0 && scrP.z < 100);
    }

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

    IEnumerator TriggerDistraction()
    {
        if (currentPhase != BossPhases.Distracted)
            yield break;
        agencik.speed = 0f;
        yield return new WaitForSeconds(5f);
        currentPhase = BossPhases.Roaming;
    }

    /// <summary>
    /// Zmiana fazy bossa w zależności od ilości punktów gracza
    /// </summary>
    /// <returns>nowa faza bossa</returns>
    private BossPhases ChangePhase()
    {
        if (PlayerInCloseRange())
        {
            return BossPhases.Chasing;
        }
        else
        {
            chaseMusicPlaying = false;
        }
        if (playersCollectibles == 0)
        {
            StopCoroutine(MovementChase());
            return BossPhases.Idle;
        }
        if (playersCollectibles > 0 && playersCollectibles < 4)
        {
            StopCoroutine(MovementIdle());
            StopCoroutine(MovementChase());
            return BossPhases.Roaming;
        }
        if (playersCollectibles >= 4 && playersCollectibles < 7)
        {
            StopCoroutine(MovementRoam());
            StopCoroutine(MovementChase());
            return BossPhases.Searching;
        }
        if (playersCollectibles >= 7 && !GameSettings.ReadSettingsFromFile().invisible)
        {
            StopCoroutine(MovementSearch());
            return BossPhases.Chasing;
        }
        return BossPhases.Unknown;
    }
    #endregion

    #region WAYPOINTS
    private GameObject bossIdleWaypoint;
    private GameObject[] bossRoamWaypoints;
    private GameObject[] bossSearchWaypoints;
    #endregion

    #region AUDIO

    private List<AudioClip> audioClips;
    [SerializeField] private AudioClip snoreClip;
    [SerializeField] private AudioClip stepClip;
    [SerializeField] private AudioClip wakeUpClip;
    [SerializeField] private AudioClip angerClip;
    [SerializeField] private AudioClip rageClip;
    [SerializeField] private AudioClip rageStepClip;

    private float walkSoundRate = 3f;
    [SerializeField] private bool playedWakeup = false;
    [SerializeField] private bool playedAnger = false;
    [SerializeField] private int playedRage = 0;

    private bool chaseMusicPlaying = false;

    IEnumerator PlayStepSounds()
    {
        while (isActiveAndEnabled)
        {
            soundPlayer.Play();
            yield return new WaitForSeconds(walkSoundRate);
        }
    }

    IEnumerator PlayRoarSound()
    {
        if (playedWakeup) yield break;
        playedWakeup = true;
        roarPlayer.PlayOneShot(wakeUpClip, 0.8f);
        yield return new WaitForSeconds(10f);
    }

    IEnumerator PlayAngryRoarSound()
    {
        if (playedAnger) yield break;
        playedAnger = true;
        roarPlayer.PlayOneShot(angerClip, 0.7f);
        yield return new WaitForSeconds(10f);
    }

    IEnumerator PlayRageQuitSound()
    {
        if (playedRage > 0) yield break;
        playedRage++;
        if (playedRage < 2)
            roarPlayer.PlayOneShot(rageClip, 0.5f);
        StopCoroutine(PlayRageQuitSound());
        yield break;
    }


    IEnumerator PlayChaseMusic()
    {
        if (chaseMusicPlaying == true) yield break;
        chaseMusicPlaying = true;
        chaseMusic.volume = 0.5f;
        chaseMusic.PlayDelayed(1f);
        yield break;
    }

    private void SelectSoundToPlay()
    {
        if (currentPhase != BossPhases.Chasing)
            chaseMusic.Stop();

        if (playedRage > 0) StopCoroutine(PlayRageQuitSound());

        switch (currentPhase)
        {
            case BossPhases.Idle:
                walkSoundRate = 4f;
                soundPlayer.clip = snoreClip;
                return;
            case BossPhases.Roaming:
                walkSoundRate = 3f;
                soundPlayer.clip = stepClip;
                StartCoroutine(PlayRoarSound());
                return;
            case BossPhases.Searching:
                walkSoundRate = 2f;
                soundPlayer.clip = stepClip;
                StartCoroutine(PlayAngryRoarSound());
                return;
            case BossPhases.Chasing:
                walkSoundRate = 2f;
                soundPlayer.clip = rageStepClip;
                StartCoroutine(PlayChaseMusic());
                if (playedRage < 1) StartCoroutine(PlayRageQuitSound());
                return;
            default:
                soundPlayer.clip = null;
                return;
        }
    }

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
        if (currentPhase != BossPhases.Idle) yield return null;

        agencik.speed = 0f;

        agencik.SetDestination(bossIdleWaypoint.transform.position);
        startedMoving = false;
        yield return new WaitForSeconds(1);
    }

    IEnumerator MovementRoam()
    {
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;
        if (currentPhase != BossPhases.Roaming) yield return null;
        agencik.isStopped = false;

        agencik.speed = 4f;

        var RWI = Random.Range(0, bossRoamWaypoints.Length); // Random Waypoint Index
        Vector3 randomMovement = bossRoamWaypoints[RWI].transform.position;
        agencik.SetDestination(randomMovement);
        yield return new WaitWhile(() => currentPhase == BossPhases.Roaming && agencik.hasPath);
        startedMoving = false;
        yield return new WaitForSeconds(1);
    }

    IEnumerator MovementSearch()
    {
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;
        if (currentPhase != BossPhases.Searching) yield return null;
        agencik.isStopped = false;

        var RWI = Random.Range(0, bossRoamWaypoints.Length); // Random Waypoint Index
        Vector3 randomMovement = bossRoamWaypoints[RWI].transform.position;
        if (bossSearchWaypoints.Length > 0 && Mathf.Abs(transform.position.y - randomMovement.y) >= 1f)
        {
            if (PlayerCanSeeMe())
                agencik.speed = 10f;
            else
                agencik.Warp(new List<GameObject>(bossSearchWaypoints).Find(x => x.transform.position.y >= randomMovement.y).transform.position);
        }
        agencik.speed = 4f + (playersCollectibles / 7);
        agencik.SetDestination(randomMovement);
        yield return new WaitWhile(() => currentPhase == BossPhases.Searching && agencik.hasPath);
        startedMoving = false;
        yield return new WaitForSeconds(1);
    }

    // preparados na szukanie
    [SerializeField] private GameObject potentialPlayer;

    // po prostu szukanie gracza i nagły boost do szybkości
    IEnumerator MovementChase()
    {
        agencik.isStopped = false;
        startedMoving = false;
        if (startedMoving == true) yield break;
        startedMoving = true;
        if (currentPhase != BossPhases.Chasing) yield return null;

        float speedF1 = Vector3.Distance(transform.position, potentialPlayer.transform.position) / 6;
        agencik.speed = ((10*playersCollectibles/14) + 2f) / (3/speedF1);

        while (currentPhase == BossPhases.Chasing)
        {
            agencik.SetDestination(potentialPlayer.transform.position);
            yield return null;
        }
        startedMoving = false;
    }

    private void SetMovement()
    {
        startedMoving = true;
        switch (currentPhase)
        {
            case BossPhases.Idle:
                StartCoroutine(MovementIdle());
                return;
            case BossPhases.Roaming:
                StartCoroutine(MovementRoam());
                return;
            case BossPhases.Searching:
                StartCoroutine(MovementSearch());
                return;
            case BossPhases.Chasing:
                startedMoving = false;
                StartCoroutine(MovementChase());
                return;
            default:
                startedMoving = false;
                return;
        }
    }

    #endregion



    private void Awake()
    {
        potentialPlayer = GameObject.FindGameObjectWithTag("PlayerBody");
        bossIdleWaypoint = GameObject.FindGameObjectWithTag("BossIdleWaypoint");
        bossRoamWaypoints = GameObject.FindGameObjectsWithTag("BossRoamWaypoint");
        bossSearchWaypoints = GameObject.FindGameObjectsWithTag("BossSearchWaypoint");
        agencik = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        audioClips = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sounds/Boss"));
        currentPhase = BossPhases.Unknown;
        soundPlayer = GetComponent<AudioSource>();

        snoreClip = audioClips.Find(a => a.name.Equals("spanko"));
        stepClip = audioClips.Find(a => a.name.Equals("krok"));
        rageStepClip = audioClips.Find(a => a.name.Equals("wkurzonykrok"));
        wakeUpClip = audioClips.Find(a => a.name.Equals("pobudka"));
        angerClip = audioClips.Find(a => a.name.Equals("wkurzenie"));
        rageClip = audioClips.Find(a => a.name.Equals("kurwica"));

        pandaAnimator = GetComponent<Animator>();
        bossCam = GetComponentInChildren<Camera>();
        agencik.Warp(bossIdleWaypoint.transform.position);
        StartCoroutine(PlayStepSounds());
    }

    void Update()
    {
        playersCollectibles = eqSystem.GetComponent<EqSystem>().GetImportantPoints();
        pandaAnimator.SetInteger("Collectibles", playersCollectibles);
        if (currentPhase != BossPhases.Distracted)
        {
            currentPhase = ChangePhase();
            SelectSoundToPlay();
        }
        pandaAnimator.SetBool("Asleep", currentPhase == BossPhases.Idle);
        if (startedMoving == false)
            SetMovement();
        speed = Vector3.Project(agencik.desiredVelocity, transform.forward).magnitude;
        pandaAnimator.SetFloat("Speed", speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bambus"))
        {
            currentPhase = BossPhases.Distracted;
            StartCoroutine(TriggerDistraction());
        }
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            var playerStats = other.gameObject.GetComponentInChildren<EnergyBar>();
            if (playerStats == null) { Debug.Log("I am null"); return; }
            while (playerStats.GetHealthLevel() > 0)
                playerStats.DecreaseHealth();
            GameObject.FindGameObjectWithTag("FlashingScreen").GetComponent<FlashingScript>().SetRedScreen();
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
