using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] PlayerStats playerStatsScript;
    [SerializeField] CutMan cutManScript;
    [SerializeField] GameManager gameManagerScript;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider playerHealthBar;
    [SerializeField] Slider cutManHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        // Set Player Health Bar From Start
        SetPlayerHealth();
        // Set Boss Health Bar
        SetBossHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerHealth();
        UpdateCutManHealth();
    }

    // Score UI
    public void UpdateScore()
    {
        scoreText.text = gameManagerScript.GetScore().ToString("D9");
    }

    // Player Healt UI
    void SetPlayerHealth()
    {
        playerHealthBar.maxValue = playerStatsScript.GetPlayerHealth();
        playerHealthBar.value = playerHealthBar.maxValue;
    }

    void UpdatePlayerHealth()
    {
        playerHealthBar.value = playerStatsScript.GetPlayerHealth();
    }

    // CutMan Health UI
    void SetBossHealth()
    {
        cutManHealthBar.maxValue = cutManScript.GetHealth();
        cutManHealthBar.value = cutManHealthBar.maxValue;
    }

    void UpdateCutManHealth()
    {
        cutManHealthBar.value = cutManScript.GetHealth();
    }
}
