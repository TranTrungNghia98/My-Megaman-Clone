using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] PlayerStats playerStatsScript;
    [SerializeField] GameManager gameManagerScript;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider playerHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        // Set Player Health Bar From Start
        SetPlayerHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        UpdatePlayerHealth();
    }

    void UpdateScore()
    {
        scoreText.text = gameManagerScript.GetScore().ToString("D8");
    }

    void SetPlayerHealth()
    {
        playerHealthBar.maxValue = playerStatsScript.GetPlayerHealth();
        playerHealthBar.value = playerHealthBar.maxValue;
    }

    void UpdatePlayerHealth()
    {
        playerHealthBar.value = playerStatsScript.GetPlayerHealth();
    }
}
