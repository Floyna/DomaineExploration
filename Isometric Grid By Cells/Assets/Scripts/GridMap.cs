using UnityEngine;

public class GridMap : MonoBehaviour
{
    public int height;
    public int length;

    int[,] grid;

    public void Init(int length, int height)
    {
        grid = new int[length, height];
        this.length = length;
        this.height = height;
    }

    public void Set(int x, int y, int to)
    {
        if (!CheckPosition(x, y))
        {
            Debug.LogWarning("Trying to set outside grid" + x.ToString() + "," + y.ToString());
            return;
        }
        grid[x, y] = to;
    }

    //0 = walkable, 1 = solid block,  2 = enemy, 3 = empty
    public int CheckValueCell(int x, int y)
    {
        if (CheckPosition(x, y))
        {
            return grid[x, y];
        }
        else
        {
            return -1;
        }
    }

    public int Get(int x, int y)
    {
        if (!CheckPosition(x, y))
        {
            Debug.LogWarning("Trying to get outside grid" + x.ToString() + "," + y.ToString());
            return -1;
        }
        return grid[x, y];
    }

    public bool CheckPosition(int x, int y)
    {
        if (x < 0 || x >= length)
        {
            return false;
        }

        if (y < 0 || y >= height)
        {
            return false;
        }

        return true;
    }

    internal bool CheckWalkable(int xPos, int yPos)
    {
        return grid[xPos, yPos] == 0;
    }
}
