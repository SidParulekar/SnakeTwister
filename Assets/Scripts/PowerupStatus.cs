using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerupStatus : MonoBehaviour
{
    private TextMeshProUGUI powerupStatus;

    private string defaultText;

    private void Awake()
    {
        powerupStatus = GetComponent<TextMeshProUGUI>();
        defaultText = powerupStatus.text;
    }

    public void RefreshUI(string powerup, string status)
    {
        powerupStatus.text = defaultText + powerup + " " + status;

        switch (powerup)
        {
            case "Shield":
                powerupStatus.color = new Color32(8, 150, 255, 255);
                break;

            case "Speed Boost":
                powerupStatus.color = new Color32(230, 255, 8, 255);
                break;

            case "Score Boost":
                powerupStatus.color = new Color32(235, 139, 27, 255);
                break;
        }
    }
}
