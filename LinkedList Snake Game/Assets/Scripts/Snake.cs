using ADT;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector3Int currentPos;
    private GameBoard gameBoard;
    
    public Vector3 CurrentPos
    {
        get { return currentPos; }
    }

    private void Start()
    {
        gameBoard = GameObject.Find("GameBoard").GetComponent<GameBoard>();
        gameBoard.snakeHead.GetComponent<Movement>().onMoveCurrent += UpdateSnakeState;
        gameBoard.snakeHead.GetComponent<Movement>().onMovePrevious += UpdatePosition;
    }

    private void UpdatePosition()
    {
        currentPos = Vector3Int.FloorToInt(this.gameObject.transform.position);
        LLNode<GameObject> myNode = gameBoard.snakeBody.GetNodeOnItem(this.gameObject);

        if(myNode.previousNode != null)
        {
            transform.position = myNode.previousNode.value.gameObject.GetComponent<Snake>().CurrentPos;
        }
    }

    private void UpdateSnakeState(Vector3Int pos)
    {
        if (pos.x < 0 || pos.x > gameBoard.gridSize.x - 1 || pos.y < 0 || pos.y > gameBoard.gridSize.y - 1)
        {;
            return;
        }
        
        //works atm but could be saving the tail point a block b4
        gameBoard.boardPieces[(int)transform.position.x, (int)transform.position.y] = GameBoard.BoardPieces.Snake;
        gameBoard.boardPieces[currentPos.x, currentPos.y] = GameBoard.BoardPieces.Empty;
    }

    private void OnDestroy()
    {
        if(gameBoard == null)
            return;
        
        gameBoard.snakeHead.GetComponent<Movement>().onMoveCurrent -= UpdateSnakeState;
        gameBoard.snakeHead.GetComponent<Movement>().onMovePrevious -= UpdatePosition;
    }
}
