using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] MainUI gameUI;
    [SerializeField] PlayerStats playerStatsScript;
    [SerializeField] CutMan cutManSctipt;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject youWin;

    private int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStatsScript.GetPlayerHealth() <= 0)
        {
            gameOver.SetActive(true);
        }

        else if (cutManSctipt.GetHealth() <= 0)
        {
            youWin.SetActive(true);
        }
    }

    // ABSTRACTION
    public int GetScore()
    {
        return score;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        gameUI.UpdateScore();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
    Application.Quit();
#endif
    }
}
