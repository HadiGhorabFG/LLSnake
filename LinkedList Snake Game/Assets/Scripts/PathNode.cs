public class PathNode
{
    private PathNode[,] nodeGrid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(PathNode[,] grid, int x, int y)
    {
        this.nodeGrid = grid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
