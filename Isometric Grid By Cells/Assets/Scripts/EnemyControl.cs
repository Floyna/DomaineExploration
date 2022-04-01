using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Informations")]
    public bool alive;
    public bool inFight;
    public bool isOnFinalDestPoint;
    public float speed;
    public int experience;

    [Header("Caracteristiques")]
    public string name_enemy;
    public int level;
    public int level_min;
    public int level_max;
    public int pdv;
    public int pdvMax;
    public int pa;
    public int pm;
    public int paMax;
    public int pmMax;
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

    [Header("Others")]
    public Vector2Int gridPos;
    public Sprite visuel_enemy;
    GridControl gridControl;
    public int destPoint = 0;
    private bool posSave;
    public Vector2 curPos;
    public Vector2 lastPath;
    public List<PathNode> pathList = new List<PathNode>();
    public List<PathNode> pathListLine = new List<PathNode>();
    List<PathNode> list = new List<PathNode>();
    List<SpellData> listInPo = new List<SpellData>();

    [Header("Link")]
    [SerializeField] EnemyData enemyData;
    [SerializeField] SpellData[] listSpell;
    [SerializeField] CircleCollider2D circleColllider;
    [SerializeField] Rigidbody2D rb;

    public bool myTurn;
    public bool inMove;
    public bool willBeInCac;
    public bool inCacPlayer;
    public bool inAttack;
    public bool willBeInPo;
    public TypeIA typeIA;

    public int minCoutPA;

    // Start is called before the first frame update
    void Start()
    {
        myTurn = false;
        if (enemyData != null)
        {
            LoadEnemy(enemyData);
        }

        gridControl = FindObjectOfType<GridControl>();

        isOnFinalDestPoint = true;
        level = Random.Range(level_min, level_max + 1);

        minCoutPA = 99;
    }

    private void FixedUpdate()
    {
        if (myTurn && !isOnFinalDestPoint)
        {
            Move(list);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (myTurn && !inMove && isOnFinalDestPoint && !inAttack && inFight)
        {
            foreach (SpellData _spell in listSpell)
            {
                if (_spell.pa < minCoutPA)
                {
                    minCoutPA = _spell.pa;
                }
            }
            if (pa >= minCoutPA)
            {
                for (int i = 0; i < listSpell.Length; i++)
                {
                    for (int j = 0; j < listSpell[i].typeSpell.Length; j++)
                    {
                        if ((TypeSpell)listSpell[i].typeSpell[j] == TypeSpell.Invoc)
                        {

                        }
                        else if ((TypeSpell)listSpell[i].typeSpell[j] == TypeSpell.Heal)
                        {

                        }
                        else if ((TypeSpell)listSpell[i].typeSpell[j] == TypeSpell.Buff)
                        {

                        }
                        else if ((TypeSpell)listSpell[i].typeSpell[j] == TypeSpell.Dot)
                        {

                        }
                        else if ((TypeSpell)listSpell[i].typeSpell[j] == TypeSpell.DirectDamage)
                        {
                            listInPo.Clear();
                            List<SpellData> listCanBeInPoFree = new List<SpellData>();
                            List<SpellData> listCanBeInPoLine = new List<SpellData>();

                            for (int k = 0; k < listSpell.Length; k++)
                            {
                                if (inPO(new Vector2Int(gridControl.currentX, gridControl.currentY), listSpell[k]) && pa >= listSpell[k].pa && (TypeSpell)listSpell[k].typeSpell[0] == TypeSpell.DirectDamage)
                                {
                                    listInPo.Add(listSpell[k]);
                                }
                                else if (listSpell[k].freeLdv && CanBeInPo(listSpell[k]) && pa >= listSpell[k].pa && (TypeSpell)listSpell[k].typeSpell[0] == TypeSpell.DirectDamage)
                                {
                                    listCanBeInPoFree.Add(listSpell[k]);
                                }
                                else if (listSpell[k].inLineOnly && CanBeInPo(listSpell[k]) && pa >= listSpell[k].pa && (TypeSpell)listSpell[k].typeSpell[0] == TypeSpell.DirectDamage)
                                {
                                    listCanBeInPoLine.Add(listSpell[k]);
                                }
                            }

                            if (listInPo.Count > 0)
                            {
                                foreach (var item in listInPo)
                                {
                                    Debug.Log("inPO : " + item.spellName);
                                }
                            }
                            if (listCanBeInPoFree.Count > 0)
                            {
                                foreach (var item in listCanBeInPoFree)
                                {
                                    Debug.Log("CanBeInPOFree : " + item.spellName);
                                }
                            }
                            if (listCanBeInPoLine.Count > 0)
                            {
                                foreach (var item in listCanBeInPoLine)
                                {
                                    Debug.Log("CanBeInPOLine : " + item.spellName);
                                }
                            }

                            if (listInPo.Count > 0)
                            {
                                int r = Random.Range(0, listInPo.Count);
                                StartCoroutine(Attack(listInPo[r]));
                            }
                            else if (listCanBeInPoFree.Count > 0)
                            {
                                list.Clear();
                                int r = Random.Range(0, listCanBeInPoFree.Count);
                                pathList = gridControl.pathfinding.FindPath(gridPos.x, gridPos.y, gridControl.currentX, gridControl.currentY);
                                for (int t = 0; t < pathList.Count - listCanBeInPoFree[r].po[1]; t++)
                                {
                                    list.Add(pathList[t]);
                                }
                                willBeInPo = true;
                                isOnFinalDestPoint = false;
                                gridControl.gridMap.Set(gridPos.x, gridPos.y, 0);
                            }
                            else if (listCanBeInPoLine.Count > 0)
                            {
                                list.Clear();

                                for (int t = 0; t < pathListLine.Count; t++)
                                {
                                    list.Add(pathListLine[t]);
                                }
                                willBeInPo = true;
                                isOnFinalDestPoint = false;
                                gridControl.gridMap.Set(gridPos.x, gridPos.y, 0);
                            }
                        }
                    }
                }
            }

            if (!willBeInPo && inFight && myTurn)
            {
                if (gridControl.pathfinding.FindPath(gridPos.x, gridPos.y, gridControl.currentX, gridControl.currentY).Count > 1)
                {
                    inCacPlayer = false;
                }

                switch (typeIA)
                {
                    case TypeIA.Close:
                        if (isOnFinalDestPoint && !inMove && pm > 0 && !inAttack && !inCacPlayer)
                        {
                            pathList = gridControl.pathfinding.FindPath(gridPos.x, gridPos.y, gridControl.currentX, gridControl.currentY);
                            if (pathList.Count > 0 && !inMove)
                            {
                                Debug.Log("pathListCount :" + pathList.Count);
                                Debug.Log("pm:" + pm);
                                list.Clear();
                                if (pathList.Count == pm)
                                {
                                    if (new Vector2Int(gridControl.currentX, gridControl.currentY) == new Vector2Int(pathList[pm - 1].xPos, pathList[pm - 1].yPos))
                                    {
                                        for (int i = 0; i < pm - 1; i++)
                                        {
                                            list.Add(pathList[i]);
                                            //Debug.Log(new Vector2Int(list[i].xPos, list[i].yPos));
                                        }
                                        willBeInCac = true;
                                    }
                                    else
                                    {
                                        for (int i = 0; i < pm; i++)
                                        {
                                            list.Add(pathList[i]);
                                            //Debug.Log(new Vector2Int(list[i].xPos, list[i].yPos));
                                        }
                                        Debug.Log("== pm");
                                        inCacPlayer = false;
                                        willBeInCac = false;
                                    }
                                }
                                else if (pathList.Count < pm)
                                {
                                    for (int i = 0; i < pathList.Count - 1; i++)
                                    {
                                        list.Add(pathList[i]);
                                        //Debug.Log(new Vector2Int(list[i].xPos, list[i].yPos));
                                    }
                                    willBeInCac = true;
                                    Debug.Log("< pm");
                                }
                                else if (pathList.Count > pm + 1)
                                {
                                    for (int i = 0; i < pm; i++)
                                    {
                                        list.Add(pathList[i]);
                                        //Debug.Log(new Vector2Int(list[i].xPos, list[i].yPos));
                                    }
                                    Debug.Log("> pm inCacFalse");
                                    inCacPlayer = false;
                                    willBeInCac = false;
                                }
                                else
                                {
                                    for (int i = 0; i < pm; i++)
                                    {
                                        list.Add(pathList[i]);
                                        //Debug.Log(new Vector2Int(list[i].xPos, list[i].yPos));
                                    }
                                    Debug.Log("> pm");
                                    willBeInCac = true;
                                }

                                gridControl.gridMap.Set(gridPos.x, gridPos.y, 2);
                                isOnFinalDestPoint = false;
                            }
                            else if (myTurn && pathList.Count == 1 && !inMove && pa >= minCoutPA && !inAttack)
                            {
                                willBeInCac = true;
                                Debug.Log("attaque path == 1 ");
                                //StartCoroutine(Action());
                            }
                            else if (myTurn && !inAttack && !inMove && pa < minCoutPA)
                            {
                                myTurn = false;
                                FindObjectOfType<FightManager>().PassTurn(false);
                                Debug.Log("Pass Turn pa < minCout");
                            }
                        }
                        else if (myTurn && isOnFinalDestPoint && pm < 1 && !inCacPlayer && !inAttack)
                        {
                            myTurn = false;
                            FindObjectOfType<FightManager>().PassTurn(false);
                            Debug.Log("Pass Turn !inCacPlayer pm < 1");
                        }
                        else if (myTurn && isOnFinalDestPoint && pa >= minCoutPA && listInPo.Count == 0 && inCacPlayer && !inAttack)
                        {
                            myTurn = false;
                            FindObjectOfType<FightManager>().PassTurn(false);
                            Debug.Log("attaque pa >= minCoutPA && inCac && listPO == 0");
                        }
                        else if (myTurn && isOnFinalDestPoint && pa < minCoutPA && inCacPlayer && !inAttack)
                        {
                            Debug.Log("Pass Turn pa < minCoutPa");
                            myTurn = false;
                            FindObjectOfType<FightManager>().PassTurn(false);
                        }
                        break;
                    default:
                        Debug.Log("Wrong typeIA");
                        break;
                }
            }
        }
    }

    private void LoadEnemy(EnemyData _data)
    {
        // Supprime tous les child de l'empty s'il y en a 
        foreach (Transform child in transform)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        visuel_enemy = enemyData.visuel.GetComponent<SpriteRenderer>().sprite;

        // Fait apparaitre le modèle 3D de notre ennemi et le configure
        GameObject visuals = Instantiate(enemyData.visuel);
        visuals.transform.SetParent(transform);
        visuals.transform.localPosition = Vector3.zero;
        visuals.transform.rotation = Quaternion.identity;

        // Recupère les stats dans enemyData et l'attribue a l'enemie
        experience = _data.experience;
        name_enemy = _data.name_enemy;
        level_min = _data.level_min;
        level_max = _data.level_max;
        initiative = _data.initiative;
        speed = _data.speed;
        pdvMax = _data.pdvMax;
        pdv = pdvMax;
        paMax = _data.pa;
        pmMax = _data.pm;
        pa = _data.pa;
        pm = _data.pm;
        vitalite = _data.vitalite;
        sagesse = _data.sagesse;
        force = _data.force;
        intel = _data.intel;
        chance = _data.chance;
        agilite = _data.agilite;
        resiNeutre = _data.resiNeutre;
        resiTerre = _data.resiTerre;
        resiFeu = _data.resiFeu;
        resiEau = _data.resiEau;
        resiAir = _data.resiAir;
        typeIA = (TypeIA)_data.typeIA;
    }

    public void Move(List<PathNode> pathL)
    {
        if (!posSave)
        {
            curPos = new Vector2(transform.position.x, transform.position.y);

            if (destPoint > 0)
            {
                lastPath = new Vector2(pathL[destPoint - 1].xPos, pathL[destPoint - 1].yPos);
            }
            else
            {
                gridControl.gridMap.Set(gridPos.x, gridPos.y, 0);
                lastPath = new Vector2(gridPos.x, gridPos.y);
            }

            posSave = true;
            //Debug.Log("Save Done");
            //Debug.Log("lastPath :" + new Vector2(lastPath.x, lastPath.y));
            //Debug.Log("PathL[DestPoint] :" + new Vector2Int(pathL[destPoint].xPos, pathL[destPoint].yPos));
        }

        if (destPoint < pathL.Count && inFight)
        {
            inMove = true;
            if (lastPath.y < pathL[destPoint].yPos)
            {
                Vector2 posToMove = new Vector2(curPos.x - .45f, curPos.y + .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move : Up");
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.y > pathL[destPoint].yPos)
            {
                //spriteRender.flipX = true;
                Vector2 posToMove = new Vector2(curPos.x + .45f, curPos.y - .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move : Down");
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.x < pathL[destPoint].xPos)
            {
                Vector2 posToMove = new Vector2(curPos.x + .45f, curPos.y + .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move : Right");
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.x > pathL[destPoint].xPos)
            {
                //spriteRender.flipX = false;
                Vector2 posToMove = new Vector2(curPos.x - .45f, curPos.y - .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    // Debug.Log("Move : Left");
                    posSave = false;
                    destPoint++;
                }
            }
        }
        else
        {
            //gridControl.UpdateVisiblePosSprite();
            gridPos = new Vector2Int((int)lastPath.x, (int)lastPath.y);
            gridControl.gridMap.Set(gridPos.x, gridPos.y, 2);
            //Debug.Log("Player Pos end : " + new Vector2(transform.position.x, transform.position.y));
            pm -= pathL.Count;
            FindObjectOfType<GameManager>().GenerateTextFloating("-" + pathL.Count.ToString(), Color.green, transform.position);
            if (willBeInCac)
            {
                inCacPlayer = true;
                willBeInCac = false;
            }
            if (willBeInPo)
            {
                willBeInPo = false;
            }
            isOnFinalDestPoint = true;
            inMove = false;
            destPoint = 0;
        }
    }

    IEnumerator Attack(SpellData spell)
    {
        if (!inAttack)
        {
            inAttack = true;
            yield return new WaitForSeconds(1f);

            pa -= spell.pa;
            int jet = 0;

            FindObjectOfType<GameManager>().GenerateTextFloating("-" + spell.pa, Color.blue, transform.position);
            FindObjectOfType<GameManager>().SendMessageToChat(name_enemy + " lance " + spell.spellName + ".", Message.MessageType.info);

            foreach (SpellEffect effet in spell.effets)
            {
                switch (effet.type)
                {
                    case SpellEffect.TypeEffect.Damage:
                        jet = Random.Range(effet.jet[0], effet.jet[1]);
                        FindObjectOfType<FightManager>().player.GetDamage(jet);
                        FindObjectOfType<GameManager>().SendMessageToChat(FindObjectOfType<FightManager>().player.username + " perd " + jet.ToString() + " PDV.", Message.MessageType.info);
                        break;
                    default:
                        break;
                }
            }

            yield return new WaitForSeconds(1f);
            inAttack = false;
        }

    }

    private bool inPO(Vector2Int targetPos, SpellData spell)
    {
        if (spell.freeLdv)
        {
            Vector2Int distanceFree = new Vector2Int(Mathf.Abs(targetPos.x - gridPos.x), Mathf.Abs(targetPos.y - gridPos.y));

            int intDistFree = distanceFree.x + distanceFree.y;

            // Debug.Log("DistanceFree : " + distanceFree);
            // Debug.Log("IntDistanceFree : " + intDistFree);
            if (intDistFree >= spell.po[0] && intDistFree <= spell.po[1])
            {
                //Debug.Log(spell.spellName + " inPo");
                return true;
            }
            else
            {
                //Debug.Log(spell.spellName + " not inPo");
                return false;
            }
        }
        else
        {
            Vector2Int distanceLineOnly = new Vector2Int(Mathf.Abs(targetPos.x - gridPos.x), Mathf.Abs(targetPos.y - gridPos.y));

            if (distanceLineOnly.x == 0 || distanceLineOnly.y == 0)
            {
                if (distanceLineOnly.x > distanceLineOnly.y)
                {
                    if (distanceLineOnly.x >= spell.po[0] && distanceLineOnly.x <= spell.po[1])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (distanceLineOnly.y >= spell.po[0] && distanceLineOnly.y <= spell.po[1])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }

    private bool CanBeInPo(SpellData spell)
    {
        Vector2Int targetPos = new Vector2Int(gridControl.currentX, gridControl.currentY);
        Vector2Int distanceFree = new Vector2Int((targetPos.x - gridPos.x), (targetPos.y - gridPos.y));

        if (spell.freeLdv)
        {
            int intDistFree = Mathf.Abs(distanceFree.x) + Mathf.Abs(distanceFree.y);

            if (intDistFree - spell.po[1] <= pm)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (distanceFree.x <= spell.po[1] + pm || distanceFree.y <= spell.po[1] + pm)
            {
                if ((pm >= distanceFree.x || pm >= distanceFree.y))
                {
                    Vector2Int want = new Vector2Int(gridPos.x, gridPos.y);
                    if (Mathf.Abs(distanceFree.x) < Mathf.Abs(distanceFree.y))
                    {
                        want = new Vector2Int(gridPos.x + distanceFree.x, gridPos.y);
                    }
                    else
                    {
                        want = new Vector2Int(gridPos.x, gridPos.y + distanceFree.y);
                    }

                    if (gridControl.gridMap.CheckWalkable(want.x, want.y))
                    {
                        pathListLine.Clear();
                        pathListLine = gridControl.pathfinding.FindPath(gridPos.x, gridPos.y, want.x, want.y);
                        if (pathListLine.Count <= pm)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Debug.Log("Too far pm | DistFree :" + distanceFree);
                    return false;
                }
            }
            else
            {
                Debug.Log("Too far spellPo | DistFree :" + distanceFree);
                return false;
            }
        }
    }

    public void GetDamage(int damage)
    {
        if (damage >= pdv)
        {
            alive = false;
            pdv = 0;
        }
        else
        {
            pdv -= damage;
        }
    }

    public enum TypeIA
    {
        Close,
        Distance,
    }

    public enum TypeSpell
    {
        DirectDamage,
        Dot,
        Heal,
        Buff,
        Invoc
    }
}
