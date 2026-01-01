using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSimulation()
    {
        SceneManager.LoadScene("WorldTestScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
