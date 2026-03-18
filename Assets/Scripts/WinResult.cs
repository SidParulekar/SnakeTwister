using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinResult : MonoBehaviour
{
    [SerializeField] private LivesController playerOneLivesController;
    [SerializeField] private LivesController playerTwoLivesController;

    private TextMeshProUGUI winResultText;

    private void Awake()
    {
        winResultText = GetComponent<TextMeshProUGUI>();
        DisplayResult();       
    }
    
    private void DisplayResult()
    {
        if (playerOneLivesController.GetLives() > playerTwoLivesController.GetLives())
        {
            winResultText.text = "Player 1 Wins!";
        }

        else if (playerOneLivesController.GetLives() < playerTwoLivesController.GetLives())
        {
            winResultText.text = "Player 2 Wins!";
        }

        else
        {
            winResultText.text = "Draw!";
        }
    }
}
