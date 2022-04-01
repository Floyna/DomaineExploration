using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BonusPanoData", menuName = "Game Bonus Pano Data")]
public class BonusPanoData : ScriptableObject
{
    public Panoplie panoplie;
    public ItemData[] itemPano;
    public BonusPanoplie[] bonus;
    public enum Panoplie
    {
        None,
        Aventurier,
        Bouftou
    }
}

[System.Serializable]
public class BonusPanoplie
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

    public TypeEffet[] type;
    public int[] jet;
}