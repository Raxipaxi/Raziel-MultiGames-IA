using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagement
{
    public int CurrentLevel { get; private set; }

    private Dictionary<Scenes, string> _scenes;

    private Action _lockCursor;
    private Action _unlockCursor;

    private float counter;
    public enum Scenes
    {
        MainMenu,
        Level1,
        Level2
    }

    public SceneManagement(Action lockCursor, Action unlockCursor)
    {      
        _scenes = new Dictionary<Scenes, string>
        {
            { Scenes.MainMenu, "MainMenu" },
            { Scenes.Level1, "Demo_Main" },
            { Scenes.Level2, "Level2" }
        };

        _lockCursor = lockCursor;
        _unlockCursor = unlockCursor;
    }

    public void Initialize()
    {
        {
            if (GameManager.Instance.MainCanvas != null)
            GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.FromBlack);

            //It's the main menu

            if (CheckIfMainMenu()) _unlockCursor();
            else _lockCursor();          
        }
    }

    public bool CheckIfMainMenu()
    {
        return SceneManager.GetActiveScene().name == _scenes[Scenes.MainMenu];
    }

    
    public void  MakeChangeScene(Scenes newScene)
    {
        if (GameManager.Instance.MainCanvas != null)
        GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.ToBlack);
                    
        while (counter < 1.5f)
        {
            counter += Time.deltaTime;
            new WaitForEndOfFrame();
        }

        counter = 0;
        SceneManager.LoadScene(_scenes[newScene]);
        if (GameManager.Instance.MainCanvas != null)
            GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.FromBlack);

        if (CheckIfMainMenu()) _unlockCursor();
        else _lockCursor();
    }   

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }










}
