using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hiscoreText;

    [SerializeField] private GameObject wonPanel;
    [SerializeField] private GameObject lostPanel;
    
    public int score = 0;

    private GameBoard gameBoard;

    [SerializeField] private Hiscore hiscore;
    
    private void Start()
    {
        gameBoard = GameObject.Find("GameBoard").GetComponent<GameBoard>();
        gameBoard.onObtainScore += UpdateScore;
        gameBoard.onDeath += LoseScreen;
        gameBoard.onWin += WinScreen;
        hiscoreText.text = hiscore.hiscore.ToString();
    }

    private void UpdateScore(int value)
    {
        scoreText.text = (score += value).ToString();

        if (score > hiscore.hiscore)
        {
            hiscoreText.text = (hiscore.hiscore += value).ToString();
        }
    }

    private void LoseScreen()
    {
        lostPanel.SetActive(true);
    }
    private void WinScreen()
    {
        wonPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level1");
    }

    private void OnDestroy()
    {
        if(gameBoard == null)
            return;
        
        gameBoard.onObtainScore -= UpdateScore;
        gameBoard.onDeath -= LoseScreen;
        gameBoard.onWin -= WinScreen;
    }
}
