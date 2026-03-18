using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void COOP()
    {
        SceneManager.LoadScene(2);
    }

    public void Vs()
    {
        SceneManager.LoadScene(3);
    }
}
