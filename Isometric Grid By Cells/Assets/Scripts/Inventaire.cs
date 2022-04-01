using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
    public int currentSlot;
    public int slotMax;
    public int currentPods;
    public int podsMax;
    public bool inLoad;

    public List<GameObject> listItem = new List<GameObject>();
    [SerializeField] ItemData[] listItemData;
    [SerializeField] Color colorPano;
    [SerializeField] Color colorPreviewStat;
    [SerializeField] Color colorMalus;
    [SerializeField] Color colorPreviewDescription;
    [SerializeField] Color colorBackStats1;
    [SerializeField] Color colorBackStats2;
    [SerializeField] Color colorWhite;
    [SerializeField] Color colorBlack;

    [Header("Links")]
    [SerializeField] TextMeshProUGUI textTopPanelObj;
    [SerializeField] GameObject inventaireItems;
    [SerializeField] GameObject content;
    [SerializeField] GameObject prefabSlotObj;

    [Header("StuffPlayer")]
    [SerializeField] SlotItem coiffe;
    [SerializeField] SlotItem cape;
    [SerializeField] SlotItem amulette;
    [SerializeField] SlotItem ceinture;
    [SerializeField] SlotItem anneau1;
    [SerializeField] SlotItem anneau2;
    [SerializeField] SlotItem bottes;
    [SerializeField] SlotItem cac;
    [SerializeField] SlotItem bouclier;
    [SerializeField] SlotItem familier;
    [SerializeField] GameObject[] SlotsDofus;

    [Header("LinksPreview")]
    [SerializeField] public GameObject panelPreview;
    [SerializeField] GameObject textTopName;
    [SerializeField] GameObject textTopLevel;
    [SerializeField] public TextMeshProUGUI textTopNameTMP;
    [SerializeField] public TextMeshProUGUI textTopLevelTMP;
    [SerializeField] TextMeshProUGUI textpods;
    [SerializeField] GameObject prefabDescription;
    [SerializeField] GameObject prefabText;
    [SerializeField] GameObject prefabTextStats;
    [SerializeField] GameObject contentStats;
    [SerializeField] GameObject contentDescription;
    [SerializeField] Image imgInformation;
    [SerializeField] VerticalLayoutGroup verticalLayoutDescription;
    [SerializeField] Sprite[] spritesElement;

    [Header("PanelPreviewPlayer")]
    [SerializeField] Image imgPlayer;
    [SerializeField] TextMeshProUGUI textPanelTopPrevPlayer;

    [Header("ButtonsPreviewItem")]
    [SerializeField] Image buttonEffets;
    [SerializeField] Image buttonConditions;
    [SerializeField] Image buttonCaracteristiques;
    [SerializeField] GameObject buttonCaracteristiquesObj;

    [Header("BonusPano")]
    [SerializeField] public GameObject panelBonusPano;
    [SerializeField] TextMeshProUGUI titrePano;
    [SerializeField] GameObject prefabTextPano;
    [SerializeField] GameObject contentBonusPano;
    [SerializeField] GameObject listObjetEquipe;
    [SerializeField] GameObject prefabObjEquipe;

    [HideInInspector]
    public Item currentItem;

    void Start()
    {
        for (int i = 0; i < slotMax; i++)
        {
            GameObject slot =  Instantiate(prefabSlotObj, content.transform);
            slot.GetComponent<SlotItem>().imageObj.SetActive(false);
        }

        currentSlot = slotMax;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, slotMax * 20);

        for (int i = 0; i < listItemData.Length; i++)
        {
            GameObject g = new GameObject();
            g.transform.parent = inventaireItems.transform;
            g.AddComponent<Item>();
            g.GetComponent<Item>().LoadItemData(listItemData[i]);
            g.transform.name = g.GetComponent<Item>().itemName;
            listItem.Add(g);
        }

        SlotItem[] slots = FindObjectsOfType<SlotItem>();
        for (int i = 0; i < listItem.Count; i++)
        {
            if (!slots[slots.Length - i - 1].isPlayerSlot)
            {
                slots[slots.Length - i - 1].item = inventaireItems.transform.GetChild(i).GetComponent<Item>();
                slots[slots.Length - i - 1].item.LoadItem(listItem[i].GetComponent<Item>());
                slots[slots.Length - i - 1].imageObj.GetComponent<Image>().sprite = listItemData[i].visuelItem;
                slots[slots.Length - i - 1].imageObj.GetComponent<Image>().color = Color.white;
                slots[slots.Length - i - 1].imageObj.SetActive(true);
            }
        }

        panelBonusPano.SetActive(false);
        textPanelTopPrevPlayer.text = "Aperçu de " + FindObjectOfType<TestPlayer>().username;
        imgPlayer.sprite = FindObjectOfType<TestPlayer>().GetComponent<SpriteRenderer>().sprite;
        ShowCategorie("Equipements");
        ResetPreviewItem();
    }

    public IEnumerator PutEquipement(Item item, SlotItem slotBefor)
    {
        if (!inLoad)
        {
            inLoad = true;
            SlotItem slotToPick = cac;

            switch (item.categorie)
            {
                case ItemData.Categorie.Arc:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Baguette:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Baton:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Dague:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Epee:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Marteau:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Pelle:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Hache:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Outil:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Pioche:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Faux:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.ArmeMagique:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Chapeau:
                    slotToPick = coiffe;
                    break;
                case ItemData.Categorie.Cape:
                    slotToPick = cape;
                    break;
                case ItemData.Categorie.Sac:
                    slotToPick = cape;
                    break;
                case ItemData.Categorie.Amulette:
                    slotToPick = amulette;
                    break;
                case ItemData.Categorie.Anneau:
                    if (anneau1.item == null)
                    {
                        slotToPick = anneau1;
                    }
                    else if(anneau2.item == null)
                    {
                        slotToPick = anneau2;
                    }
                    else
                    {
                        int rand = Random.Range(1,3);
                        if (rand == 1)
                        {
                            slotToPick = anneau1;
                        }
                        else
                        {
                            slotToPick = anneau2;
                        }
                    }
                    break;
                case ItemData.Categorie.Ceinture:
                    slotToPick = ceinture;
                    break;
                case ItemData.Categorie.Bottes:
                    slotToPick = bottes;
                    break;
                case ItemData.Categorie.Bouclier:
                    slotToPick = bouclier;
                    break;
                case ItemData.Categorie.Dofus:
                    foreach (GameObject slot in SlotsDofus)
                    {
                        if (slot.GetComponent<SlotItem>().item == null)
                        {
                            slotToPick = slot.GetComponent<SlotItem>();
                            break;
                        }
                    }
                    if (slotToPick == cac)
                    {
                        Debug.Log("All slot occuped");
                        slotToPick = SlotsDofus[0].GetComponent<SlotItem>();
                    }
                    break;
                case ItemData.Categorie.Familier:
                    slotToPick = familier;
                    break;
                case ItemData.Categorie.Dragodinde:
                    break;
                case ItemData.Categorie.Pierre:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.Filet:
                    slotToPick = cac;
                    break;
                case ItemData.Categorie.None:
                    break;
                default:
                    break;
            }

            if (slotToPick.item == null)
            {
                slotToPick.item = item;
                slotToPick.transform.GetChild(0).GetComponent<Image>().sprite = item.visuelItem;
                slotToPick.imageObj.SetActive(true);
                slotToPick.item.isEquiped = true;
                slotBefor.item = null;
                slotBefor.transform.GetChild(0).transform.gameObject.SetActive(false);
                ShowCategorie("Equipements");
            }
            else
            {
                slotToPick.item.isEquiped = false;
                slotToPick.item = item;
                slotToPick.transform.GetChild(0).GetComponent<Image>().sprite = item.visuelItem;
                slotToPick.imageObj.SetActive(true);
                slotToPick.item.isEquiped = true;
                slotBefor.item = null;
                slotBefor.transform.GetChild(0).transform.gameObject.SetActive(false);
                ShowCategorie("Equipements");
            }

            if (item.panoplie != ItemData.Panoplie.None)
            {
                ShowBonusPano();
            }

            yield return new WaitForSeconds(.1f);
            inLoad = false;
        }
    }

    public IEnumerator Unequip(Item item, SlotItem slotBefor)
    {
        if (!inLoad)
        {
            inLoad = true;
            item.isEquiped = false;
            slotBefor.item = null;
            slotBefor.transform.GetChild(0).GetComponent<Image>().sprite = null;
            slotBefor.transform.GetChild(0).gameObject.SetActive(false);
            ShowCategorie("Equipements");
            if (item.panoplie != ItemData.Panoplie.None)
            {
                ShowBonusPano();
            }
            yield return new WaitForSeconds(.1f);
            inLoad = false;
        }
    }

    public void ShowCategorie(string type)
    {
        textTopPanelObj.text = type;
        SlotItem[] slots = FindObjectsOfType<SlotItem>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && !slots[i].isPlayerSlot)
            {
                slots[i].imageObj.GetComponent<Image>().sprite = null;
                slots[i].item = null;
                slots[i].imageObj.SetActive(false);
            }
        }

        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].GetComponent<Item>().typeItem.ToString() == type && !listItem[i].GetComponent<Item>().isEquiped)
            {
                for (int j = 0; j < slots.Length; j++)
                {
                    if (slots[slots.Length - j - 1].item == null && !slots[slots.Length - j - 1].isPlayerSlot)
                    {
                        slots[slots.Length - j - 1].item = listItem[i].GetComponent<Item>();
                        slots[slots.Length - j - 1].imageObj.GetComponent<Image>().sprite = listItem[i].GetComponent<Item>().visuelItem;
                        slots[slots.Length - j - 1].imageObj.GetComponent<Image>().color = Color.white;
                        slots[slots.Length - j - 1].imageObj.SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    public void ResetPreviewItem()
    {
        panelPreview.SetActive(false);
        textTopLevel.SetActive(false);
        textTopName.SetActive(false);
    }


    public void ShowInformations(Item item)
    {
        currentItem = item;
        panelPreview.SetActive(true);
        textTopName.SetActive(true);
        textTopLevel.SetActive(true);
        textTopNameTMP.text = item.itemName;
        textTopLevelTMP.text = "Niv." + item.level.ToString();
        textpods.text = item.pods.ToString() + " pods";
        imgInformation.sprite = item.visuelItem;

        if (item.panoplie != ItemData.Panoplie.None)
        {
            textTopNameTMP.color = colorPano;
        }
        else
        {
            textTopNameTMP.color = Color.white;
        }

        ShowStatsItem();
        ShowDescriptionItem();
        if (item.categorie == ItemData.Categorie.Arc || item.categorie == ItemData.Categorie.Baguette || item.categorie == ItemData.Categorie.Baton ||
            item.categorie == ItemData.Categorie.Dague || item.categorie == ItemData.Categorie.Epee || item.categorie == ItemData.Categorie.Faux ||
            item.categorie == ItemData.Categorie.Filet || item.categorie == ItemData.Categorie.Hache || item.categorie == ItemData.Categorie.Marteau ||
            item.categorie == ItemData.Categorie.Outil || item.categorie == ItemData.Categorie.Pelle || item.categorie == ItemData.Categorie.Pierre ||
            item.categorie == ItemData.Categorie.Pioche)
        {
            buttonCaracteristiquesObj.SetActive(true);
        }
        else
        {
            buttonCaracteristiquesObj.SetActive(false);
        }

        if (item.panoplie != ItemData.Panoplie.None)
        {
            panelBonusPano.SetActive(true);
            ShowBonusPano();
        }
        else
        {
            panelBonusPano.SetActive(false);
        }
    }

    public void ShowStatsItem()
    {
        buttonEffets.color = colorBackStats2;
        buttonEffets.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
        buttonConditions.color = colorBlack;
        buttonConditions.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        buttonCaracteristiques.color = colorBlack;
        buttonCaracteristiques.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        if (contentStats.transform.childCount > 0)
        {
            for (int i = 0; i < contentStats.transform.childCount; i++)
            {
                Destroy(contentStats.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < currentItem.effets.Length; i++)
        {
            GameObject newTextEffet = Instantiate(prefabTextStats, contentStats.transform);
            newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewStat;

            foreach (Sprite sprite in spritesElement)
            {
                if (sprite.name == currentItem.effets[i].type.ToString())
                {
                    newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                }
            }

            if (i % 2 == 0)
            {
                newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats2;
            }
            else
            {
                newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats1;
            }
            //newTextEffet.GetComponent<RectTransform>().sizeDelta = new Vector2(650, 35);
            newTextEffet.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);
            //newTextEffet.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector2(0, 0);

            switch (currentItem.effets[i].type)
            {
                case Effet.TypeEffet.PA:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.PM:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.PO:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " à la portée";
                    break;
                case Effet.TypeEffet.Soin:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Dommages:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.PerDo:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Augmente les dommages de " + currentItem.effets[i].jet + "%";
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Dommages")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    break;
                case Effet.TypeEffet.Critique:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " aux coups critiques";
                    break;
                case Effet.TypeEffet.Invocation:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Pods:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Prospection:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Initiative:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Vitalite:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Sagesse:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Force:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Intelligence:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Chance:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.Agilite:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + " " + currentItem.effets[i].type.ToString();
                    break;
                case Effet.TypeEffet.ResiNeutre:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + "% resistance neutre";
                    break;
                case Effet.TypeEffet.ResiTerre:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + "% resistance terre";
                    break;
                case Effet.TypeEffet.ResiFeu:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + "% resistance feu";
                    break;
                case Effet.TypeEffet.ResiEau:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + "% resistance eau";
                    break;
                case Effet.TypeEffet.ResiAir:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.effets[i].jet + "% resistance air";
                    break;
                case Effet.TypeEffet.DoDoTerre:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Dommages : " + currentItem.effets[i].jetRange[0] + " à " + currentItem.effets[i].jetRange[1] + " (terre)";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Force")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    break;
                case Effet.TypeEffet.DoDoIntel:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Dommages : " + currentItem.effets[i].jetRange[0] + " à " + currentItem.effets[i].jetRange[1] + " (feu)";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Intelligence")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    break;
                case Effet.TypeEffet.DoDoChance:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Dommages : " + currentItem.effets[i].jetRange[0] + " à " + currentItem.effets[i].jetRange[1] + " (eau)";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Chance")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    break;
                case Effet.TypeEffet.DoDoAgilite:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Dommages : " + currentItem.effets[i].jetRange[0] + " à " + currentItem.effets[i].jetRange[1] + " (air)";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Agilite")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    break;
                case Effet.TypeEffet.DoDoNeutre:
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Dommages : " + currentItem.effets[i].jetRange[0] + " à " + currentItem.effets[i].jetRange[1] + " (neutre)";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "ResiNeutre")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    break;
                case Effet.TypeEffet.MalusVitalite:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Vilalite")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Vitalite";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusForce:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Force")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Force";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusIntelligence:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Intelligence")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Intelligence";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusChance:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Chance")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Chance";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusAgilite:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Agilite")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Agilite";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusSagesse:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Sagesse")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Sagesse";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusPA:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "PA")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " PA";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusPO:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "PO")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " PO";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusDommages:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Dommages")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Dommages";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusCritique:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Critique")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Critique";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                case Effet.TypeEffet.MalusInitiative:
                    foreach (Sprite sprite in spritesElement)
                    {
                        if (sprite.name == "Initiative")
                        {
                            newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                        }
                    }
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "-" + currentItem.effets[i].jet + " Initiative";
                    newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorMalus;
                    break;
                default:
                    break;
            }

            if (newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite == null)
            {
                newTextEffet.transform.GetChild(2).GetComponent<Image>().enabled = false;
            }
        }
    }

    public void ShowConditionsItem()
    {
        buttonEffets.color = colorBlack;
        buttonEffets.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        buttonConditions.color = colorBackStats2;
        buttonConditions.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
        buttonCaracteristiques.color = colorBlack;
        buttonCaracteristiques.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        if (contentStats.transform.childCount > 0)
        {
            for (int i = 0; i < contentStats.transform.childCount; i++)
            {
                Destroy(contentStats.transform.GetChild(i).gameObject);
            }
        }

        if (currentItem.conditions.Length > 0)
        {
            for (int i = 0; i < currentItem.conditions.Length; i++)
            {
                GameObject newTextEffet = Instantiate(prefabTextStats, contentStats.transform);
                newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                newTextEffet.transform.GetChild(2).gameObject.SetActive(false);

                if (i % 2 == 0)
                {
                    newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats2;
                }
                else
                {
                    newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats1;
                }

                newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentItem.conditions[i].type.ToString() + " > " + currentItem.conditions[i].jet.ToString();
            }
        }
        else
        {
            GameObject newTextEffet = Instantiate(prefabTextStats, contentStats.transform);
            newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats2;
            newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
            newTextEffet.transform.GetChild(2).gameObject.SetActive(false);
            newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Aucune";
        }
    }

    private void ShowDescriptionItem()
    {
        if (contentDescription.transform.childCount > 0)
        {
            for (int i = 0; i < contentDescription.transform.childCount; i++)
            {
                Destroy(contentDescription.transform.GetChild(i).gameObject);
            }
        }

        GameObject newTextTitreInfoDescription = Instantiate(prefabText, contentDescription.transform);
        newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
        newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().fontSizeMin = 26;
        newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().fontSizeMax = 26;
        newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;

        if (currentItem.panoplie != ItemData.Panoplie.None)
        {
            newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().text = "Panoplie " + currentItem.panoplie.ToString() + " (" + "Catégorie : " + currentItem.categorie + ")";
        }
        else
        {
            newTextTitreInfoDescription.GetComponent<TextMeshProUGUI>().text = "(" + "Catégorie : " + currentItem.categorie + ")";
        }

        newTextTitreInfoDescription.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 0);

        GameObject newTextDescription = Instantiate(prefabDescription, contentDescription.transform);
        newTextDescription.GetComponent<TextMeshProUGUI>().text = currentItem.description;
        newTextDescription.GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
    }

    public void ShowCaracteristiques()
    {
        buttonEffets.color = colorBlack;
        buttonEffets.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        buttonConditions.color = colorBlack;
        buttonConditions.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        buttonCaracteristiques.color = colorBackStats2;
        buttonCaracteristiques.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;

        if (contentStats.transform.childCount > 0)
        {
            for (int i = 0; i < contentStats.transform.childCount; i++)
            {
                Destroy(contentStats.transform.GetChild(i).gameObject);
            }
        }

        if (currentItem.caracteristiques.Length > 0)
        {
            for (int i = 0; i < currentItem.caracteristiques.Length; i++)
            {
                GameObject newTextEffet = Instantiate(prefabTextStats, contentStats.transform);
                newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewDescription;
                newTextEffet.transform.GetChild(2).gameObject.SetActive(false);

                if (i % 2 == 0)
                {
                    newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats2;
                }
                else
                {
                    newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats1;
                }

                switch (currentItem.caracteristiques[i].typeCaract)
                {
                    case Caracteristique.TypeCaract.PA:
                        newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentItem.caracteristiques[i].typeCaract.ToString() + " : " + currentItem.caracteristiques[i].jet[0].ToString();
                        break;
                    case Caracteristique.TypeCaract.PO:
                        newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Portée : " + currentItem.caracteristiques[i].jet[0].ToString() + " à " + currentItem.caracteristiques[i].jet[1].ToString();
                        break;
                    case Caracteristique.TypeCaract.BonusCritique:
                        newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Bonus coups critiques : +" + currentItem.caracteristiques[i].jet[0].ToString();
                        break;
                    case Caracteristique.TypeCaract.Critique:
                        newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Critique : " + currentItem.caracteristiques[i].jet[0].ToString() + "/" + currentItem.caracteristiques[i].jet[1].ToString();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ShowBonusPano()
    {
        titrePano.text = "Panoplie " + currentItem.panoplie.ToString();
        titrePano.color = colorPano;
        int countBonus = 0;
        if (contentBonusPano.transform.childCount > 0)
        {
            for (int i = 0; i < contentBonusPano.transform.childCount; i++)
            {
                Destroy(contentBonusPano.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < inventaireItems.transform.childCount; i++)
        {
            if (inventaireItems.transform.GetChild(i).GetComponent<Item>().isEquiped && inventaireItems.transform.GetChild(i).GetComponent<Item>().panoplie == currentItem.panoplie)
            {
                countBonus++;
            }
        }

        for (int i = 0; i < listObjetEquipe.transform.childCount; i++)
        {
            Destroy(listObjetEquipe.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < currentItem.bonusData.itemPano.Length; i++)
        {
            GameObject prefItem = Instantiate(prefabObjEquipe, listObjetEquipe.transform);
            prefItem.transform.GetChild(1).GetComponent<Image>().sprite = currentItem.bonusData.itemPano[i].visuelItem;
            prefItem.transform.name = currentItem.bonusData.itemPano[i].itemName;
        }

        // pour chaque objet de l'inventaire
        for (int a = 0; a < inventaireItems.transform.childCount; a++)
        {
            // pour chaque objet equiper && egale
            if (inventaireItems.transform.GetChild(a).GetComponent<Item>().isEquiped && inventaireItems.transform.GetChild(a).GetComponent<Item>().panoplie.ToString() == currentItem.bonusData.panoplie.ToString())
            {
                // on cherche l'item dans le content de la listObjEquipe
                for (int i = 0; i < listObjetEquipe.transform.childCount; i++)
                {
                    if (listObjetEquipe.transform.GetChild(i).transform.name == inventaireItems.transform.GetChild(a).GetComponent<Item>().itemName)
                    {
                        listObjetEquipe.transform.GetChild(i).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    }
                }
            }
        }

        if (countBonus > 1)
        {
            for (int i = 0; i < currentItem.bonusData.bonus[countBonus - 2].type.Length; i++)
            {
                GameObject newTextEffet = Instantiate(prefabTextPano, contentBonusPano.transform);
                newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = colorPreviewStat;

                foreach (Sprite sprite in spritesElement)
                {
                    if (sprite.name == currentItem.bonusData.bonus[countBonus - 2].type[i].ToString())
                    {
                        newTextEffet.transform.GetChild(2).GetComponent<Image>().sprite = sprite;
                    }
                }

                if (i % 2 == 0)
                {
                    newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats2;
                }
                else
                {
                    newTextEffet.transform.GetChild(0).GetComponent<Image>().color = colorBackStats1;
                }

                switch (currentItem.bonusData.bonus[countBonus - 2].type[i])
                {
                    case BonusPanoplie.TypeEffet.PerDo:
                        break;
                    case BonusPanoplie.TypeEffet.ResiNeutre:
                        break;
                    case BonusPanoplie.TypeEffet.ResiTerre:
                        break;
                    case BonusPanoplie.TypeEffet.ResiFeu:
                        break;
                    case BonusPanoplie.TypeEffet.ResiEau:
                        break;
                    case BonusPanoplie.TypeEffet.ResiAir:
                        break;
                    case BonusPanoplie.TypeEffet.MalusVitalite:
                        break;
                    case BonusPanoplie.TypeEffet.MalusForce:
                        break;
                    case BonusPanoplie.TypeEffet.MalusIntelligence:
                        break;
                    case BonusPanoplie.TypeEffet.MalusChance:
                        break;
                    case BonusPanoplie.TypeEffet.MalusAgilite:
                        break;
                    case BonusPanoplie.TypeEffet.MalusSagesse:
                        break;
                    case BonusPanoplie.TypeEffet.MalusPA:
                        break;
                    case BonusPanoplie.TypeEffet.MalusPO:
                        break;
                    case BonusPanoplie.TypeEffet.MalusDommages:
                        break;
                    case BonusPanoplie.TypeEffet.MalusCritique:
                        break;
                    case BonusPanoplie.TypeEffet.MalusInitiative:
                        break;
                    default:
                        newTextEffet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + currentItem.bonusData.bonus[countBonus - 2].jet[i].ToString() + " " + currentItem.bonusData.bonus[countBonus - 2].type[i].ToString();
                        break;
                }
            }
        } 
    }
}
