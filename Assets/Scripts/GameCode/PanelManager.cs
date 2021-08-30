using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public GameObject panel_A;
    public GameObject panel_B;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public GameObject saveWordButton;
    public GameObject generateBoardButton;

    void OnEnable()
    {
        GameManager.Instance.OnGameStart += StartGame;
        GameManager.Instance.OnGameOver += DispalyGameOverCallBack;
    }
    // Start is called before the first frame update
    void Start()
    {

        GameManager.Instance.OnGameStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameManager.GameState SetPanel(GameManager.GameState currentState)
    {
        switch (currentState)
        {
            case GameManager.GameState.PanelA:
                
                panel_A.SetActive(true);
                panel_B.SetActive(false);
                gameOverPanel.SetActive(false);
                saveWordButton.SetActive(true);
                generateBoardButton.SetActive(false);
                StopAllCoroutines();

                break;
            case GameManager.GameState.PanelB:
                panel_B.SetActive(true);
                panel_A.SetActive(false);
                gameOverPanel.SetActive(false);
                break;
            case GameManager.GameState.GameOver:
                gameOverPanel.SetActive(true);
                
                if (gameOverText != null)
                {
                    if (GameManager.Instance.hasWon)
                    {
                        gameOverText.text = "GOOD STUFF!!";
                    }
                    else
                        gameOverText.text = "OPTIONS";
                }
                
                break;
        }

        return currentState;
    }

    public void StartGame()
    {
        GameManager.Instance.currentState = SetPanel(GameManager.GameState.PanelA);
    }

    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GotoBoardCallBack()
    {
        GameManager.Instance.currentState = SetPanel(GameManager.GameState.PanelB);
    }

    public void OnGenerateBoardCallBack()
    {
        GameManager.Instance.OnGenerateBoard.Invoke();
    }

    public void DispalyGameOverCallBack()
    {
        StartCoroutine(DisplayGameOver());
    }

    public void DisplayOptionsMenuCallBack()
    {
        GameManager.Instance.currentState = SetPanel(GameManager.GameState.GameOver);
    }

    public void OnRestartCallback()
    {
        GameManager.Instance.OnRestartPressed.Invoke();

    }

    public IEnumerator DisplayGameOver()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.currentState = SetPanel(GameManager.GameState.GameOver);

    }



    void OnDisable()
    {
        if (GameManager.Instance != null)
        {

            GameManager.Instance.OnGameStart -= StartGame;
            GameManager.Instance.OnGameOver -= DispalyGameOverCallBack;
        }
    }

}
