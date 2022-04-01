using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Links")]
    [SerializeField] public Sprite visuelItem;

    [Header("Configurations")]
    public Type typeItem;
    public Panoplie panoplie;
    public Categorie categorie;
    public string itemName;
    public string description;
    public int level;
    public int pods;
    public BonusPanoData bonusData;
    public Effet[] effets;
    public ItemData[] itemsRecette;
    public Caracteristique[] caracteristiques;
    public Condition[] conditions;

    public enum Type 
    {
        Equipements,
        Consommables,
        Ressources,
        Quetes,
        None
    }


    public enum Categorie
    {
        Arc,
        Baguette,
        Baton,
        Dague,
        Epee,
        Marteau,
        Pelle,
        Hache,
        Outil,
        Pioche,
        Faux,
        ArmeMagique,
        Chapeau,
        Cape,
        Sac,
        Amulette,
        Anneau,
        Ceinture,
        Bottes,
        Bouclier,
        Dofus,
        Familier,
        Dragodinde,
        Pierre,
        Filet,
        None
    }

    public enum Panoplie
    {
        None,
        Aventurier,
        Bouftou
    }
}

[System.Serializable]
public class Effet
{
    public enum TypeEffet
    {
        PA,
        PM,
        PO,
        Soin,
        Dommages,
        PerDo,
        Critique,
        Invocation,
        Pods,
        Prospection,
        Initiative,
        Vitalite,
        Sagesse,
        Force,
        Intelligence,
        Chance,
        Agilite,
        ResiNeutre,
        ResiTerre,
        ResiFeu,
        ResiEau,
        ResiAir,
        DoDoTerre,
        DoDoIntel,
        DoDoChance,
        DoDoAgilite,
        DoDoNeutre,
        MalusVitalite,
        MalusForce,
        MalusIntelligence,
        MalusChance,
        MalusAgilite,
        MalusSagesse,
        MalusPA,
        MalusPO,
        MalusDommages,
        MalusCritique,
        MalusInitiative
    }

    public TypeEffet type;
    public int[] jetRange;
    public int jet;
}

[System.Serializable]
public class Condition
{
    public enum TypeCondition
    {
        PA,
        PM,
        PO,
        Soin,
        Dommages,
        PerDo,
        Critique,
        Creature,
        Prospection,
        Initiative,
        Vitalite,
        Sagesse,
        Force,
        Intelligence,
        Chance,
        Agilite,
        ResiNeutre,
        ResiTerre,
        ResiFeu,
        ResiEau,
        ResiAir
    }

    public TypeCondition type;
    public int jet;
}

[System.Serializable]
public class Caracteristique
{
    public enum TypeCaract
    {
        PA,
        PO,
        BonusCritique,
        Critique
    }

    public TypeCaract typeCaract;

    public int[] jet;
}