using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
   
    public bool IsPaused { get; private set; }
    public event Action<bool> OnPause;

    public SceneManagement _sceneManagement;

    [HideInInspector]public LevelResetHandler resetHandler;
    public MainCanvas MainCanvas { get; private set; }
    //Camera is cached for no overhead cost
    private Camera _mainCamera;
    public Camera MainCamera
    {
        get
        {
            //Make sure to always search for camera if it's null
            if (_mainCamera == null) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }

    private void Awake()
    {
        if (GameManager.Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            //Wake up managers            
            _sceneManagement = new SceneManagement(LockCursor, UnlockCursor);
            Application.targetFrameRate = 144;
            PauseForce(false);
        }
    }

    //Mainly, reset variables that need to be reset on different levels
    private void OnLevelLoadHandler()
    {
       _sceneManagement.Initialize();
    }

    public void ResetLevelState()
    {
        Debug.Log("Reset the level pls");
        resetHandler.HandleResetOfLevel();
    }

    public void GetcanvasRef(MainCanvas canvas)
    {
        MainCanvas = canvas;
        OnPause += canvas.OnPauseHandler;
    }

    public void GetResetLevelHandler(LevelResetHandler levelResetHandler)
    {
        Debug.Log("level reset data obtained");
        resetHandler = levelResetHandler;
    }
    public void EliminateCanvasRef(MainCanvas canvas)
    {
        OnPause -= canvas.OnPauseHandler;
        MainCanvas = null;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!_sceneManagement.CheckIfMainMenu())
            PauseForce(!IsPaused);
    }

    public void PauseForce(bool state)
    {
        IsPaused = state;
        OnPause?.Invoke(state);
        Time.timeScale = state ? 0 : 1;
        if (state) UnlockCursor();
        else LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

}
