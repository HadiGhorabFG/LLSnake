using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public delegate void OnMovePrevious();
    public OnMovePrevious onMovePrevious;
    
    public delegate void OnMoveCurrent(Vector3Int pos);
    public OnMoveCurrent onMoveCurrent;

    [SerializeField] private bool AI = true;
    [SerializeField] private bool player = false;
    
    [SerializeField] private float moveTimerValue = 0.5f;
    private float moveTimer = 0;

    private Vector3 directionVector;
    private Quaternion directionRot;

    private GameBoard gameBoard;
    
    private PathFinding pathFinding;

    private List<Vector3> foodPosList;
    
    private enum DirectionStates
    {
        Down, Up, Left, Right
    }
    private DirectionStates directionStates;

    private void Start()
    {
        gameBoard = GameObject.Find("GameBoard").GetComponent<GameBoard>();

        foodPosList = gameBoard.food.Keys.ToList();
        
        moveTimer = moveTimerValue;
        directionVector = transform.up;

        pathFinding = new PathFinding(gameBoard.gridSize.x, gameBoard.gridSize.y);
    }
    
    private void Update()
    {
        if(player) 
            MoveInDirection();
        else if(AI)
        {
            MoveInPath();
        }
    }
    
    private void MoveInPath()
    {
        if(moveTimer <= 0)
        {
            onMovePrevious.Invoke();
            
            List<PathNode> path = pathFinding.FindPath((int)transform.position.x, (int)transform.position.y,
                (int)foodPosList[0].x, (int)foodPosList[0].y);
            
            gameObject.transform.GetChild(0).transform.rotation = DirectionRot(path);
            
            if (path != null)
            {
                transform.position = new Vector3(path[1].x, path[1].y, 0);
            }

            onMoveCurrent.Invoke(Vector3Int.FloorToInt(transform.position));
            
            if (transform.position == new Vector3((int) foodPosList[0].x, (int) foodPosList[0].y, 0))
            {
                foodPosList.RemoveAt(0);
            }
            
            moveTimer = moveTimerValue;
        }

        moveTimer -= Time.deltaTime;    
    }

    private void MoveInDirection()
    {
        DirectionKeyPress();
            
        if(moveTimer <= 0)
        {
            onMovePrevious.Invoke();
            gameObject.transform.GetChild(0).transform.rotation = directionRot;
            transform.position += directionVector;
            onMoveCurrent.Invoke(Vector3Int.FloorToInt(transform.position));
            
            moveTimer = moveTimerValue;
        }

        moveTimer -= Time.deltaTime;
    }

    private void DirectionKeyPress()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            directionVector = transform.up;
            directionRot = Quaternion.Euler(0, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            directionVector = -transform.up;
            directionRot = Quaternion.Euler(0, 0, 180);
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            directionVector = -transform.right;
            directionRot = Quaternion.Euler(0, 0, 90);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            directionVector = transform.right;
            directionRot = Quaternion.Euler(0, 0, -90);
        }
    }
    
    private Quaternion DirectionRot(List<PathNode> path)
    {
        if (path[1].x > transform.position.x)
        {
            directionStates = DirectionStates.Right;
        }
        else if(path[1].x < transform.position.x)
        {
            directionStates = DirectionStates.Left;
        }
        else if(path[1].y > transform.position.y)
        {
            directionStates = DirectionStates.Up;
        }
        else if(path[1].y < transform.position.y)
        {
            directionStates = DirectionStates.Down;
        }
        
        switch (directionStates)
        {
            case DirectionStates.Up:
                directionRot = Quaternion.Euler(0, 0, 0);
                break;
            case DirectionStates.Down:
                directionRot = Quaternion.Euler(0, 0, 180);
                break;
            case DirectionStates.Left:
                directionRot = Quaternion.Euler(0, 0, 90);
                break;
            case DirectionStates.Right:
                directionRot = Quaternion.Euler(0, 0, -90);
                break;
        }
        
        return directionRot;
    }
}
