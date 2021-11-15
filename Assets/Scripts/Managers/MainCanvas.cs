using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainCanvas : MonoBehaviour
{
   
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private float timeToFadeMissionCanvas;
    private WaitForSeconds _waitForSeconds;
    [SerializeField] private TextMeshProUGUI missionTextComponent;

    [SerializeField] private GameObject hintCanvas;
    [SerializeField] private Text hintText;
    [SerializeField] private Animator transition;
    [SerializeField] private Text gameCounter;



    public enum TransitionStates
    {
       ToBlack,
       FromBlack
    }        
  
    [SerializeField] private GameObject _missionCanvas;
    [SerializeField] private Image _playerHealth;
   

    private void Start()
    {       
        InitialState();
    }


    private void InitialState()
    {
        _missionCanvas.SetActive(false);
        _waitForSeconds = new WaitForSeconds(timeToFadeMissionCanvas);        
        GameManager.Instance.GetcanvasRef(this);  
        SetActiveHintCanvas(false);
    }

    public void SetHintText(string hint)
    {
        hintText.text = hint;
    }
    public void ChangeCurrentMission(string newMission)
    {
        missionTextComponent.text = newMission;
        _missionCanvas.SetActive(true);
    }

    public void EndCurrentMissionCanvas()
    {
        StartCoroutine(MissionCanvasFader());
    }
    public void UpdateHealth(float newFillAmount)
    {
        _playerHealth.fillAmount = newFillAmount;     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
          SetActiveHintCanvas(!hintCanvas.activeSelf);
        }
    }

    public void OnPauseHandler(bool state)
    {
        Debug.Log("On Pause handled");
        pauseMenu.SetActive(state);
    }
    
    public void ContinueGameplay()
    {
        GameManager.Instance.PauseGame();
    }

    public void TransitionSet (TransitionStates state)
    {
        switch (state)
        {
            case TransitionStates.ToBlack:
                transition.Play("ToBlack");
                break;
            case TransitionStates.FromBlack:
                transition.Play( "FromBlack");
                break;
            default:
                throw new System.Exception("Transition state not implemented");              
        }
    }

    private IEnumerator MissionCanvasFader()
    {
        yield return _waitForSeconds;        
        _missionCanvas.SetActive(false);
    }

    public void RestartScene()
    {
        GameManager.Instance._sceneManagement.ReloadScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetActiveHintCanvas(bool newState)
    {
        hintCanvas.SetActive(newState);
        //Maybe play sound
    }

  
    private void OnDestroy()
    {
        GameManager.Instance.EliminateCanvasRef(this);
    }
}
