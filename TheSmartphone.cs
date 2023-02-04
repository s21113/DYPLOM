using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Do zgłaszania wyjątków rzucanych w smartfonie
/// </summary>
public class SmartphoneException : Exception
{
    public SmartphoneException() : base("Coś w tym cudownym smartfonie się spierdoliło i teraz nie działa") { }
    public SmartphoneException(string message) : base("Coś w tym cudownym smartfonie się zepsuło: \n\t" + message) { }
}

public enum CurrentMenu
{
    None,
    Main,
    Map,
    Inventory,
    Message
}

// -------------------------------- //

public class TheSmartphone : MonoBehaviour
{
    private static readonly bool DEBUG = true; // debugging lol

    public CurrentMenu currentMenu = CurrentMenu.None;
    public bool isActive = false; // Czy smartfon jest aktywny
    //private bool inMainMenu = true;
    //private bool isInMenu = false; // Czy jesteśmy w jakimś menu
    private bool isViewingMessage = false; // Czy wyświetlamy jakąś wiadomość

    private RectTransform position;
    private SmartphoneOptions options = new SmartphoneOptions(); // Lista opcji
    private SmartphoneOptions messages = new SmartphoneOptions(); // Coś jak smartphone options ale z wiadomościami
    private SmartphoneOptions inventory = new SmartphoneOptions(); // Coś jak smartphone options ale z itemkami
    private List<GameObject> optionObjs = new List<GameObject>(); // obiekty z opcjami
    private List<GameObject> itemObjs = new List<GameObject>(); // obiekty z przedmiotami z ekwipunku
    private List<GameObject> messageObjs = new List<GameObject>(); // obiekty z wiadomościami

    public GameObject optionsScene, menusScene; // analogicznie dla menu
    private GameObject mapContainer, invContainer, msgContainer; // kontenery z itemami menu
    private GameObject currentMessageObj;
    public GameObject togglePrefab; // prefab toggle'a
    public Text energyLevelText; // tekst zawierający ile energii latarka ma

    public GameObject _eqSystem; // przypisać w inspektorze
    private EqSystem eqSystem; // pobrać w awake

    public static TheSmartphone instance;



    #region UPDATES
    /// <summary>
    /// Pobiera z ekwipunku ile mamy punktów i wyświetla w opcji
    /// </summary>
    private void GetAndUpdateCollectibles()
    {
        //elo
        // muszę inaczej zrobić pobieranie znajdziek
        var collectibles = eqSystem.GetImportantPoints();
        var option = options.FindByNamePart("Points");
        var optionIndex = options.FindIndexOfOption(option);
        option.name = Regex.Replace(option.name, "([0-9]{1,})", collectibles + "");
        optionObjs[optionIndex].transform.GetChild(0).gameObject.GetComponent<Text>().text = option.name;
    }

    /// <summary>
    /// Pobiera z ekwipunku poziom baterii gracza
    /// </summary>
    private void UpdateBatteryLevel()
    {
        var playerStats = eqSystem.transform.parent.GetComponentInChildren<EnergyBar>();
        Dictionary<float, string> energyTexts = new Dictionary<float, string>
        {
            {1.00f, "█ █ █ █" },
            {0.95f, "█ █ █ █" },
            {0.90f, "█ █ █ █" },
            {0.85f, "█ █ █ █" },
            {0.80f, "█ █ █ █" },
            {0.75f, "█ █ █" },
            {0.70f, "█ █ █" },
            {0.65f, "█ █ █" },
            {0.60f, "█ █ █" },
            {0.55f, "█ █ █" },
            {0.50f, "█ █" },
            {0.45f, "█ █" },
            {0.40f, "█ █" },
            {0.35f, "█ █" },
            {0.30f, "█ █" },
            {0.25f, "█" },
            {0.20f, "█" },
            {0.15f, "█" },
            {0.10f, "█" },
            {0.05f, "█" },
            {0.01f, " " }
        };
        string _ = "";
        float energy = (float)(playerStats.GetEnergyLevel() / playerStats.energyMax);
        energyTexts.TryGetValue(energy, out _);
        //Debug.Log(playerStats.GetEnergyLevel() + " energy is at " + energy*100 + "%, text received: " + _);
        if (_ != null)
            energyLevelText.text = _;
    }

    private void UpdateReceivedMessages()
    {
        var newJournals = eqSystem.GetCollectedJournals();
        int c = newJournals.Count;
        if (c <= messageObjs.Count || c <= messages.GetSize()) return;
        for (int i = messageObjs.Count; i < c; i++)
        {
            var msg = newJournals[i];
            SmartphoneOption msgAsOption = new SmartphoneOption(msg.name);
            msgAsOption.isMenu = true;
            messageObjs.Add(CreateMessageObject(msgContainer, msg, i, msgAsOption));

        }

    }

    private void UpdateInventory()
    {
        var newItems = eqSystem.GetCollectedItems();
        int c = newItems.Count;
        if (c <= itemObjs.Count || c <= inventory.GetSize()) return;
        for (int i = itemObjs.Count; i < c; i++)
        {
            var it = newItems[i];
            SmartphoneOption itAsOption = new SmartphoneOption(it.name);
            itemObjs.Add(CreateItemObject(invContainer, it, i, itAsOption));
        }
    }
    #endregion

    /// <summary>
    /// Generycznie tworzy opcję GUI na podstawie opcji (UŻYWAĆ DLA MENU GŁÓWNEGO)
    /// </summary>
    private GameObject SceneObjectCreation(SmartphoneOption opt)
    {
        // Stwórz mi gameObject i ustaw rozmiar
        var optionGameObject = Instantiate(togglePrefab);
        optionGameObject.name = $"{opt.name} Toggle";
        optionGameObject.GetComponentInChildren<Text>().text = opt.name;
        // ustaw toggla
        var toggleC = optionGameObject.GetComponent<Toggle>();
        toggleC.transition = Selectable.Transition.None;
        toggleC.isOn = false;
        var imageC = optionGameObject.GetComponent<Image>();
        imageC.color = SelectionColors.UNSELECTED_DISABLED;
        if (!opt.isMenu) toggleC.onValueChanged.AddListener(delegate
        {
            opt.Execute();
        });
        opt.toggle = toggleC;
        var lolC = optionGameObject.GetComponent<SmartphoneOptionGUI>();
        lolC.Set(opt);
        // ustaw rodzicielstwo
        optionGameObject.transform.SetParent(optionsScene.transform);
        return optionGameObject;
    }

    #region MAP
    /// <summary>
    /// nuff said, tworzenie menu mapy w scenie bo tego się genericsami nie da
    /// </summary>
    private GameObject CreateMapMenuInScene()
    {
        // TODO zrobienie faktycznej mapy
        mapContainer = new GameObject("Map");
        GameObject tmp = new GameObject("In progress");
        Text text = mapContainer.AddComponent<Text>();
        text.text = "Map\nis\nWIP";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 20;
        mapContainer.transform.SetParent(menusScene.transform);
        tmp.transform.SetParent(mapContainer.transform);
        mapContainer.SetActive(false);
        return mapContainer;
    }
    #endregion

    #region INVENTORY
    private GameObject CreateItemObject(GameObject invContainer, Item item, int i = -1, SmartphoneOption opt = null)
    {
        GameObject itemToggleObj = Instantiate(togglePrefab);
        itemToggleObj.name = $"{opt.name} Item";
        itemToggleObj.GetComponentInChildren<Text>().text = opt.name;
        // ustaw toggla
        var toggleC = itemToggleObj.GetComponent<Toggle>();
        toggleC.transition = Selectable.Transition.None;
        toggleC.isOn = false;
        var imageC = itemToggleObj.GetComponent<Image>();
        imageC.color = SelectionColors.UNSELECTED_DISABLED;
        if (opt != null)
        {
            toggleC.onValueChanged.AddListener(delegate
            {
                opt.Execute();
            });
            opt.toggle = toggleC;
            var lolC = itemToggleObj.GetComponent<SmartphoneOptionGUI>();
            lolC.Set(opt);
            inventory.AddOption(opt);
        }
        // ustaw rodzicielstwo
        itemToggleObj.transform.SetParent(invContainer.transform);
        itemToggleObj.SetActive(true);
        return itemToggleObj;
    }

    private GameObject CreateInventoryMenuInScene()
    {
        invContainer = new GameObject("Inventory");
        var its = eqSystem.GetComponent<EqSystem>().GetCollectedItems();
        List<SmartphoneOption> test = new List<SmartphoneOption>();
        foreach (var it in its)
        {
            SmartphoneOption itAsOption = new SmartphoneOption(it.name);
            itemObjs.Add(CreateItemObject(invContainer, it, its.IndexOf(it), itAsOption));
            test.Add(itAsOption);
        }
        inventory = new SmartphoneOptions(test.ToArray());
        invContainer.transform.SetParent(menusScene.transform);

        var vertlg = invContainer.AddComponent<VerticalLayoutGroup>();
        vertlg.spacing = 2;
        vertlg.childAlignment = TextAnchor.UpperCenter;
        vertlg.childControlWidth = false; vertlg.childControlHeight = false;
        vertlg.childForceExpandWidth = false; vertlg.childForceExpandHeight = false;
        vertlg.childScaleWidth = false; vertlg.childScaleHeight = false;

        // TODO kwadratowy layout

        invContainer.SetActive(false);
        return invContainer;
    }
    #endregion

    #region MESSAGES
    /// <summary>
    /// wyciągnąłem całe to tworzenie wiadomości do metody
    /// bo co jeżeli nagle dostaniemy jakąś wiadomość?
    /// </summary>
    /// <param name="msgContainer">kontener ze wszystkimi wiadomościami i całym menu</param>
    /// <param name="msg">wiadomość do wepchnięcia</param>
    /// <param name="i">która to już</param>
    /// <returns>GameObject zawierający wszystko co potrzeba, gotowy do akcji</returns>
    private GameObject CreateMessageObject(GameObject msgContainer, Journal msg, int i = -1, SmartphoneOption opt = null)
    {
        GameObject msgToggleObj = Instantiate(togglePrefab);
        msgToggleObj.name = "Message " + i;
        string tmpname = msg.contents.Substring(0, 13) + "...";
        msgToggleObj.GetComponentInChildren<Text>().text = tmpname;

        GameObject msgObj = new GameObject("Contents of Message " + i);
        Text msgContent = msgObj.AddComponent<Text>();
        msgContent.horizontalOverflow = HorizontalWrapMode.Wrap;
        msgContent.text = msg.contents;

        // ustaw toggla
        var toggleC = msgToggleObj.GetComponent<Toggle>();
        toggleC.transition = Selectable.Transition.None;
        toggleC.isOn = false;
        var imageC = msgToggleObj.GetComponent<Image>();
        imageC.color = SelectionColors.UNSELECTED_DISABLED;
        if (opt != null)
        {
            opt.toggle = toggleC;
            var lolC = msgToggleObj.GetComponent<SmartphoneOptionGUI>();
            lolC.Set(opt);
            messages.AddOption(opt);
        }
        // ustaw rodzicielstwo
        msgObj.transform.SetParent(msgToggleObj.transform);
        msgToggleObj.transform.SetParent(msgContainer.transform);

        msgToggleObj.SetActive(true);
        return msgToggleObj;
    }

    /// <summary>
    /// nuff said, tworzenie menu wiadomości w scenie bo tego się genericsami nie da
    /// </summary>
    private GameObject CreateMessagesMenuInScene()
    {
        msgContainer = new GameObject("Messages");
        var msgs = eqSystem.GetComponent<EqSystem>().GetCollectedJournals();
        int i = 0;
        List<SmartphoneOption> test = new List<SmartphoneOption>();
        foreach (var msg in msgs)
        {
            i++;
            SmartphoneOption msgAsOption = new SmartphoneOption(msg.name);
            msgAsOption.isMenu = true;
            messageObjs.Add(CreateMessageObject(msgContainer, msg, i, msgAsOption));
            test.Add(msgAsOption);
        }
        messages = new SmartphoneOptions(test.ToArray());
        msgContainer.transform.SetParent(menusScene.transform);

        var vertlg = msgContainer.AddComponent<VerticalLayoutGroup>();
        vertlg.spacing = 2;
        vertlg.childAlignment = TextAnchor.UpperCenter;
        vertlg.childControlWidth = false; vertlg.childControlHeight = false;
        vertlg.childForceExpandWidth = false; vertlg.childForceExpandHeight = false;
        vertlg.childScaleWidth = false; vertlg.childScaleHeight = false;

        msgContainer.SetActive(false);

        return msgContainer;
    }
    #endregion



    #region MENU_NAVIGATION

    private float _moveWrt = 350f;
    /// <summary>
    /// Przywołaj smartfona do góry, pod warunkiem że jest nieaktywny
    /// </summary>
    public void BringUp()
    {
        if (!isActive) isActive = true;
        position.Translate(new Vector3(0, _moveWrt, 0));
    }

    /// <summary>
    /// Schowaj smartfona, pod warunkiem że jest aktywny
    /// </summary>
    public void Hide()
    {
        if (isActive) isActive = false;
        position.Translate(new Vector3(0, -_moveWrt, 0));
    }
    private void Deselect(SmartphoneOptions opts, List<GameObject> objs, int index = 0)
    {
        if (index < 0 || index > opts.GetSize()) return;
		//opts.Select(index);
        opts.GetOption(index).SetSelection(false);
        objs[index].GetComponent<SmartphoneOptionGUI>().Deselect();
    }
    private void Select(SmartphoneOptions opts, List<GameObject> objs, int index = 0)
    {
        if (index < 0 || index > opts.GetSize()) return;
        if (!opts.GetOption(index).isSelectable) Select(opts, objs, ++index);
		//opts.Deselect(index);
        opts.GetOption(index).SetSelection(true);
        objs[index].GetComponent<SmartphoneOptionGUI>().Select();
    }
    private void Toggle(SmartphoneOptions opts, List<GameObject> objs)
    {
        int i = opts.GetCurrentlySelectedIndex();
        if (opts.GetOption(i).isMenu)
        {
            GoToMenu(opts, objs);
            return;
        }
        opts.GetOption(i).Execute();
        objs[i].GetComponent<SmartphoneOptionGUI>().Execute();
    }

    private void SetAllToggles(bool enable, GameObject container, bool backFromMenu)
    {
        container.SetActive(true);
        if (!backFromMenu) return;
        for (int i = 0; i < container.transform.childCount; i++)
        {
            var msg = container.transform.GetChild(i).gameObject;
            msg.SetActive(enable);
        }
    }

    private void GoToMenu(SmartphoneOptions opts, List<GameObject> objs, bool backFromMenu = false)
    {
        int index = opts.GetCurrentlySelectedIndex();
        var destination = opts.GetOption(index);
        if (destination == null) return;
        if (!destination.isMenu) return;
        optionsScene.SetActive(false);
        menusScene.SetActive(true);
        if (objs[index].name.Contains("Messages")) SetAllToggles(true, msgContainer, backFromMenu);
        else if (objs[index].name.Contains("Map")) SetAllToggles(true, mapContainer, backFromMenu);
        else if (objs[index].name.Contains("Inventory")) SetAllToggles(true, invContainer, backFromMenu);
        else throw new SmartphoneException($"optionObjs na pozycji {index} ma nazwę {objs[index].name} nie odpowiadającą {destination.name}... który idiota tu dowodzi?");
    }
    private void BackToMain(SmartphoneOptions opts, List<GameObject> objs)
    {
        optionsScene.SetActive(true);
        menusScene.SetActive(false);
        msgContainer.SetActive(false);
        mapContainer.SetActive(false);
        invContainer.SetActive(false);
        Select(opts, objs, opts.GetCurrentlySelectedIndex());
    }

    private void ViewMessage(SmartphoneOption current, int index)
    {
        if (current == null) return;
        for (int i = 0; i < msgContainer.transform.childCount; i++)
        {
            var msg = msgContainer.transform.GetChild(i).gameObject;
            msg.SetActive(false);
        }
        var toggle = msgContainer.transform.GetChild(index);
        GameObject contents = null;
        for (int i = 0; i < toggle.transform.childCount; i++)
        {
            var child = toggle.transform.GetChild(i);
            if (child.name.StartsWith("Contents"))
            {
                contents = child.gameObject;
                break;
            }
        }
        if (contents == null) throw new SmartphoneException("Nic się nie zmieniło... contents nadal są nullem");
        if (currentMessageObj == null)
        {
            currentMessageObj = new GameObject("Current Message Display");
            currentMessageObj.transform.SetParent(msgContainer.transform);
            var newTxt = currentMessageObj.AddComponent<Text>();
        }
        var txt = currentMessageObj.GetComponent<Text>();
        txt.text = contents.GetComponent<Text>().text;
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.fontSize = 20;
        txt.horizontalOverflow = HorizontalWrapMode.Wrap;
        var rect = currentMessageObj.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 400);
        currentMessageObj.SetActive(true);
    }

    #endregion



    #region INPUT_HANDLING
    private void HandleMainMenuInputs()
    {
        if (!isActive && !Input.GetKeyDown(NavigationKeys.menuUp)) return;
        else if (!isActive && Input.GetKeyDown(NavigationKeys.menuUp))
        {
            isActive = true;
            currentMenu = CurrentMenu.Main;
            BringUp();
            Select(options, optionObjs);
        }
        if (currentMenu != CurrentMenu.Main) return;
        var current = options.GetCurrentlySelected();
        int currI = options.GetCurrentlySelectedIndex();
        if (isViewingMessage) return;
        if (Input.GetKeyDown(NavigationKeys.menuBack))
        {
            if (DEBUG) Debug.Log("Main menu received left arrow.");
            Hide();
            Deselect(options, optionObjs, currI);
            currentMenu = CurrentMenu.None;
        }
        if (Input.GetKeyDown(NavigationKeys.menuDown))
        {
            if (options.GetSize() <= 1) return;
            if (currI + 1 > options.GetSize() - 1) return;
            int nI = currI + 1;
            Deselect(options, optionObjs, currI);
            Select(options, optionObjs, nI);
        }
        else if (Input.GetKeyDown(NavigationKeys.menuUp))
        {
            if (currI - 1 < 0) return;
            int nI = currI - 1;
            Deselect(options, optionObjs, currI);
            Select(options, optionObjs, nI);
        }
        if (Input.GetKeyDown(NavigationKeys.menuAdvance))
        {
            if (current.isMenu)
            {
                if (DEBUG) Debug.Log("Main Menu: Going to a different menu!");
                GoToMenu(options, optionObjs);
                if (current.name.Equals("Messages"))
                {
                    if (messageObjs.Count > 0) Select(messages, messageObjs);
                    currentMenu = CurrentMenu.Message;
                }
                else if (current.name.Equals("Inventory"))
                {
                    if (itemObjs.Count > 0) Select(inventory, itemObjs);
                    currentMenu = CurrentMenu.Inventory;
                }
                else if (current.name.Equals("Map"))
                {
                    currentMenu = CurrentMenu.Map;
                }
            }
            else Toggle(options, optionObjs);
        }
    }

    /// <summary>
    /// Obsługiwanie inputów jeżeli jesteśmy w menu wiadomości
    /// </summary>
    private void HandleMessageMenuInputs()
    {
        if (!isActive) return;
        if (currentMenu != CurrentMenu.Message) return;
        var current = messages.GetCurrentlySelected();
        int currI = messages.GetCurrentlySelectedIndex();
        if (Input.GetKeyDown(NavigationKeys.menuBack))
        {
            if (DEBUG) Debug.Log("Message menu received left arrow.");
            if (isViewingMessage)
            {
                if (DEBUG) Debug.Log("Going back to Messages menu.");
                Destroy(currentMessageObj);
                GoToMenu(options, optionObjs, true);
                isViewingMessage = false;
            }
            else
            {
                if (DEBUG) Debug.Log("Going back to Main menu.");
                BackToMain(options, optionObjs);
                currentMenu = CurrentMenu.Main;
            }
        }
        if (eqSystem.GetCollectedJournals().Count == 0) return;

        if (Input.GetKeyDown(NavigationKeys.menuAdvance) && !isViewingMessage)
        {
            if (current == null) return;
            if (DEBUG) Debug.Log("Message menu received right arrow.");
            if (current.isMenu)
            {
                if (DEBUG) Debug.Log("Going to a message!");
                ViewMessage(current, currI);
                isViewingMessage = true;
            }
            else return;
        }
        if (eqSystem.GetCollectedJournals().Count == 1) return;

        if (Input.GetKeyDown(NavigationKeys.menuDown) && !isViewingMessage)
        {
            if (DEBUG) Debug.Log("Message menu received down arrow.");
            if (messages.GetSize() <= 1) return;
            if (currI + 1 > messages.GetSize() - 1) return;
            int nI = currI + 1;
            Deselect(messages, messageObjs, currI);
            Select(messages, messageObjs, nI);
        }
        else if (Input.GetKeyDown(NavigationKeys.menuUp) && !isViewingMessage)
        {
            if (DEBUG) Debug.Log("Message menu received up arrow.");
            if (currI - 1 < 0) return;
            int nI = currI - 1;
            Deselect(messages, messageObjs, currI);
            Select(messages, messageObjs, nI);
        }
    }

    /// <summary>
    /// Obsługiwanie inputów jeśli jesteśmy w menu mapy
    /// </summary>
    private void HandleMapMenuInputs()
    {
        if (!isActive) return;
        if (currentMenu != CurrentMenu.Map) return;
        if (Input.GetKeyDown(NavigationKeys.menuBack))
        {
            if (DEBUG) Debug.Log("Map menu received left arrow.");
            if (DEBUG) Debug.Log("Going back to Main menu.");
            BackToMain(options, optionObjs);
            currentMenu = CurrentMenu.Main;
        }
        // i to tyle, bo więcej mapa nie ma
    }

    /// <summary>
    /// Obsługiwanie inputów jeśli jesteśmy w ekwipunku
    /// </summary>
    private void HandleInventoryMenuInputs()
    {
        if (!isActive) return;
        if (currentMenu != CurrentMenu.Inventory) return;
        // jeśli nie jest aktywny lub enum nie jest ustawiony, to po co siedzieć w tej metodzie xd

        var current = inventory.GetCurrentlySelected();
        int currI = inventory.GetCurrentlySelectedIndex();
        if (Input.GetKeyDown(NavigationKeys.menuBack))
        {
            if (DEBUG) Debug.Log("Inventory menu received left arrow.");
            if (DEBUG) Debug.Log("Going back to Main menu.");
            BackToMain(options, optionObjs);
            currentMenu = CurrentMenu.Main;
        }
        if (eqSystem.GetCollectedItems().Count == 0) return;
        // jeśli nie mamy żadnych przedmiotów, to pewnie będzie się sypała nawigacja - dlatego wyjdźmy z metody

        if (Input.GetKeyDown(NavigationKeys.menuAdvance))
        {
            if (DEBUG) Debug.Log("Inventory menu received right arrow.");
            if (current == null) return;
            if (current.isMenu)
            {
                if (DEBUG) Debug.Log("Using an item!");
                Toggle(inventory, itemObjs);
            }
            else return;
        }
        if (eqSystem.GetCollectedItems().Count == 1) return;
        // jeśli mamy tylko jeden przedmiot, to pewnie będzie się sypała nawigacja - dlatego wyjdźmy z metody

        if (Input.GetKeyDown(NavigationKeys.menuDown))
        {
            if (DEBUG) Debug.Log("Inventory menu received down arrow.");
            if (messages.GetSize() <= 1) return;
            if (currI + 1 > messages.GetSize() - 1) return;
            int nI = currI + 1;
            Deselect(inventory, itemObjs, currI);
            Select(inventory, itemObjs, nI);
        }
        else if (Input.GetKeyDown(NavigationKeys.menuUp))
        {
            if (DEBUG) Debug.Log("Inventory menu received up arrow.");
            if (currI - 1 < 0) return;
            int nI = currI - 1;
            Deselect(inventory, itemObjs, currI);
            Select(inventory, itemObjs, nI);
        }
    }

    #endregion



    // metody unity
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        eqSystem = _eqSystem.GetComponent<EqSystem>();
        optionObjs = new List<GameObject>();
        itemObjs = new List<GameObject>();
        messageObjs = new List<GameObject>();

        // tutaj zdefiniujmy opcje do zaladowania xd
        SmartphoneOption flopt = new SmartphoneOption("Flashlight", () => Flashlight.instance.Toggle());
        SmartphoneMenu mamen = new SmartphoneMenu("Map");
        SmartphoneMenu inmen = new SmartphoneMenu("Inventory");
        SmartphoneMenu memen = new SmartphoneMenu("Messages");
        SmartphoneOption poopt = new SmartphoneOption("Points = 0");
        poopt.isSelectable = false;

        options = new SmartphoneOptions(poopt, flopt, inmen, mamen, memen);
    }

    void Start()
    {
        position = GetComponent<RectTransform>();
        for (int i = 0; i < options.GetSize(); i++)
        {
            var opt = options.GetOption(i);
            optionObjs.Add(SceneObjectCreation(opt));
        }
        CreateMapMenuInScene();
        CreateInventoryMenuInScene();
        CreateMessagesMenuInScene();
    }

    void Update()
    {
        if (PauseHandler.instance.paused == true) return;
        GetAndUpdateCollectibles();
        UpdateBatteryLevel();
        UpdateReceivedMessages();
        UpdateInventory();
        if (currentMenu == CurrentMenu.None || currentMenu == CurrentMenu.Main)
            Invoke("HandleMainMenuInputs", 0.000001f);
        else if (currentMenu == CurrentMenu.Message)
            Invoke("HandleMessageMenuInputs", 0.0001f);
        else if (currentMenu == CurrentMenu.Map)
            Invoke("HandleMapMenuInputs", 0.0001f);
        else if (currentMenu == CurrentMenu.Inventory)
            Invoke("HandleInventoryMenuInputs", 0.0001f);
    }
}