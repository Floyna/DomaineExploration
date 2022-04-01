using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSpell : MonoBehaviour
{
    [SerializeField] GridControl gridControl;
    [SerializeField] public SpellData spell;
    private TestPlayer player;
    Image image;
    List<Vector2Int> availableCellList = new List<Vector2Int>();

    private void Start()
    {
        image = gameObject.GetComponent<Image>();

        if (spell != null && image.sprite.name == "UISprite")
        {
            image.sprite = spell.visuelSpell;
        }
        player = FindObjectOfType<TestPlayer>();
    }

    public void SetSpellInSlot(SpellData spellData)
    {
        this.spell = spellData;
        image.sprite = this.spell.visuelSpell;
    }

    public void PrepareSpell()
    {
        gridControl.tilemapHighlightSpell.ClearAllTiles();
        availableCellList.Clear();
        if (spell != null)
        {
            if (FindObjectOfType<TestPlayer>().inFight && FindObjectOfType<FightManager>().playerTurn)
            {
                if (spell != null && player.pa >= spell.pa)
                {
                    Debug.Log(spell.spellName);
                    List<Vector2Int> solidCell = new List<Vector2Int>();
                    FindObjectOfType<TestPlayer>().inPrepareSpell = true;
                    FindObjectOfType<TestPlayer>().currentSpell = spell;

                    for (int x = gridControl.currentX - spell.po[1]; x < gridControl.currentX + spell.po[1] + 1; x++)
                    {
                        for (int y = gridControl.currentY - spell.po[1]; y < gridControl.currentY + spell.po[1] + 1; y++)
                        {
                            if (x >= 0 && y >= 0)
                            {
                                if (gridControl.gridMap.CheckPosition(x, y) == true)
                                {
                                    if (gridControl.gridMap.CheckValueCell(x, y) == 0 || gridControl.gridMap.CheckValueCell(x, y) == 2)
                                    {
                                        if (x == gridControl.currentX && y == gridControl.currentY) { continue; }
                                        else
                                        {
                                            Vector2Int distance = new Vector2Int(x - gridControl.currentX, y - gridControl.currentY);
                                            if (((Mathf.Abs(distance.x) + Mathf.Abs(distance.y)) <= spell.po[1]) && ((Mathf.Abs(distance.x) + Mathf.Abs(distance.y)) >= spell.po[0]))
                                            {
                                                availableCellList.Add(new Vector2Int(x, y));
                                                gridControl.tilemapHighlightSpell.SetTile(new Vector3Int(x, y, 0), gridControl.tileSetMove.tiles[3]);
                                            }
                                        }
                                    }
                                    if (gridControl.gridMap.CheckValueCell(x, y) == 1 || gridControl.gridMap.CheckValueCell(x, y) == 2)
                                    {
                                        solidCell.Add(new Vector2Int(x, y));
                                    }
                                }
                            }

                        }
                    }
                    foreach (Vector2Int cell in solidCell)
                    {
                        for (int x = 1; x < availableCellList.Count; x++)
                        {
                            for (int y = 1; y < availableCellList.Count; y++)
                            {
                                if (Mathf.Abs(cell.x) > gridControl.currentX && Mathf.Abs(cell.y) > gridControl.currentY)
                                {
                                    //if (cell.x + x == cell.x + 1  && cell.y + y == cell.y + 1) { continue; }
                                    gridControl.tilemapHighlightSpell.SetTile(new Vector3Int(cell.x + x, cell.y + y, 0), gridControl.tileSet.tiles[4]);
                                }
                                else if (Mathf.Abs(cell.x) < gridControl.currentX && Mathf.Abs(cell.y) < gridControl.currentY)
                                {
                                    gridControl.tilemapHighlightSpell.SetTile(new Vector3Int(cell.x - x, cell.y - y, 0), gridControl.tileSet.tiles[4]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Unable to load spell");
                }
                FindObjectOfType<TestPlayer>().availableCellPos = availableCellList;
            }
        }
    }
}
