using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    [SerializeField] public Tilemap tilemapFight;
    [SerializeField] GridControl gridControl;
    [SerializeField] GameObject panelReady;
    [SerializeField] GameObject panelPasse;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject panelTimeLine;
    [SerializeField] GameObject contentTimeLine;
    [SerializeField] GameObject prefTimeLine;
    public List<EnemyControl> listEnemy = new List<EnemyControl>();
    public List<Vector2> listPosCellPlayer = new List<Vector2>();
    public List<Vector2> listPosCellEnemy = new List<Vector2>();
    public List<GameObject> listEntite = new List<GameObject>();
    public TestPlayer player;

    private Vector2Int mouseGridPos = new Vector2Int();

    BoundsInt boundsFight;
    TileBase[] allTilesFight;

    int timeLinePos;
    //int tour = 0;

    public bool inFight;
    public bool inLoad;
    public bool playerTurn;
    public bool EnemeyTurn;

    private void Update()
    {
        mouseGridPos = new Vector2Int(gridControl.targetPosX, gridControl.targetPosY);

        if (player == null)
        {
            player = FindObjectOfType<TestPlayer>();
        }

        if (inFight)
        {
            if (playerTurn)
            {
                if (!panelPasse.activeInHierarchy)
                {
                    panelPasse.SetActive(true);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && inLoad && listPosCellPlayer.Contains(mouseGridPos))
        {
            Vector2 originGridPos = mouseGridPos;
            Vector2 posCellPlayer = ConvertGridPosToTransformPos(originGridPos);

            player.transform.position = new Vector3(posCellPlayer.x, posCellPlayer.y);
            gridControl.currentX = (int)originGridPos.x;
            gridControl.currentY = (int)originGridPos.y;
            player.playerGridPos = originGridPos;
            player.curPos = player.transform.position;
            player.lastPath = originGridPos;
        }

        if (contentTimeLine.transform.childCount > 7)
        {
            if (contentTimeLine.GetComponent<ContentSizeFitter>().horizontalFit == ContentSizeFitter.FitMode.Unconstrained)
            {
                contentTimeLine.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
        }
        else
        {
            if (contentTimeLine.GetComponent<ContentSizeFitter>().horizontalFit == ContentSizeFitter.FitMode.PreferredSize)
            {
                contentTimeLine.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }
        }
    }

    public void InitialisePosCell()
    {
        tilemapFight.CompressBounds();

        boundsFight = tilemapFight.cellBounds;
        allTilesFight = tilemapFight.GetTilesBlock(boundsFight);

        for (int x = 0; x < boundsFight.size.x; x++)
        {
            for (int y = 0; y < boundsFight.size.y; y++)
            {
                TileBase tile = allTilesFight[x + y * boundsFight.size.x];
                if (tile != null)
                {
                    if (tile.name == "BlueCell")
                    {
                        listPosCellPlayer.Add(new Vector2(x + boundsFight.xMin, y + boundsFight.yMin));
                    }
                    else if (tile.name == "RedCell")
                    {
                        listPosCellEnemy.Add(new Vector2(x + boundsFight.xMin, y + boundsFight.yMin));
                    }
                }
            }
        }
    }

    void CheckTurnAndPlay(GameObject entite)
    {
        if (entite.tag == "Player")
        {
            EnemeyTurn = false;
            playerTurn = true;
            panelPasse.SetActive(true);
            Debug.Log("Player Turn");
        }
        else if (entite.tag == "Enemy")
        {
            playerTurn = false;
            EnemyControl enemy = entite.GetComponent<EnemyControl>();
            enemy.myTurn = true;
            Debug.Log("Enemy Turn");
        }
    }

    Vector2 ConvertGridPosToTransformPos(Vector2 posCell)
    {
        float bonusX = 0;
        float bonusY = 0;

        Vector2 realPos = new Vector2();

        if (posCell.x == posCell.y)
        {
            bonusX = 0f;
            bonusY = .45f;

            realPos = new Vector2(0, (posCell.y / 2) + bonusY);

            //  Debug.Log(" == ");
        }
        else if (posCell.x > posCell.y)
        {
            // Debug.Log("x > y");
            bonusX = .45f;
            bonusY = .25f;
            realPos = new Vector2((posCell.x - posCell.y) * bonusX, ((posCell.x + posCell.y) * bonusY) + bonusX);
        }
        else if (posCell.x < posCell.y)
        {
            //  Debug.Log("x < y");
            bonusX = -.45f;
            bonusY = .25f;
            realPos = new Vector2((posCell.y - posCell.x) * bonusX, ((posCell.x + posCell.y) * bonusY) + .45f);
        }

        return realPos;
    }

    public void InitialisePosEntite(TestPlayer player, EnemyControl enemy)
    {
        timeLinePos = 0;
        //tour = 0;

        listEntite.Add(player.gameObject);
        listEntite.Add(enemy.gameObject);
        /*
        foreach (GameObject item in enemy.GetComponent<Groupe_Enemy>().listEnemy)
        {
            listEntite.Add(item);
        }
        */
        tilemapFight.gameObject.SetActive(true);
        gridControl.tilemapGrid.gameObject.SetActive(true);
        gridControl.tilemapTactique.gameObject.SetActive(true);
        gridControl.tilemapTactiqueRaise.gameObject.SetActive(true);
        gridControl.tilemapPropsGround.gameObject.SetActive(false);
        gridControl.tilemapPropsRaise.gameObject.SetActive(false);
        gridControl.tilemapGround.gameObject.SetActive(false);
        gridControl.tilemapRaise.gameObject.SetActive(false);
        gridControl.tilemapRaiseSolide.gameObject.SetActive(false);

        inLoad = true;
        panelReady.SetActive(true);
        int randBlue = Random.Range(0, listPosCellPlayer.Count);
        int randRed = Random.Range(0, listPosCellEnemy.Count);

        Vector2 originGridPos = listPosCellPlayer[randBlue];
        Vector2 originGridPosEnemy = listPosCellEnemy[randRed];
        Vector2 posCellPlayer = ConvertGridPosToTransformPos(originGridPos);
        Vector2 posCellEnemy = ConvertGridPosToTransformPos(originGridPosEnemy);

        listEnemy.Add(enemy);

        if (player != null)
        {
            player.transform.position = new Vector3(posCellPlayer.x, posCellPlayer.y);
            gridControl.currentX = (int)originGridPos.x;
            gridControl.currentY = (int)originGridPos.y;
            player.playerGridPos = originGridPos;
            // Debug.Log("postFightPlayer :" + new Vector2(originGridPos.x, originGridPos.y));
            //Debug.Log("PosPlayer :" + player.transform.position);
        }

        if (listEnemy.Count > 0)
        {
            listEnemy[0].transform.position = new Vector3(posCellEnemy.x, posCellEnemy.y);
            gridControl.gridMap.Set((int)originGridPosEnemy.x, (int)originGridPosEnemy.y, 2);
            listEnemy[0].gridPos = new Vector2Int((int)originGridPosEnemy.x, (int)originGridPosEnemy.y);
            listEnemy[0].curPos = listEnemy[0].transform.position;
            listEnemy[0].lastPath = new Vector2(originGridPosEnemy.x, originGridPosEnemy.y);
        }

        /*
        foreach (var item in collection)
        {

        }
        */
        inFight = true;
    }

    public void Ready()
    {
        panelReady.SetActive(false);
        tilemapFight.gameObject.SetActive(false);
        panelPasse.SetActive(true);
        panelTimeLine.SetActive(true);

        List<int> listInitiative = new List<int>();

        for (int i = 0; i < listEntite.Count; i++)
        {
            if (listEntite[i].transform.tag == "Player")
            {
                listInitiative.Add(listEntite[i].GetComponent<TestPlayer>().initiative);
            }
            else
            {
                listInitiative.Add(listEntite[i].GetComponent<EnemyControl>().initiative);
            }
        }

        listInitiative.Sort();

        for (int i = 0; i < listEntite.Count; i++)
        {
            GameObject entiteTimeLine = Instantiate(prefTimeLine, contentTimeLine.transform);

            if (listEntite[i].transform.tag == "Player")
            {
                entiteTimeLine.GetComponent<LinksTimePref>().imageEntite.sprite = listEntite[i].GetComponent<TestPlayer>().spriteRender.sprite;
                entiteTimeLine.GetComponent<LinksTimePref>().SliderImg.color = Color.red;
            }
            else
            {
                entiteTimeLine.GetComponent<LinksTimePref>().imageEntite.sprite = listEntite[i].GetComponent<EnemyControl>().visuel_enemy;
                entiteTimeLine.GetComponent<LinksTimePref>().SliderImg.color = Color.blue;
            }
            
        }

        if (player.initiative > listEnemy[0].initiative)
        {
            playerTurn = true;
            EnemeyTurn = false;
        }
        else if (player.initiative < listEnemy[0].initiative)
        {
            playerTurn = false;
            EnemeyTurn = true;
        }
        else if (player.initiative == listEnemy[0].initiative)
        {
            if (Random.Range(0, 2) == 0)
            {
                playerTurn = true;
                EnemeyTurn = false;
            }
            else
            {
                playerTurn = false;
                EnemeyTurn = true;
            }
        }

        for (int i = 0; i < listEntite.Count; i++)
        {
            if (listEntite[i].transform.tag == "Player")
            {
                listEntite[i].GetComponent<TestPlayer>().inFight = true;
            }
            else
            {
                listEntite[i].GetComponent<EnemyControl>().inFight = true;
            }
        }

        gameManager.SendMessageToChat("Debut du combat.", Message.MessageType.info);
        inLoad = false;
    }

    public void PassTurn(bool isPlayer)
    {
        if (isPlayer)
        {
            panelPasse.SetActive(false);
            player.pa = player.paMax;
            player.pm = player.pmMax;
        }
        else
        {
            EnemyControl enemy = listEntite[timeLinePos].GetComponent<EnemyControl>();
            enemy.pa = enemy.paMax;
            enemy.pm = enemy.pmMax;
        }

        timeLinePos++;

        if (timeLinePos < listEntite.Count)
        {
            CheckTurnAndPlay(listEntite[timeLinePos]);
        }
        else
        {
            timeLinePos = 0;
            CheckTurnAndPlay(listEntite[timeLinePos]);
        }
    }
}
