using UnityEngine;

[CreateAssetMenu(fileName = "SpellData", menuName = "Game Spell Data")]

public class SpellData : ScriptableObject
{
    [Header("Links")]
    [SerializeField] public Sprite visuelSpell;
    //[SerializeField] public ItemData[] loot;

    [Header("Configurations")]
    public string spellName;
    public string description;
    public int[] level;
    public int[] po;
    public int[] crit;
    public SpellEffect[] effets;
    public int pa;
    public int nbrAttackPerTurn;
    public int nbrAttackPerEntite;
    public int nbrTurnBeforReAttack;
    public bool poModifialbe;
    public bool freeLdv;
    public bool inLineOnly;
    public bool celluleLibre;
    public TypeSpellForIA[] typeSpell;

    public enum TypeSpellForIA
    {
        DirectDamage,
        Dot,
        Heal,
        Buff,
        Invoc
    }

}

[System.Serializable]
public class SpellEffect
{
    public enum TypeEffect
    {
        Damage,
        Heal,
        VoleDeVie,
        BuffDamage,
        BuffPO,
        BuffPA,
        BuffPM,
        RetraitDamage,
        RetraitPO,
        RetraitPA,
        RetraitPM,
        StealDamage,
        StealPO,
        StealPA,
        StealPM,
        Attirance,
        Repulsion
    }

    public enum ElementSpell
    {
        None,
        Neutre,
        Terre,
        Feu,
        Eau,
        Air
    }

    public enum Etat
    {
        None,
        Lourd,
        Pesenteur
    }
    public TypeEffect type;
    public ElementSpell element;
    public Etat etat;
    public int[] jet;
    public int duration;
}
