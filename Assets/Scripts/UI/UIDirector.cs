using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDirector : MonoBehaviour
{
    public GameObject BaseCanvas;
    public GameObject LifeBarsContainer;
    public GameObject friendlyPanel;
    public GameObject enemyPanel;
    public GameObject prefabIcon;
    public GameObject prefabPlayerPanel;
    public GameObject prefabInventoryItem;
    public GameObject prefabHealthBar;
    public InventoryNotify prefabInventoryNotify;

    public Texture2D CursorImage;
    public Texture2D TargetCursorImage;
    public Texture2D AttackCursorImage;

    public static UIDirector instance;
    
    private CreatureStats[] _creatures;
    private List<PlayerPanelScript> _playerPanels = new List<PlayerPanelScript>();
    private Dictionary<IInventoryItem, GameObject> _inventoryIcons = new Dictionary<IInventoryItem, GameObject>();

    public InventoryNotify Notify { get; private set; }

    void Start()
    {
        instance = this;
        _creatures = FindObjectsOfType<CreatureStats>();
        Cursor.SetCursor(CursorImage, new Vector2(25, 25), CursorMode.ForceSoftware);

        //Генерим иконки персонажей
        GenerateCreturesIcons();
        //Генерим панельки активных игроков
        GeneratePlayersPanels();
        //Генерим HealthBar
        GenerateHealthBars();
        

        //Проверяем текущего игрока
        MatchSystem.instance.OnChangePlayer += MatchSystem_OnChangePlayer;

        //Проверяем инвентарь
        InventorySystem.instance.OnPickInventoryItem += InventorySystem_OnPickInventoryItem;
        InventorySystem.instance.OnRemoveInventoryItem += InventorySystem_OnRemoveInventoryItem;

        //Определяем наведение
        ClickObject.OnHoverIn += ClickObject_OnHoverIn;
        ClickObject.OnHoverOut += ClickObject_OnHoverOut;
    }

    public void RefreshUIGameData()
    {
        _creatures = FindObjectsOfType<CreatureStats>();
        Debug.Log("Refresh UI");
        //удаляем аватарки
        foreach(AvatarScript a in FindObjectsOfType<AvatarScript>())
        {
            Destroy(a.gameObject);
        }
        //удаляем HealthBar
        /*foreach (UIHealthBarScript a in FindObjectsOfType<UIHealthBarScript>())
        {
            Destroy(a.gameObject);
        }*/
        GenerateCreturesIcons();
        GenerateHealthBars();
    }

    private void ClickObject_OnHoverIn(ClickObject obj)
    {
        /*if (obj.Cursor != null) Cursor.SetCursor(obj.Cursor, new Vector2(25,25), CursorMode.ForceSoftware);

        if (obj.isCreature)
        {
            if (obj.GetComponent<CreatureStats>().team != 0)
            {
                Cursor.SetCursor(AttackCursorImage, new Vector2(25, 25), CursorMode.ForceSoftware);
            }
        }*/
    }

    private void ClickObject_OnHoverOut(ClickObject obj)
    {
        //Cursor.SetCursor(CursorImage, new Vector2(25, 25), CursorMode.ForceSoftware);
    }

    private void MatchSystem_OnChangePlayer(CreatureStats creature)
    {
        PlayerPanelScript panel = _playerPanels.FirstOrDefault(x => x.Creature == creature);

        if (panel != null)
        {
            foreach (PlayerPanelScript p in _playerPanels)
            {
                if (p == panel) p.gameObject.SetActive(true);
                else p.gameObject.SetActive(false);
            }
        }

        UpdateUISpellsMP();
    }

    private void InventorySystem_OnPickInventoryItem(CreatureStats creature, IInventoryItem item)
    {
        Notify = Instantiate<InventoryNotify>(prefabInventoryNotify, BaseCanvas.transform);
        Notify.SetItem(item);

        PlayerPanelScript panel = _playerPanels.FirstOrDefault(x => x.Creature == creature);

        if (panel != null)
        {
            GameObject newItem = Instantiate(prefabInventoryItem, panel.Inventory.transform);
            newItem.GetComponent<InventoryItemScript>().SetItem(item);
            _inventoryIcons.Add(item, newItem);
        }
    }

    private void InventorySystem_OnRemoveInventoryItem(CreatureStats creature, IInventoryItem item)
    {
        Destroy(_inventoryIcons[item]);
    }

    private void GenerateCreturesIcons()
    {
        foreach (CreatureStats cs in _creatures)
        {
            if (cs.tag == "DeathPlayer") continue;
            GameObject icon;
            if (cs.team == 0)
            {
                icon = Instantiate(prefabIcon, friendlyPanel.transform);
            }
            else
            {
                icon = Instantiate(prefabIcon, enemyPanel.transform);
            }
            icon.GetComponent<Image>().sprite = cs.avatar;
            icon.GetComponent<AvatarScript>().Creature = cs;
        }
    }

    private void GenerateHealthBars()
    {
        List<UIHealthBarScript> currents = FindObjectsOfType<UIHealthBarScript>().ToList();

        foreach (CreatureStats cs in _creatures)
        {
            if (cs.tag != "DeathPlayer" && !currents.Exists(x => x.Creature == cs))
            {
                GameObject healthBar = Instantiate(prefabHealthBar, LifeBarsContainer.transform);
                healthBar.GetComponent<UIHealthBarScript>().Creature = cs;
                //healthBar.GetComponent<UIHealthBarScript>().CalcHP();
            }
            
        }
    }

    private void GeneratePlayersPanels()
    {
        foreach (CreatureStats c in _creatures)
        {
            if (!c.GetComponent<AIScript>().isActiveAndEnabled)
            {
                GameObject panel = Instantiate(prefabPlayerPanel, BaseCanvas.transform);
                PlayerPanelScript component = panel.GetComponent<PlayerPanelScript>();
                component.Creature = c;
                component.UpdateParams();
                _playerPanels.Add(component);
                if (MatchSystem.instance.GetActivePlayer() == c) panel.SetActive(true);
            }
        }

        UpdateUISpellsMP();
    }

    private void UpdateUISpellsMP()
    {
        UISpell[] spells = FindObjectsOfType<UISpell>();
        foreach (UISpell spell in spells)
        {
            spell.CalcMP();
        }
    }
}
