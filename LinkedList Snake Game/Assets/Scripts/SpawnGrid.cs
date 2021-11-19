using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    [SerializeField] private GameObject tileGO;

    private GameBoard gameBoard;
    
    private void Start()
    {
        gameBoard = gameObject.GetComponent<GameBoard>();
        
        for (int x = 0; x < gameBoard.gridSize.x; x++)
        {
            for (int y = 0; y < gameBoard.gridSize.y; y++)
            {
                SpawnGridTiles(x, y, 0, 0, 0);           
            }
        }
    }
    
    private void SpawnGridTiles(float x, float y, float z, int rotX, int rotY)
    {
        GameObject go = Instantiate(tileGO, new Vector3(x, y, z), Quaternion.Euler(rotX, rotY,0));
        go.gameObject.transform.parent = this.gameObject.transform;
    }
}
