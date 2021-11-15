using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FinalCanvas : MonoBehaviour
{

    [SerializeField] private Text scoreText;


    private void Awake()
    {
        scoreText.text = "Your score was " + GameManager.Instance.GetTotalScore().ToString();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance._sceneManagement.MakeChangeScene(SceneManagement.Scenes.Room1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}
