using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPickupScript : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider coll;
    private GameObject eqSystem;
    public Transform player;

    public GameObject minigameInterface;
	public bool canPlayMinigame = false;
    private bool isPlaying = false;
    private bool receivedInteractionInput => Input.GetKeyUp(KeyCode.E);
    private bool receivedFinishInput => Input.GetKeyUp(KeyCode.Return);
    private bool receivedExitInput => Input.GetKeyUp(KeyCode.Escape);

    public Item item;

    //private Renderer renderer;

    public bool playerInSightRange;
    public LayerMask whatIsPlayer;

    private void Start()
    {
        // zmienię to wszystko jutro jak nie bedzie napięcia
        player = GameObject.Find("Player").transform;
        eqSystem = GameObject.FindGameObjectWithTag("Inventory");
    }

    internal void StartMinigame(MiniGameInterface game, bool firstTime = true)
    {
        isPlaying = true;
        player.gameObject.GetComponent<BetterPlayerMovement>().TryMinigame();
        if (firstTime) game.LoadMinigame();
        if (TheSmartphone.instance.isActive)
            TheSmartphone.instance.Hide();
        game.BringUp();
    }

    internal void HandleMinigame(MiniGameInterface game)
    {
        if (receivedFinishInput)
        {
            Debug.Log("Received finish input");
            game.Solve();
            if (game.isSolved)
            {
                isPlaying = false;
                player.gameObject.GetComponent<BetterPlayerMovement>().ExitMinigame();
                eqSystem.GetComponent<EqSystem>().PickupItem(item);
                PauseHandler.instance.inSomeMenu = false;
                Destroy(gameObject);
                return;
            }
            else
            {
                StartMinigame(game, false);
            }
        }
        else if (receivedExitInput)
        {
            Debug.Log("Received exit input");
            game.Escape();
            isPlaying = false;
            player.gameObject.GetComponent<BetterPlayerMovement>().ExitMinigame();
            PauseHandler.instance.inSomeMenu = false;
            return;
        }
    }


    void Update()
    {
        if (minigameInterface == null)
        {
            Debug.LogError("Coś dziwnego się stało, zwykle tutaj nie wchodzimy.");
            Destroy(gameObject);
            return;
        }
        float distance = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2) + Mathf.Pow(player.position.z - transform.position.z, 2));
        canPlayMinigame = distance < 3;

        var game = minigameInterface.GetComponent<MiniGameInterface>();
        if (canPlayMinigame && !isPlaying)
        {
            if (receivedInteractionInput)
                StartMinigame(game);
        }
        if (isPlaying)
        {
            HandleMinigame(game);
        }
    }
}
