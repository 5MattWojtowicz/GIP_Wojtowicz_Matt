using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string gameSceneName2;
    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }
public void Menu()
    {
        SceneManager.LoadScene(gameSceneName2);
    }
    public void Quit() { 
        Application.Quit(); 
    }
}
