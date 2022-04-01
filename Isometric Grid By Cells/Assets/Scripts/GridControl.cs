using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridControl : MonoBehaviour
{
    [SerializeField] public Tilemap tilemapGround;
    [SerializeField] Tilemap tilemapDebugGrid;
    [SerializeField] public Tilemap targetTilemapHighlight;
    [SerializeField] public Tilemap tilemapHighlightSpell;
    [SerializeField] public Tilemap tilemapRaise;
    [SerializeField] public Tilemap tilemapRaiseSolide;
    [SerializeField] public Tilemap tilemapTactique;
    [SerializeField] public Tilemap tilemapTactiqueRaise;
    [SerializeField] public Tilemap tilemapGrid;
    [SerializeField] public Tilemap tilemapPropsGround;
    [SerializeField] public Tilemap tilemapPropsRaise;
    [SerializeField] GridManager gridManager;
    [SerializeField] public TileSet tileSetMove;
    [SerializeField] public TileSet tileSet;
    [SerializeField] public GridMap gridMap;
    [SerializeField] FightManager fightManager;
    public Pathfinding pathfinding;

    public BoundsInt boundsGround;
    public TileBase[] allTilesGround;

    BoundsInt boundsRaise;
    TileBase[] allTilesRaise;

    BoundsInt boundsRaiseSolide;
    TileBase[] allTilesRaiseSolide;

    public int currentX = 0;
    public int currentY = 0;
    public int targetPosX = 0;
    public int targetPosY = 0;
    public int pathInSaveX = 0;
    public int pathInSaveY = 0;

    Vector3 spawnPoint;

    TestPlayer player;
    List<PathNode> pathList = new List<PathNode>();
    public List<PathNode> pathListSave = new List<PathNode>();
    bool inSave;
    bool inLoad;
    bool inShowEffect;

    float cdShow;
    private void Start()
    {
        tilemapDebugGrid.gameObject.SetActive(false);
        tilemapGround.CompressBounds();
        tilemapRaise.CompressBounds();
        tilemapRaiseSolide.CompressBounds();
        tilemapGrid.ClearAllTiles();
        tilemapGrid.CompressBounds();
        tilemapTactique.ClearAllTiles();
        tilemapTactique.CompressBounds();
        tilemapTactiqueRaise.ClearAllTiles();
        tilemapTactiqueRaise.CompressBounds();
        //tilemapGround.ClearAllTiles();
        inLoad = true;
        //targetTilemapHighlight.SetTile(new Vector3Int(currentX, currentY, 0), tileSetMove.tiles[1]);
        player = FindObjectOfType<TestPlayer>();
        player.isOnFinalDestPoint = true;
        /*
        currentX = Mathf.RoundToInt(spawnPoint.x);
        currentY = Mathf.RoundToInt(spawnPoint.y);
        player.playerGridPos = new Vector2(currentX, currentY);
        */

        boundsGround = tilemapGround.cellBounds;
        allTilesGround = tilemapGround.GetTilesBlock(boundsGround);

        boundsRaise = tilemapRaise.cellBounds;
        allTilesRaise = tilemapRaise.GetTilesBlock(boundsRaise);

        boundsRaiseSolide = tilemapRaiseSolide.cellBounds;
        allTilesRaiseSolide = tilemapRaiseSolide.GetTilesBlock(boundsRaiseSolide);

        if (boundsRaise.xMax >= boundsGround.xMax && boundsRaise.yMax >= boundsGround.yMax)
        {
            gridMap.Init(boundsRaise.xMax, boundsRaise.yMax);
        }
        else if (boundsRaise.xMax < boundsGround.xMax && boundsRaise.yMax < boundsGround.yMax)
        {
            gridMap.Init(boundsGround.xMax, boundsGround.yMax);
        }
        else if (boundsRaise.xMax >= boundsGround.xMax && boundsRaise.yMax < boundsGround.yMax)
        {
            gridMap.Init(boundsRaise.xMax, boundsGround.yMax);
        }
        else if (boundsRaise.xMax < boundsGround.xMax && boundsRaise.yMax >= boundsGround.yMax)
        {
            gridMap.Init(boundsGround.xMax, boundsRaise.yMax);
        }

        // Debug.Log("Ground x:" + boundsGround.xMax + " y:" + boundsGround.yMax +
        //  "\nRaise x:" + boundsRaise.xMax + " y:" + boundsRaise.yMax);
        //tilemapGround.transform.position = new Vector3(min_x, min_y, 0);
        // tilemapGround.ClearAllTiles();

        //int count = 0;
        for (int x = 0; x < gridMap.length; x++)
        {
            for (int y = 0; y < gridMap.height; y++)
            {
                //tilemapGrid.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[3]);
                gridMap.Set(x, y, 3);
            }
        }

        /*
        for (int x = 0; x < boundsGround.size.x; x++)
        {
            for (int y = 0; y < boundsGround.size.y; y++)
            {
                try
                {
                    TileBase tile = allTilesGround[x + y * boundsGround.size.x];
                    if (tile != null)
                    {
                        tilemapGrid.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[3]);
                        gridMap.Set(x, y, 0);
                    }
                }
                catch (System.Exception) { }
            }
        }

        for (int x = 0; x < boundsRaise.size.x; x++)
        {
            for (int y = 0; y < boundsRaise.size.y; y++)
            {
                try
                {
                    TileBase tile1 = allTilesRaise[x + y * boundsRaise.size.x];
                    if (tile1 != null)
                    {
                        tilemapGrid.SetTile(new Vector3Int(x + boundsRaise.min.x, y + boundsRaise.min.y, 0), tileSet.tiles[3]);
                        gridMap.Set(x + boundsRaise.min.x, y + boundsRaise.min.y, 0);
                    }
                }
                catch (System.Exception) { }
            }
        }
        */

        ConvertTilesInGridPos(boundsGround, allTilesGround, 0);
        ConvertTilesInGridPos(boundsRaise, allTilesRaise, 1);
        ConvertTilesInGridPos(boundsRaiseSolide, allTilesRaiseSolide, 2);


        /*
        foreach (var posWalkable in walkable)
        {
            foreach (var posNotWalkable in notWalkable)
            {
                if (!notWalkable.Contains(posWalkable))
                {
                    gridMap.Set((int)posNotWalkable.x, (int)posNotWalkable.y, 2);
                    tilemapGrid.SetTile(new Vector3Int((int)posNotWalkable.x, (int)posNotWalkable.y, 0), tileSet.tiles[3]);
                }
            }
        }
        */

        /*
        for (int x = 0; x < gridMap.length; x++)
        {
            for (int y = 0; y < gridMap.height; y++)
            {

            }
        }
        */
        /*
        for (int x = 0; x < bounds.xMax; x++)
        {
            for (int y = 0; y < bounds.yMax; y++)
            {
                tilemapGrid.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[3]);
            }
        }
        */

        FindObjectOfType<FightManager>().InitialisePosCell();

        //  FindObjectOfType<FightManager>().InitialisePosEntite(player, FindObjectOfType<EnemyControl>());
        pathfinding = gridManager.GetComponent<Pathfinding>();
        pathfinding.Init();
        inLoad = false;
    }

    public void ConvertTilesInGridPos(BoundsInt bounds, TileBase[] allTiles, int elevation)
    {
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                try
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null && elevation == 0)
                    {
                        tilemapDebugGrid.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[3]);
                        tilemapGrid.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[5]);
                        tilemapTactique.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[6]);
                        gridMap.Set(x, y, 0);
                    }
                    else if (tile != null && elevation == 1)
                    {
                        tilemapDebugGrid.SetTile(new Vector3Int(x + boundsRaise.min.x, y + boundsRaise.min.y, 0), tileSet.tiles[3]);
                        tilemapGrid.SetTile(new Vector3Int(x + boundsRaise.min.x, y + boundsRaise.min.y, 0), tileSet.tiles[5]);
                        tilemapTactique.SetTile(new Vector3Int(x + boundsRaise.min.x, y + boundsRaise.min.y, 0), tileSet.tiles[6]);
                        gridMap.Set(x + boundsRaise.min.x, y + boundsRaise.min.y, 0);
                    }
                    else if (tile != null && elevation == 2)
                    {
                        //tilemapGrid.SetTile(new Vector3Int(x + boundsRaise.min.x, y + boundsRaise.min.y, 0), tileSet.tiles[5]);
                        tilemapTactiqueRaise.SetTile(new Vector3Int(x + boundsRaise.min.x + 2, y + boundsRaise.min.y + 1, 0), tileSet.tiles[8]);
                        gridMap.Set(x + boundsRaise.min.x + 2, y + boundsRaise.min.y + 1, 1);
                    }
                    else
                    {
                        //tilemapTactique.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[7]);
                    }
                }
                catch (System.Exception) { }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MouseInput();

        if (!inLoad)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickPosition = targetTilemapHighlight.WorldToCell(worldPoint);

            targetPosX = clickPosition.x - 5;
            targetPosY = clickPosition.y - 5;

            if (targetPosX != pathInSaveX || targetPosY != pathInSaveY)
            {
                if (gridMap.CheckPosition(targetPosX, targetPosY))
                {
                    if (gridMap.CheckWalkable(targetPosX, targetPosY) && player.isOnFinalDestPoint)
                    {
                        targetTilemapHighlight.ClearAllTiles();
                        if (player.inFight && fightManager.playerTurn && !player.inPrepareSpell)
                        {
                            StartCoroutine(SavePOS(targetPosX, targetPosY));
                            pathList = pathfinding.FindPath(currentX, currentY, targetPosX, targetPosY);

                            if (pathList != null && pathList.Count <= player.pm && !fightManager.inLoad)
                            {
                                for (int i = 0; i < pathList.Count; i++)
                                {
                                    targetTilemapHighlight.SetTile(new Vector3Int(pathList[i].xPos, pathList[i].yPos, 0), tileSetMove.tiles[2]);
                                }
                            }
                        }
                        else if (player.inFight && !fightManager.playerTurn)
                        {
                            targetTilemapHighlight.ClearAllTiles();
                        }
                        else if (player.alive)
                        {
                            StartCoroutine(SavePOS(targetPosX, targetPosY));
                            pathList = pathfinding.FindPath(currentX, currentY, targetPosX, targetPosY);
                        }
                    }
                    else
                    {
                        targetTilemapHighlight.ClearAllTiles();
                    }
                }
            }
        }

        if(player.inFight && player.inPrepareSpell)
        {
            if (player.availableCellPos.Contains(new Vector2Int(targetPosX, targetPosY)))
            {
                targetTilemapHighlight.SetTile(new Vector3Int(targetPosX, targetPosY, 0), tileSetMove.tiles[2]);
            }
            else
            {
                targetTilemapHighlight.ClearAllTiles();
            }
        }

        if (Input.GetMouseButtonDown(0) && gridMap.CheckPosition(targetPosX, targetPosY) && gridMap.CheckWalkable(targetPosX, targetPosY) && player.isOnFinalDestPoint && !player.waitForMove && player.playerGridPos != new Vector2(targetPosX, targetPosY) && !player.inPrepareSpell && player.canMove)
        {
            UpdatePos();
        }

        if (Input.GetMouseButtonDown(0) && player.inFight && fightManager.playerTurn)
        {
            if (player.inPrepareSpell)
            {
                if (player.availableCellPos.Contains(new Vector2Int(targetPosX, targetPosY)))
                {
                    if (player.pa >= player.currentSpell.pa)
                    {
                        player.pa -= player.currentSpell.pa;
                        int jet = 0;
                        targetTilemapHighlight.ClearAllTiles();
                        FindObjectOfType<GameManager>().GenerateTextFloating("-" + player.currentSpell.pa, Color.blue, player.transform.position);
                        FindObjectOfType<GameManager>().SendMessageToChat(player.username + " lance " + player.currentSpell.spellName + ".", Message.MessageType.info);

                        switch (gridMap.CheckValueCell(targetPosX, targetPosY))
                        {
                            case 0:
                                break;
                            case 2:
                                foreach (var enemy in FindObjectsOfType<EnemyControl>())
                                {
                                    if (enemy.gridPos == new Vector2Int(targetPosX, targetPosY))
                                    {
                                        foreach (SpellEffect effet in player.currentSpell.effets)
                                        {
                                            switch (effet.type)
                                            {
                                                case SpellEffect.TypeEffect.Damage:
                                                    jet = Random.Range(effet.jet[0], effet.jet[1]);
                                                    enemy.GetDamage(jet);
                                                    FindObjectOfType<GameManager>().SendMessageToChat(enemy.name_enemy + " perd " + jet.ToString() + " PDV.", Message.MessageType.info);
                                                    StartCoroutine(ShowEffect(0f, "-", jet, Color.red, enemy.transform.position));
                                                    break;
                                                case SpellEffect.TypeEffect.RetraitPA:
                                                    jet = effet.jet[0];
                                                    enemy.pa -= jet;
                                                    FindObjectOfType<GameManager>().SendMessageToChat(enemy.name_enemy + " perd " + jet.ToString() + " PA.", Message.MessageType.info);
                                                    StartCoroutine(ShowEffect(.3f, "-", jet, Color.blue, enemy.transform.position));
                                                    break;
                                                case SpellEffect.TypeEffect.RetraitPM:
                                                    float cd = .3f;
                                                    if (player.currentSpell.effets.Length >= 3)
                                                    {
                                                        cd = .6f;
                                                    }
                                                    jet = effet.jet[0];
                                                    enemy.pm -= jet;
                                                    FindObjectOfType<GameManager>().SendMessageToChat(enemy.name_enemy + " perd " + jet.ToString() + " PM.", Message.MessageType.info);
                                                    StartCoroutine(ShowEffect(cd, "-", jet, Color.green, enemy.transform.position));
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            tilemapHighlightSpell.ClearAllTiles();
            player.inPrepareSpell = false;
        }

        if (Input.GetMouseButtonDown(2))
        {
            //Debug.Log(pathList.Count);

            if (gridMap.CheckPosition(targetPosX, targetPosY))
            {
                Debug.Log("x : " + targetPosX + " y :" + targetPosY +
                      "\nCheckWalkable : " + gridMap.CheckWalkable(targetPosX, targetPosY));
                switch (gridMap.CheckValueCell(targetPosX, targetPosY))
                {
                    case 0:
                        Debug.Log("Walkable");
                        break;
                    case 1:
                        Debug.Log("Solid block");
                        break;
                    case 2:
                        Debug.Log("Enemy");
                        break;
                    case 3:
                        Debug.Log("Empty");
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator ShowEffect(float cooldown, string text, int jet, Color couleur, Vector3 position)
    {
        yield return new WaitForSeconds(cooldown);
        FindObjectOfType<GameManager>().GenerateTextFloating(text + jet, couleur, position);
    }

    IEnumerator SavePOS(int x, int y)
    {
        if (!inSave)
        {
            inSave = true;
            yield return new WaitForSeconds(.1f);
            pathInSaveX = x;
            pathInSaveY = y;
            inSave = false;
        }
    }

    private void UpdatePos()
    {
        targetTilemapHighlight.ClearAllTiles();
        //targetTilemapHighlight.SetTile(new Vector3Int(currentX, currentY, 0), tileSetMove.tiles[1]);
        pathListSave.Clear();
        if (pathList != null)
        {
            if (player.inFight && pathList.Count <= player.pm && !fightManager.inLoad)
            {
                for (int i = 0; i < pathList.Count; i++)
                {
                    pathListSave.Add(pathList[i]);
                }
                player.isOnFinalDestPoint = false;
                player.pm -= pathList.Count;
            }
            else if (!player.inFight && player.alive)
            {
                for (int i = 0; i < pathList.Count; i++)
                {
                    //Debug.Log("Pos path " + i + " : " + new Vector2(pathList[i].xPos, pathList[i].yPos));
                    //Debug.Log("Player Pos Start: " + new Vector2(player.transform.position.x, player.transform.position.y));
                    pathListSave.Add(pathList[i]);
                }
                player.isOnFinalDestPoint = false;
            }
            // Debug.Log("Clic");
        }

        //CheckWall();
    }

    private void CheckWall()
    {
        // Check no walkable for disable view
        for (int x = 0; x < gridMap.length; x++)
        {
            for (int y = 0; y < gridMap.height; y++)
            {
                if (!gridMap.CheckWalkable(x, y) && (currentX - x) == 1 && (currentY == y))
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[4]);
                    /*
                    Debug.Log("HideCurrentX(" + currentX.ToString() + ")" +
                              "\nCurrentY(" + currentY.ToString() + ")" +
                              "\nX(" + x.ToString() + ")" +
                              "\nY(" + y.ToString() + ")" +
                              "\nCalculeCurX-X(" + (currentX - x).ToString() +
                              "\nCalculeCurY-Y(" + (currentY - y).ToString() + ")");
                    */
                    //Debug.Log("Hide no.1 - xPlayerSuperieur");
                }
                else if (!gridMap.CheckWalkable(x, y) && (currentX - x) == 1 && (currentY - y) == 1)
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[4]);
                    // Debug.Log("Hide no.2 - yPlayerSuperieur");
                }
                else if (!gridMap.CheckWalkable(x, y) && (currentX - x) == 0 && (currentY - y) == 1)
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[4]);
                }
                else if (!gridMap.CheckWalkable(x, y) && ((currentX - x) > 0))
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[2]);
                }
                else if (!gridMap.CheckWalkable(x, y) && ((currentY - y) > 0))
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[2]);
                }
                else if (!gridMap.CheckWalkable(x, y) && ((currentX - x) < 0))
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[2]);
                }
                else if (!gridMap.CheckWalkable(x, y) && ((currentY - y) < 0))
                {
                    tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[2]);
                }
            }
        }
    }

    public void UpdateVisiblePosSprite()
    {
        targetTilemapHighlight.SetTile(new Vector3Int(currentX, currentY, 0), tileSetMove.tiles[1]);
    }
}
