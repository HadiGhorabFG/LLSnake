using System.Collections.Generic;
using ADT;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class GameBoard : MonoBehaviour
{
    public delegate void OnObtainScore(int score);
    public OnObtainScore onObtainScore;
    
    public delegate void OnDeath();
    public OnDeath onDeath;
    
    public delegate void OnWin();
    public OnWin onWin;

    public Dictionary<Vector3, GameObject> food;

    public LList<GameObject> snakeBody;
    public GameObject snakeHead;

    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private GameObject foodPrefab;

    [SerializeField] private int foodScore;
    
    public Vector2Int gridSize;

    public enum BoardPieces
    {
        Empty, Snake, Food
    }
    
    public BoardPieces[,] boardPieces;
    
    private void Awake()
    {   
        food = new Dictionary<Vector3, GameObject>();
        
        snakeBody = new LList<GameObject>();
        snakeBody.Add(GameObject.Find("SnakeHead"));
        snakeHead = snakeBody.headNode.value;

        snakeHead.GetComponent<Movement>().onMoveCurrent += SnakeHeadHit;
        
        boardPieces = new BoardPieces[gridSize.x, gridSize.y];
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                boardPieces[x, y] = BoardPieces.Empty;
            }
        }
        
        PlaceFood(7);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (boardPieces[x, y] == BoardPieces.Snake)
                    {
                        Debug.Log(new Vector2(x, y));
                    }
                }
            }
        }
    }

    private void SnakeHeadHit(Vector3Int pos)
    {
        switch(CheckBoardState(pos.x, pos.y))
        {
            case 1:
                AddSnakeBody(snakeBody.tailNode.value.transform.position);
                RemoveFood(pos);
                break;
            case 2:
                Death();
                break;
            case -1:
                break;
        }
    }

    public int CheckBoardState(int x, int y)
    {
        if (x < 0 || x > gridSize.x - 1 || y < 0 || y > gridSize.y - 1)
        {
            return 2;
        }
        
        if (boardPieces[x, y] == BoardPieces.Food)
        {
            return 1;
        }
        else if(boardPieces[x, y] == BoardPieces.Snake)
        {
            return 2;
        }

        return -1;
    }

    private void PlaceFood(int amount)
    {
        Random rdm = new Random();
        
        for (int i = 0; i < amount; i++)
        {
            boardPieces[rdm.Next(0, gridSize.x), rdm.Next(0, gridSize.y)] = BoardPieces.Food;
        }
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (boardPieces[x, y] == BoardPieces.Food)
                {
                    GameObject go = Instantiate(foodPrefab, new Vector3(x, y, 0), quaternion.identity);
                    food.Add(go.transform.position, go);
                }
            }
        }
    }

    private void RemoveFood(Vector3Int pos)
    {
        boardPieces[pos.x, pos.y] = BoardPieces.Empty;
        Destroy(food[pos]);
        food.Remove(pos);
        
        onObtainScore.Invoke(foodScore);

        if (food.Count == 0)
        {
            Time.timeScale = 0;       
            onWin.Invoke();
        }
    }
    
    public void AddSnakeBody(Vector3 pos)
    {
        GameObject go = Instantiate(snakeBodyPrefab, pos, Quaternion.identity);
        snakeBody.Add(go);
    }

    public void Death()
    {
        Time.timeScale = 0;
        onDeath.Invoke();
    }

    private void OnDestroy()
    {
        if(snakeHead == null)
            return;
        
        snakeHead.GetComponent<Movement>().onMoveCurrent -= SnakeHeadHit;
    }
}
