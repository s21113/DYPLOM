using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BetterPlayerMovement : MonoBehaviour
{
    [Header("Player Data")]
    public GameObject _inventory;
    private EqSystem inventory;
    private EnergyBar playerStats;

    GameObject flashingScreen;

    [Header("Settings")]
    [SerializeField] private Settings settings;
    //GameObject staminaBar;
    //GameObject healthBar;
    bool CrRunning = false;
    public bool CanMove { get; private set; } = true;
    private bool isSprinting => (settings.sprintToggle ? Input.GetKeyDown(sprintKey) : Input.GetKey(sprintKey)) && !isCrouching; 
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch = false;//> (settings.crouchToggle ? Input.GetKeyDown(crouchKey) : Input.GetKey(crouchKey)) && !duringCrouchingAnimation && characterController.isGrounded;

    [Header("Functional Options")]
    //private bool running = false;
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool canPlayMinigame = false;
    [SerializeField] private bool isPlaying = false;
    [SerializeField] private MouseLook mouseLook;


    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;


    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed=5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;


    [Header("Look Parameters")]
    //[SerializeField, Range(1, 10)] private float lookSpeedX = 1.2f;
    //[SerializeField, Range(1, 10)] private float lookSpeedY = 1.2f;
    [SerializeField, Range(1, 180)] private float lookUpLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lookDownLimit = 80.0f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14.0f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18.0f;
    [SerializeField] private float sprintBobAmount = 0.09f;
    [SerializeField] private float crouchBobSpeed = 8.0f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;


    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight=0.5f;
    [SerializeField] private float standHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standCenter = new Vector3(0, 0, 0);
    public bool isCrouching;
    private bool duringCrouchingAnimation;
    public bool isLeaving = false;
    public Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float xRotation=0;
    private Transform spawnPointer;


    // debug rzeczy
    private static string[] troll2 = { "a", "w", "r", "u", "k" };
    private static int troll1 = 4;

    private void Start()
    {
        playerStats = GetComponent<EnergyBar>();
    }
    // Start is called before the first frame update
    void Awake()
    {
        spawnPointer = GameObject.FindGameObjectWithTag("Respawn").transform;
        settings = GameSettings.ReadSettingsFromFile();
        inventory = _inventory.GetComponent<EqSystem>();
        //playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        CameraFocus();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current height UPDATE: " + characterController.height + " ,current CEnter UPDATE: "+ characterController.center);
        GameSettings.UpdateSettings(out this.settings);
        CameraFocus();
        CheckIfPaused();
        if (isPlaying)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ExitMinigame();
            }
        }
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (canUseHeadbob)
                HandleHeadbob();

            if (canJump)
                HandleJump();

            if (canCrouch)
                HandleCrouch();

            ApplyFinalMovement();
            if (!isPlaying) TryMinigame();
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(troll2[troll1]))
                    --troll1;
                else
                    troll1 = 4;
            }
            if (troll1 == -1)
            {
                troll1 = 4;
                inventory.func_f742358363();
                Debug.Log("Pong!");
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                ExitMinigame();
            }
        }
    }


    private void HandleMovementInput()
    {
        currentInput = new Vector2((isSprinting && canSprint ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isSprinting && canSprint ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        //odpalam utratę stminy z blokadą sprintu jeżeli spadnie za nisko
        if (playerStats.GetStaminaLevel() <= 0)
        {
            canSprint = false;
        }
        else if (isSprinting)
        {
            canSprint = true;
            if (Math.Abs(currentInput.x) >= 0.07f || Math.Abs(currentInput.y) >= 0.07f)
            {
                playerStats.updateStaminaBar();
            }
        }
        //odpalam regenracje staminy
        if (Input.GetKeyUp(sprintKey))
        {
            if (!CrRunning)
            {
                StartCoroutine("regenerateStamina");
            }
        }
        else if (Input.GetKey(sprintKey))
        {
            if (!CrRunning)
            {
                StopCoroutine("regenerateStamina");
            }
        }
        


        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        playerCamera.fieldOfView = settings.fov;
        float yAxis = Input.GetAxis("Mouse Y") * settings.mouseSens;
        if (settings.invertY) yAxis *= -1;
        xRotation -= yAxis;
        xRotation = Mathf.Clamp(xRotation, -lookUpLimit, lookDownLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * settings.mouseSens, 0);

    }
    
    private void HandleJump()
    {
        if (shouldJump)
            moveDirection.y = jumpForce;
    }

    private void HandleCrouch()
    {
        if (shouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if(Mathf.Abs(moveDirection.x)>0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting && canSprint ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos+Mathf.Sin(timer)*(isCrouching?crouchBobAmount:isSprinting && canSprint ? sprintBobAmount:walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private IEnumerator CrouchStand()
    {

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;
        Debug.Log("Stand Height: "+standHeight + " , crouch height: " + crouchHeight);

        duringCrouchingAnimation = true;
        float timeElapsed = 0;
        float targetHeight = isCrouching ? standHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standCenter : crouchCenter;
        Vector3 currentCenter = characterController.center;
        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;
        Debug.Log("Current Height: " + currentHeight );
        Debug.Log("Target height: " + targetHeight + " , terget center: " + targetCenter);
        isCrouching = !isCrouching;

        duringCrouchingAnimation = false;
    }

    private void ApplyFinalMovement()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);

        if (transform.position.y <= -100f)
        {
            transform.position = spawnPointer.position;
            inventory.ReceiveMessage(inventory.FindJournal("Out of bounds"));
        }
    }

    private void CameraFocus()
    {
        if (!Cursor.visible) return;
        if (CanMove)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void TryMinigame()
    {
        if (!canPlayMinigame) return;
        PauseHandler.instance.inSomeMenu = true;
        isPlaying = true;
        StartCoroutine(PlayMinigame());
    }

    private IEnumerator PlayMinigame()
    {
        if (!canPlayMinigame) yield break;
    }

    public void ExitMinigame()
    {
        if (!isPlaying) return;
        StopCoroutine(PlayMinigame());
        PauseHandler.instance.inSomeMenu = false;
        isPlaying = false;
    }

    private void CheckIfPaused()
    {
        //Debug.Log(CanMove + " |=| " + PauseHandler.instance.paused + " |=| " + PauseHandler.instance.inSomeMenu);
        CanMove = !( PauseHandler.instance.paused || PauseHandler.instance.inSomeMenu );
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("ZnajdzkaPlusGra")) return;
        var pickupScr = other.gameObject.GetComponent<SpecialPickupScript>();
        if (pickupScr == null) return;
        pickupScr.canPlayMinigame = canPlayMinigame;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("ZnajdzkaPlusGra")) return;
        var pickupScr = other.gameObject.GetComponent<SpecialPickupScript>();
        if (pickupScr == null) return;
        canPlayMinigame = pickupScr.canPlayMinigame;
    }

    public IEnumerator regenerateStamina()
    {
        bool run = false;
        yield return new WaitForSeconds(1f);
        do {
            run = playerStats.regenerateStaminaBar(2);
            yield return new WaitForSeconds(0.03f);
        } while (run);
        playerStats.setConcreteStamina();
        StopCoroutine("regenerateStamina");
    }


    private void OnTriggerEnter(Collider collider)
    {
        flashingScreen = GameObject.Find("FlashingScreen");

        if (collider.gameObject.tag == "Enemy")
        {
            flashingScreen.GetComponent<FlashingScript>().SetRedScreen();
        }
        /*if (collider.gameObject.CompareTag("BOSS"))
        {
            if (inventory.GetImportantPoints() < 1) return;
            while (playerStats.GetHealthLevel() > 0)
                playerStats.DecreaseHealth();
            GameObject.FindGameObjectWithTag("FlashingScreen").GetComponent<FlashingScript>().SetRedScreen();
        }*/

        /*if (collider.gameObject.tag == "PowerBank")
        {
            if (GetComponentInChildren<Flashlight>().CheckChargeLevel() < 500)
            {
                GetComponentInChildren<Flashlight>().ChargeUp(100);
                Destroy(collider.gameObject);
            }
        }

        if (collider.gameObject.tag == "Health")
        {
            if (playerStats.GetHealthLevel() < 4)
            {
                playerStats.updateHealthBarUP();
                Destroy(collider.gameObject);
            }
        }*/
    }
}
