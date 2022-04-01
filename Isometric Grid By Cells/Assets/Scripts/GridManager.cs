using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(GridMap))]
[RequireComponent(typeof(Tilemap))]
public class GridManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap tilemapRaise;
    [SerializeField] TileSet tileSet;

    GridMap grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridMap>();
        // grid.Init(grid.length, grid.height);
        //Set(1, 1, 2);
        //Set(1, 2, 2);
        //Set(1, 3, 2);
    }

    void UpdateTileMap()
    {
        for (int x = 0; x < grid.length; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                UpdateTile(x, y);
            }
        }
    }

    private void UpdateTile(int x, int y)
    {
        int tileID = grid.Get(x, y);
        if (tileID == -1)
        {
            return;
        }

        if (tileID == 2)
        {
            return;
        }

        tilemap.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[tileID]);
    }

    private void UpdateTileRaise(int x, int y)
    {

        int tileID = grid.Get(x, y);
        if (tileID == -1)
        {
            return;
        }

        tilemapRaise.SetTile(new Vector3Int(x, y, 0), tileSet.tiles[tileID]);

    }

    public void Set(int x, int y, int to)
    {
        grid.Set(x, y, to);

        if (to != 2)
        {
            UpdateTile(x, y);
        }
        else if (to == 2)
        {
            UpdateTileRaise(x, y);
        }
    }
}
