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

    public Action<bool> OnNewSceneLoad;
    private float counter;
    public enum Scenes
    {
        MainMenu,
        Level1,
        Level2,
        Level3,
        Room1,
        Room2,
        Room3,
        Win,
        Lose
    }

    
    public SceneManagement(Action lockCursor, Action unlockCursor)
    {      
        _scenes = new Dictionary<Scenes, string>
        {
            { Scenes.MainMenu, "MainMenu" },
            {Scenes.Room1,"MainRoom1"},
            { Scenes.Level1, "Sokoban" },
            { Scenes.Level2, "Herder" },
            {Scenes.Level3, "Stealth"},
            {Scenes.Room2, "MainRoom2"},
            {Scenes.Room3,"MainRoom3"},
            {Scenes.Win, "WinGame"},
            {Scenes.Lose,"LoseGame"}
        };

        _lockCursor = lockCursor;
        _unlockCursor = unlockCursor;

        OnNewSceneLoad += GameManager.Instance.OnLevelLoadHandler;
    }

    public void Initialize()
    {
        {
            if (GameManager.Instance.MainCanvas != null)
                GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.FromBlack);

            //It's the main menu

            //if (CheckIfMainMenu()) _unlockCursor();
            //else _lockCursor();          
        }
    }

    public bool CheckIfMainMenu()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var isMenuScene = sceneName == _scenes[Scenes.MainMenu] || sceneName == _scenes[Scenes.Win] ||
                          sceneName == _scenes[Scenes.Lose];
        return isMenuScene;
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
        
        OnNewSceneLoad?.Invoke(isSceneWithTime(newScene));
        if (GameManager.Instance.MainCanvas != null)
            GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.FromBlack);

       // if (CheckIfMainMenu()) _unlockCursor();
        //else _lockCursor();
    }

    private bool isSceneWithTime(Scenes newScene)
    {
        if (newScene == Scenes.Level1 || newScene == Scenes.Level2 || newScene == Scenes.Level3) return true;
        return false;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
