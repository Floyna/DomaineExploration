using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    public Vector2 mousePos = Vector2.zero;
    [SerializeField] Inventaire inventaire;
    [SerializeField] GameObject panel_infoBulle;
    [SerializeField] GameObject panel_info_fight;
    [SerializeField] GameObject text_panel_infoBulle;
    [SerializeField] TextMeshProUGUI info_fight_name;
    [SerializeField] TextMeshProUGUI info_fight_level;
    [SerializeField] TextMeshProUGUI info_fight_pdv;
    [SerializeField] TextMeshProUGUI info_fight_pa;
    [SerializeField] TextMeshProUGUI info_fight_pm;
    [SerializeField] Image info_fight_sprite;
    [SerializeField] TextMeshProUGUI info_fight_resNeutre;
    [SerializeField] TextMeshProUGUI info_fight_resTerre;
    [SerializeField] TextMeshProUGUI info_fight_resFeu;
    [SerializeField] TextMeshProUGUI info_fight_resEau;
    [SerializeField] TextMeshProUGUI info_fight_resAir;
    [SerializeField] GridControl gridControl;
    public Color colorPano;

    public bool wantFight;
    public bool canDropSpell;
    public bool leftAlt;
    private bool altAffichage;

    private bool oneClick;
    private bool doubleClick;

    [HideInInspector]
    public GameObject currentSlot;

    private TestPlayer player;

    private void Start()
    {
        player = FindObjectOfType<TestPlayer>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<TestPlayer>();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            leftAlt = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            leftAlt = false;
            altAffichage = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Click());
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit)
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            clickable?.Click();
            //Debug.Log(hit.collider.name);

            if (hit.collider.tag == "SlotSpell")
            {
                canDropSpell = true;
                currentSlot = hit.collider.gameObject;
            }

            if (hit.collider.tag == "SlotItem" && Input.GetMouseButtonDown(0))
            {
                if (hit.collider.GetComponent<SlotItem>().item != null)
                {
                    inventaire.ShowInformations(hit.collider.GetComponent<SlotItem>().item);
                }
                else
                {
                    inventaire.currentItem = null;
                    inventaire.panelPreview.SetActive(false);
                    inventaire.panelBonusPano.SetActive(false);
                    inventaire.textTopNameTMP.text = "";
                    inventaire.textTopLevelTMP.text = "";
                }
            }

            if (hit.collider.tag == "SlotItem" && doubleClick && Input.GetMouseButtonDown(0))
            {
                if (hit.collider.GetComponent<SlotItem>().item != null && !hit.collider.GetComponent<SlotItem>().item.isEquiped)
                {
                    if (hit.collider.GetComponent<SlotItem>().item.typeItem == ItemData.Type.Equipements && hit.collider.GetComponent<SlotItem>().item.level <= player.level)
                    {
                        StartCoroutine(inventaire.PutEquipement(hit.collider.GetComponent<SlotItem>().item, hit.collider.GetComponent<SlotItem>()));
                    }
                    else if(hit.collider.GetComponent<SlotItem>().item.typeItem == ItemData.Type.Equipements && hit.collider.GetComponent<SlotItem>().item.level > player.level)
                    {
                        Debug.LogWarning("Level insufisant");
                    }
                }
                else if(hit.collider.GetComponent<SlotItem>().item != null && hit.collider.GetComponent<SlotItem>().item.isEquiped)
                {
                    StartCoroutine(inventaire.Unequip(hit.collider.GetComponent<SlotItem>().item, hit.collider.GetComponent<SlotItem>()));
                }
            }

            if (hit.collider.tag == "SlotItem" && GameObject.FindGameObjectsWithTag("Panel_Info_Item").Length == 0)
            {
                SlotItem slot = hit.collider.GetComponent<SlotItem>();
                
                if (slot.item != null)
                {
                    GameObject panel = Instantiate(panel_infoBulle, transform);
                    panel.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, 0);
                    panel.tag = "Panel_Info_Item";

                    GameObject newText = Instantiate(text_panel_infoBulle, panel.transform);
                    GameObject newTextEmpty = Instantiate(text_panel_infoBulle, panel.transform);
                    newText.GetComponent<TextMeshProUGUI>().text = slot.item.itemName + " (Niv. " + slot.item.level + ")";
                    newText.GetComponent<RectTransform>().sizeDelta = new Vector2(newText.GetComponent<TextMeshProUGUI>().text.Length * 8, newText.GetComponent<RectTransform>().sizeDelta.y);
                    newText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                    
                    for (int i = 0; i < slot.item.effets.Length; i++)
                    {
                        GameObject newTextEffet = Instantiate(text_panel_infoBulle, panel.transform);
                        switch (slot.item.effets[i].type)
                        {
                            case Effet.TypeEffet.PerDo:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + "% dommages";
                                break;
                            case Effet.TypeEffet.DoDoTerre:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "Dommages : " + slot.item.effets[i].jetRange[0] + " à " + slot.item.effets[i].jetRange[1] + " (terre)";
                                break;
                            case Effet.TypeEffet.DoDoIntel:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "Dommages : " + slot.item.effets[i].jetRange[0] + " à " + slot.item.effets[i].jetRange[1] + " (feu)";
                                break;
                            case Effet.TypeEffet.DoDoChance:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "Dommages : " + slot.item.effets[i].jetRange[0] + " à " + slot.item.effets[i].jetRange[1] + " (eau)";
                                break;
                            case Effet.TypeEffet.DoDoAgilite:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "Dommages : " + slot.item.effets[i].jetRange[0] + " à " + slot.item.effets[i].jetRange[1] + " (air)";
                                break;
                            case Effet.TypeEffet.ResiNeutre:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + "% resistance neutre";
                                break;
                            case Effet.TypeEffet.ResiAir:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + "% resistance air";
                                break;
                            case Effet.TypeEffet.ResiTerre:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + "% resistance terre";
                                break;
                            case Effet.TypeEffet.ResiEau:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + "% resistance eau";
                                break;
                            case Effet.TypeEffet.ResiFeu:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + "% resistance feu";
                                break;
                            default:
                                newTextEffet.GetComponent<TextMeshProUGUI>().text = "+" + slot.item.effets[i].jet + " " + slot.item.effets[i].type.ToString();
                                break;
                        }

                        newTextEffet.GetComponent<RectTransform>().sizeDelta = new Vector2(newTextEffet.GetComponent<TextMeshProUGUI>().text.Length * 10, newTextEffet.GetComponent<RectTransform>().sizeDelta.y);
                        newTextEffet.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
                        if (slot.item.panoplie != ItemData.Panoplie.None)
                        {
                            newText.GetComponent<TextMeshProUGUI>().color = colorPano;
                            newTextEffet.GetComponent<TextMeshProUGUI>().color = colorPano;
                        }
                    }

                }
            }

            if ((hit.collider.tag != "SlotItem") && GameObject.FindGameObjectsWithTag("Panel_Info_Item").Length > 0 )
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_Item"));
            }

            if (hit.collider.tag == "Player" && GameObject.FindGameObjectsWithTag("Panel_Info_User").Length == 0)
            {
                GameObject panel = Instantiate(panel_infoBulle, transform);

                panel.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, 0);
                panel.tag = "Panel_Info_User";
                GameObject newText = Instantiate(text_panel_infoBulle, panel.transform);

                if (!player.inFight)
                {
                    newText.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<TestPlayer>().username;
                }
                else
                {
                    if (!FindObjectOfType<FightManager>().inLoad)
                    {
                        newText.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<TestPlayer>().username + " (" + player.pdv + ")";
                    }
                    else
                    {
                        newText.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<TestPlayer>().username + " (" + player.level + ")";
                    }
                }
            }

            if ((hit.collider.tag != "Player") && GameObject.FindGameObjectsWithTag("Panel_Info_User").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_User"));
            }

            if (hit.collider.tag == "Enemy" && GameObject.FindGameObjectsWithTag("Panel_Info_Mob").Length == 0)
            {
                GameObject panel = Instantiate(panel_infoBulle, transform);
                panel.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, 0);
                panel.tag = "Panel_Info_Mob";

                if (!player.inFight)
                {
                    GameObject newTextTitreLevel = Instantiate(text_panel_infoBulle, panel.transform);

                    int levelTotal = 0;

                    foreach (var enemy in hit.collider.GetComponent<Groupe_Enemy>().listEnemy)
                    {
                        GameObject newText = Instantiate(text_panel_infoBulle, panel.transform);

                        newText.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<EnemyControl>().name_enemy + " (" + hit.collider.GetComponent<EnemyControl>().level + ")";

                        levelTotal += hit.collider.GetComponent<EnemyControl>().level;
                    }

                    newTextTitreLevel.GetComponent<TextMeshProUGUI>().text = "Level " + levelTotal.ToString();
                }
                else
                {
                    GameObject newText = Instantiate(text_panel_infoBulle, panel.transform);

                    if (!FindObjectOfType<FightManager>().inLoad)
                    {
                        newText.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<EnemyControl>().name_enemy + " (" + hit.collider.GetComponent<EnemyControl>().pdv + ")";
                    }
                    else
                    {
                        newText.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<EnemyControl>().name_enemy + " (" + hit.collider.GetComponent<EnemyControl>().level + ")";
                    }     
                } 
            }

            if ((hit.collider.tag != "Enemy") && GameObject.FindGameObjectsWithTag("Panel_Info_Mob").Length > 0 && !leftAlt)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_Mob"));
            }

            if (hit.collider.tag == "Enemy" && player.inFight)
            {
                hit = LoadPanelInfoE(hit);
            }
            else if (hit.collider.tag == "Player" && player.inFight)
            {
                LoadPanelInfoP();
            }
        }
        else
        {
            if (panel_infoBulle.activeInHierarchy)
            {
                ClearPanelInfo();
                panel_infoBulle.SetActive(false);
            }

            if (panel_info_fight.activeInHierarchy)
            {
                panel_info_fight.SetActive(false);
            }

            canDropSpell = false;
            currentSlot = null;

            if (GameObject.FindGameObjectsWithTag("Panel_Info_User").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_User"));
            }

            if (GameObject.FindGameObjectsWithTag("Panel_Info_Mob").Length > 0 && !leftAlt)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_Mob"));
            }

            if (GameObject.FindGameObjectsWithTag("Panel_Info_Item").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_Item"));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.FindGameObjectsWithTag("Panel_Info_User").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_User"));
            }

            if (GameObject.FindGameObjectsWithTag("Panel_Info_Mob").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_Mob"));
            }

            if (GameObject.FindGameObjectsWithTag("Panel_Info_Item").Length > 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Panel_Info_Item"));
            }
        }

        if (Input.GetMouseButtonDown(0) && hit)
        {
            if (hit.collider.tag == "Enemy")
            {
                wantFight = true;
            }
        }
        else if (Input.GetMouseButtonDown(0) && !hit)
        {
            wantFight = false;
        }

        if (!player.inFight && leftAlt)
        {
            if (!altAffichage)
            {
                altAffichage = true;
                GameObject[] groupes = GameObject.FindGameObjectsWithTag("Enemy");
                if (groupes.Length > 0)
                {
                    foreach (GameObject enemyObj in groupes)
                    {
                        GameObject panel = Instantiate(panel_infoBulle, transform);
                        panel.transform.position = new Vector3(enemyObj.transform.position.x, enemyObj.transform.position.y + 1, 0);
                        panel.tag = "Panel_Info_Mob";

                        if (!player.inFight)
                        {
                            GameObject newTextTitreLevel = Instantiate(text_panel_infoBulle, panel.transform);

                            int levelTotal = 0;

                            foreach (var enemy in enemyObj.GetComponent<Groupe_Enemy>().listEnemy)
                            {
                                GameObject newText = Instantiate(text_panel_infoBulle, panel.transform);

                                newText.GetComponent<TextMeshProUGUI>().text = enemyObj.GetComponent<EnemyControl>().name_enemy + " (" + enemyObj.GetComponent<EnemyControl>().level + ")";

                                levelTotal += enemyObj.GetComponent<EnemyControl>().level;
                            }

                            newTextTitreLevel.GetComponent<TextMeshProUGUI>().text = "Level " + levelTotal.ToString();
                        }
                    }
                }
            }
        }
    }

    private void LoadPanelInfoP()
    {
        info_fight_name.text = player.username;
        info_fight_level.text = "Niveau " + player.level.ToString();
        info_fight_pdv.text = "PDV " + player.pdv.ToString();
        info_fight_pa.text = "PA " + player.pa.ToString();
        info_fight_pm.text = "PM " + player.pm.ToString();
        info_fight_sprite.sprite = player.spriteRender.sprite;
        info_fight_resNeutre.text = player.resiNeutre.ToString() + "%";
        info_fight_resTerre.text = player.resiTerre.ToString() + "%";
        info_fight_resFeu.text = player.resiFeu.ToString() + "%";
        info_fight_resEau.text = player.resiEau.ToString() + "%";
        info_fight_resAir.text = player.resiAir.ToString() + "%";
        panel_info_fight.SetActive(true);
    }

    private RaycastHit2D LoadPanelInfoE(RaycastHit2D hit)
    {
        EnemyControl enemy = hit.collider.GetComponent<EnemyControl>();
        info_fight_name.text = enemy.name_enemy;
        info_fight_level.text = "Niveau " + enemy.level.ToString();
        info_fight_pdv.text = "PDV " + enemy.pdv.ToString();
        info_fight_pa.text = "PA " + enemy.pa.ToString();
        info_fight_pm.text = "PM " + enemy.pm.ToString();
        info_fight_sprite.sprite = enemy.visuel_enemy;
        info_fight_resNeutre.text = enemy.resiNeutre.ToString() + "%";
        info_fight_resTerre.text = enemy.resiTerre.ToString() + "%";
        info_fight_resFeu.text = enemy.resiFeu.ToString() + "%";
        info_fight_resEau.text = enemy.resiEau.ToString() + "%";
        info_fight_resAir.text = enemy.resiAir.ToString() + "%";
        panel_info_fight.SetActive(true);
        return hit;
    }

    IEnumerator Click()
    {
        if (!oneClick)
        {
            //Debug.Log("OneClick");
            oneClick = true;
            yield return new WaitForSeconds(.25f);
            oneClick = false;
        }
        else if (oneClick)
        {
            //Debug.Log("DoubleClick");
            oneClick = false;
            doubleClick = true;
            yield return new WaitForSeconds(.1f);
            doubleClick = false;
        }
    }

    public void ClearPanelInfo()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Text_Panel_Enemy");

        foreach (var item in toDestroy)
        {
            Destroy(item);
        }
    }

    bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
