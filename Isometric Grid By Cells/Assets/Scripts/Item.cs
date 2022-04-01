using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite visuelItem;
    public ItemData.Type typeItem;
    public ItemData.Panoplie panoplie;
    public ItemData.Categorie categorie;
    public string itemName;
    public string description;
    public int level;
    public int pods;
    public BonusPanoData bonusData;
    public Effet[] effets;
    public ItemData[] itemsRecette;
    public Caracteristique[] caracteristiques;
    public Condition[] conditions;
    public bool isEquiped;

    public void LoadItemData(ItemData itemData)
    {
        visuelItem = itemData.visuelItem;
        typeItem = itemData.typeItem;
        panoplie = itemData.panoplie;
        categorie = itemData.categorie;
        itemName = itemData.itemName;
        description = itemData.description;
        level = itemData.level;
        pods = itemData.pods;
        effets = itemData.effets;
        itemsRecette = itemData.itemsRecette;
        caracteristiques = itemData.caracteristiques;
        conditions = itemData.conditions;
        bonusData = itemData.bonusData;
        foreach (var effet in effets)
        {
            effet.jet = Random.Range(effet.jetRange[0], effet.jetRange[1] + 1);
        }
    }

    public void LoadItem(Item itemData)
    {
        visuelItem = itemData.visuelItem;
        typeItem = itemData.typeItem;
        panoplie = itemData.panoplie;
        categorie = itemData.categorie;
        itemName = itemData.itemName;
        description = itemData.description;
        level = itemData.level;
        pods = itemData.pods;
        effets = itemData.effets;
        itemsRecette = itemData.itemsRecette;
        caracteristiques = itemData.caracteristiques;
        conditions = itemData.conditions;
        bonusData = itemData.bonusData;

        foreach (var effet in effets)
        {
            effet.jet = Random.Range(effet.jetRange[0], effet.jetRange[1] + 1);
        }
    }

    public void Reset()
    {
        visuelItem = null;
        typeItem = ItemData.Type.None;
        panoplie = ItemData.Panoplie.None;
        categorie = ItemData.Categorie.None;
        itemName = "";
        description = "";
        level = 0;
        pods = 0;
        effets = null;
        itemsRecette = null;
        caracteristiques = null;
        conditions = null;
    }
}
