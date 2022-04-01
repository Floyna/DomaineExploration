using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public TMP_InputField chatBox;

    public Color playerMessage, info, warning;

    [SerializeField] RectTransform rect;

    TestPlayer player;

    [SerializeField] GameObject panelCaract;
    [SerializeField] GameObject panelSort;
    [SerializeField] GameObject panelInventaire;
    [SerializeField] GameObject tilemapGrid;
    [SerializeField] GameObject tilemapFight;
    [SerializeField] GameObject floatingText;

    public bool shift;
    public bool alpha1;
    public bool leftAlt;
    private bool inLoad;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    public GameObject contentSpellPanel;
    [SerializeField] GameObject prefabSpell;
    // Start is called before the first frame update
    void Start()
    {
        SendMessageToChat("Bienvenue dans le monde des uns", Message.MessageType.warning);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<TestPlayer>();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && player.inFight && !chatBox.isFocused)
        {
            GameObject[] Slot = GameObject.FindGameObjectsWithTag("SlotSpell");
            Slot[0].GetComponent<SlotSpell>().PrepareSpell();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && player.inFight && !chatBox.isFocused)
        {
            GameObject[] Slot = GameObject.FindGameObjectsWithTag("SlotSpell");
            Slot[1].GetComponent<SlotSpell>().PrepareSpell();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && player.inFight && !chatBox.isFocused)
        {
            GameObject[] Slot = GameObject.FindGameObjectsWithTag("SlotSpell");
            Slot[2].GetComponent<SlotSpell>().PrepareSpell();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && player.inFight && !chatBox.isFocused)
        {
            GameObject[] Slot = GameObject.FindGameObjectsWithTag("SlotSpell");
            Slot[3].GetComponent<SlotSpell>().PrepareSpell();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && player.inFight && !chatBox.isFocused)
        {
            GameObject[] Slot = GameObject.FindGameObjectsWithTag("SlotSpell");
            Slot[4].GetComponent<SlotSpell>().PrepareSpell();
        }

        if (Input.GetKeyDown(KeyCode.S) && !inLoad && !chatBox.isFocused)
        {
            ShowPanel(panelSort);
        }

        if (Input.GetKeyDown(KeyCode.I) && !inLoad && !chatBox.isFocused)
        {
            ShowPanel(panelInventaire);
        }

        if (Input.GetKeyDown(KeyCode.C) && !inLoad && !chatBox.isFocused)
        {
            ShowPanel(panelCaract);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shift = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shift = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            alpha1 = true;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            alpha1 = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            leftAlt = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            leftAlt = false;
        }

        if (shift && alpha1 && !inLoad)
        {
            StartCoroutine(Show(tilemapGrid));
        }

        if (Input.GetKeyDown(KeyCode.A) && !inLoad && !chatBox.isFocused)
        {
            StartCoroutine(Show(tilemapFight));
        }

        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(player.username + ": " + chatBox.text, Message.MessageType.playerMessage);
                chatBox.text = "";
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject[] panels =  GameObject.FindGameObjectsWithTag("Panel");
            foreach (GameObject panel in panels)
            {
                if (panel.activeInHierarchy)
                {
                    panel.SetActive(false);
                }
            }

            player.canMove = true;
        }
    }

    IEnumerator Show(GameObject obj)
    {
        if (!inLoad)
        {
            inLoad = true;
            yield return new WaitForSeconds(.1f);
            if (obj.activeInHierarchy)
            {
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
            }
            inLoad = false;
        }
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
            player.canMove = true;
        }
        else
        {
            player.canMove = false;
            GameObject[] panels = GameObject.FindGameObjectsWithTag("Panel");
            foreach (var item in panels)
            {
                if (item.activeInHierarchy)
                {
                    item.SetActive(false);
                }
            }
            panel.SetActive(true);
            if (panel.gameObject.name == "PanelSpell")
            {
                LoadSpell();
            }
        }

    }

    public void LoadSpell()
    {
        if (contentSpellPanel.transform.childCount > 0)
        {
            for (int i = 0; i < contentSpellPanel.transform.childCount; i++)
            {
                Destroy(contentSpellPanel.transform.GetChild(i).gameObject);
            }
        }

        foreach (SpellData spell in FindObjectOfType<TestPlayer>().GetComponent<SpellManager>().spellDataList)
        {
            GameObject newSpell = Instantiate(prefabSpell, contentSpellPanel.transform);
            newSpell.GetComponent<DragAndDrop>().spellData = spell;
            newSpell.tag = "SpellUI";
            for (int i = 0; i < newSpell.transform.childCount; i++)
            {
                switch (newSpell.transform.GetChild(i).gameObject.name)
                {
                    case "Image":
                        newSpell.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = spell.visuelSpell;
                        break;
                    case "TextSpellName":
                        newSpell.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = spell.spellName;
                        break;
                    case "TextSpellCostPA":
                        newSpell.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = spell.pa.ToString() + " PA";
                        break;
                    case "TextSpellCostPO":
                        newSpell.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = spell.po[0].ToString() + " - " + spell.po[1].ToString() + " PO";
                        break;
                    case "TextSpellLevel":
                        newSpell.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = "Niveau " + spell.level[0].ToString();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void GenerateTextFloating(string text, Color color, Vector3 _transformPos)
    {
        GameObject txtFloating = Instantiate(floatingText, _transformPos, Quaternion.identity) as GameObject;
        txtFloating.transform.GetChild(0).GetComponent<TextMeshPro>().text = text;
        txtFloating.transform.GetChild(0).GetComponent<TextMeshPro>().color = color;
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        if (chatBox.text.ToLower() == "%stats%")
        {
            text = text.Substring(0, text.Length - 7) + "Vitalité(" + player.vitalite + ") Sagesse (" + player.sagesse + ") Force(" + player.force + ") Intel(" + player.intel + ") Chance(" + player.chance + ") Agilité(" + player.agilite + ")";
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = newMessage.text;

        newMessage.textObject.color = MessageType(messageType);

        messageList.Add(newMessage);
    }

    Color MessageType(Message.MessageType messageType)
    {
        Color color = Color.white;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
            case Message.MessageType.info:
                color = info;
                break;
            case Message.MessageType.warning:
                color = warning;
                break;
            default:
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info,
        warning
    }
}