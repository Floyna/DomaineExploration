using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] public SpriteRenderer spriteRender;
    [SerializeField] GameObject floatingPoints;
    public string username;
    public bool alive;
    public bool inFight;
    public bool isOnFinalDestPoint;
    public bool waitForMove;
    public bool inPrepareSpell;
    public bool canMove;
    public float speed;

    public int level;
    public int experience;
    public int pdv;
    public int paMax;
    public int pa;
    public int pmMax;
    public int pm;
    public int initiative;


    [Header("Caracteristiques")]
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

    GridControl gridControl;
    private int destPoint = 0;
    private bool posSave;

    public Vector2 playerGridPos;
    public Vector2 curPos;
    public Vector2 lastPath;

    FightManager fightManager;

    [HideInInspector]
    public SpellData currentSpell;
    public List<Vector2Int> availableCellPos = new List<Vector2Int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && FindObjectOfType<Mouse>().wantFight)
        {
            StartCoroutine(StartFight(collision.GetComponent<EnemyControl>()));
        }
    }

    private void Start()
    {
        alive = true;
        inFight = false;
        waitForMove = false;
        gridControl = FindObjectOfType<GridControl>();
        fightManager = FindObjectOfType<FightManager>();
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!isOnFinalDestPoint && inFight && fightManager.inFight && fightManager.playerTurn)
        {
            Move(gridControl.pathListSave);
        }
        else if (!inFight && !isOnFinalDestPoint)
        {
            FreeMove(gridControl.pathListSave);
        }
    }

    public void LoadStuffEffects()
    {

    }

    public void FreeMove(List<PathNode> pathL)
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
                lastPath = new Vector2(playerGridPos.x, playerGridPos.y);
            }

            posSave = true;
        }

        if (destPoint < pathL.Count)
        {
            if (lastPath.x > pathL[destPoint].xPos && lastPath.y < pathL[destPoint].yPos)
            {
                Vector2 posToMove = new Vector2(curPos.x - .9f, curPos.y);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move :  Left Up");
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.x < pathL[destPoint].xPos && lastPath.y > pathL[destPoint].yPos)
            {
                Vector2 posToMove = new Vector2(curPos.x + .9f, curPos.y);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move :  Right Down");
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.x < pathL[destPoint].xPos && lastPath.y < pathL[destPoint].yPos)
            {
                Vector2 posToMove = new Vector2(curPos.x, curPos.y + .5f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move :  Up Right");
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.x > pathL[destPoint].xPos && lastPath.y > pathL[destPoint].yPos)
            {
                Vector2 posToMove = new Vector2(curPos.x, curPos.y - .5f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    // Debug.Log("Move :  Down Left");
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.y < pathL[destPoint].yPos)
            {
                Vector2 posToMove = new Vector2(curPos.x - .45f, curPos.y + .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    // Debug.Log("Move : Up");
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.y > pathL[destPoint].yPos)
            {
                spriteRender.flipX = true;
                Vector2 posToMove = new Vector2(curPos.x + .45f, curPos.y - .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    // Debug.Log("Move : Down");
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
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
                    gridControl.currentX = pathL[destPoint].xPos;
                    gridControl.currentY = pathL[destPoint].yPos;
                    posSave = false;
                    destPoint++;
                }
            }
            else if (lastPath.x > pathL[destPoint].xPos)
            {
                spriteRender.flipX = false;
                Vector2 posToMove = new Vector2(curPos.x - .45f, curPos.y - .25f);
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), posToMove, speed * Time.deltaTime);
                if (new Vector2(transform.position.x, transform.position.y) == posToMove)
                {
                    //Debug.Log("Move : Left");
                    posSave = false;
                    destPoint++;
                }
            }
        }
        else
        {
            gridControl.currentX = pathL[destPoint - 1].xPos;
            gridControl.currentY = pathL[destPoint - 1].yPos;
            // gridControl.UpdateVisiblePosSprite();
            playerGridPos = new Vector2(lastPath.x, lastPath.y);
            //Debug.Log("Player Pos end : " + new Vector2(transform.position.x, transform.position.y));
            isOnFinalDestPoint = true;
            destPoint = 0;
            StartCoroutine(WaitforMove());
        }
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
                lastPath = new Vector2(playerGridPos.x, playerGridPos.y);
            }

            posSave = true;
        }

        if (destPoint < pathL.Count)
        {
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
                spriteRender.flipX = true;
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
                spriteRender.flipX = false;
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
            gridControl.currentX = pathL[destPoint - 1].xPos;
            gridControl.currentY = pathL[destPoint - 1].yPos;
            GenerateTextFloating("-" + pathL.Count.ToString(), Color.green);
            //gridControl.UpdateVisiblePosSprite();
            playerGridPos = new Vector2(lastPath.x, lastPath.y);
            //Debug.Log("Player Pos end : " + new Vector2(transform.position.x, transform.position.y));
            isOnFinalDestPoint = true;
            destPoint = 0;
            StartCoroutine(WaitforMove());
        }
    }

    IEnumerator StartFight(EnemyControl enemy)
    {
        if (!inFight && !waitForMove)
        {
            yield return new WaitForSeconds(.3f);
            fightManager.InitialisePosEntite(this, enemy);
            curPos = transform.position;
            lastPath = new Vector2(gridControl.currentX, gridControl.currentY);
            isOnFinalDestPoint = true;
            inFight = true;
        }
    }

    IEnumerator WaitforMove()
    {
        if (!waitForMove)
        {
            waitForMove = true;
            yield return new WaitForSeconds(.3f);
            waitForMove = false;
        }

    }

    public void GetDamage(int damage)
    {
        GenerateTextFloating("-" + damage.ToString(), Color.red);

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

    private void GenerateTextFloating(string text, Color color)
    {
        GameObject txtFloating = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
        txtFloating.transform.GetChild(0).GetComponent<TextMeshPro>().text = text;
        txtFloating.transform.GetChild(0).GetComponent<TextMeshPro>().color = color;
    }
}
