using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainCanvas : MonoBehaviour
{
   
    [SerializeField] private GameObject pauseMenu;
    //[SerializeField] private GameObject enemiesLeftMenu;
    //[SerializeField] private TextMeshProUGUI  enemiesLeftText;

    [SerializeField] private float timeToFadeMissionCanvas;
    private WaitForSeconds _waitForSeconds;
    [SerializeField] private TextMeshProUGUI _missionTextComponent;

    [SerializeField] private Animator transition; 

    public enum TransitionStates
    {
       ToBlack,
       FromBlack
    }        
  
    [SerializeField] private GameObject _missionCanvas;
    [SerializeField] private Image _playerHealth;
   

    
    private void Start()
    {       
        _missionCanvas.SetActive(false);     
        //ActivateEnemiesLeftCanvas(false);
        _waitForSeconds = new WaitForSeconds(timeToFadeMissionCanvas);        
        GameManager.Instance.GetcanvasRef(this);  
    }


    public void ChangeCurrentMission(string newMission)
    {
        _missionTextComponent.text = newMission;
        StartCoroutine(MissionCanvasFader());
    }
    public void UpdateHealth(float newFillAmount)
    {
        _playerHealth.fillAmount = newFillAmount;     
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
                transition.SetTrigger("Fade");
                break;
            case TransitionStates.FromBlack:
                transition.SetTrigger("FromBlack");
                break;
            default:
                throw new System.Exception("Transition state not implemented");              
        }
    }

    private IEnumerator MissionCanvasFader()
    {       
        _missionCanvas.SetActive(true);
        yield return _waitForSeconds;        
        _missionCanvas.SetActive(false);
    }

    public void RestartScene()
    {
        GameManager.Instance._sceneManagement.ReloadScene();
     //   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    /*
    public void ActivateEnemiesLeftCanvas(bool isActive)
    {
        enemiesLeftMenu.SetActive(isActive);
    }
    public void UpdateEnemiesLeft(int newAmountOfEnemies)
    {
        if (newAmountOfEnemies <= 0)
        {
            ActivateEnemiesLeftCanvas(false);
        }

        enemiesLeftText.text = "Enemies left: " + newAmountOfEnemies.ToString();
    }
    */
    private void OnDestroy()
    {
        Debug.Log("DestroyedCanvas");
        GameManager.Instance.EliminateCanvasRef(this);
    }
}
