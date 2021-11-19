using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    public int x;
    public int y;
    
    private PathFinding pathFinding;
    public GameObject testPrefab;
    
    private void Start()
    {
        pathFinding = new PathFinding(10, 10);

        List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
        Debug.Log(path.Count);
        
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Instantiate(testPrefab, new Vector3(path[i].x, path[i].y, 0), Quaternion.identity);
            }
        }
    }
}
