using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Links")]
    [SerializeField] public GameObject visuel;
    //[SerializeField] public ItemData[] loot;

    [Header("Configurations")]
    public int experience;
    public string name_enemy;
    public int level_min;
    public int level_max;
    public int speed;
    public int pdvMax;
    public int pa;
    public int pm;
    public int initiative;
    public int vitalite;
    public int sagesse;
    public int force;
    public int intel;
    public int chance;
    public int agilite;
    public int resiNeutre;
    public int resiTerre;
    public int resiFeu;
    public int resiEau;
    public int resiAir;
    public TypeIA typeIA;

    public enum TypeIA
    {
        Close,
        Distance,
    }
}
