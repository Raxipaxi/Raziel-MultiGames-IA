using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject credits;
    [SerializeField] private string mainRoom1;


    public void ToGame()
    {
        SceneManager.LoadScene(mainRoom1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }

    public void ToCredits()
    {
        main.SetActive(false);
        credits.SetActive(true);
    }

    public void ToMain()
    {
        main.SetActive(true);
        credits.SetActive(false);
    }


}
