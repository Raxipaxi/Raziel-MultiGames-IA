using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour, IInteractable
{
    [SerializeField] private int buttonID;
    public int ButtonID => buttonID;

    [HideInInspector, SerializeField] private PuzzleButtonSolver _parentSolver;

    private Renderer _myRenderer;

    private bool _interactable;

    private AudioSource _audioSrc;

    public Canvas InteractCanvas => throw new System.NotImplementedException();

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _audioSrc = GetComponent<AudioSource>();
        _interactable = true;
    }

    private void Start()
    {
        if (_parentSolver != null) Debug.Log("Tengo parent solver");
        else Debug.Log("No hay parent solver");
    }

    public void SetParentSolver (PuzzleButtonSolver instance)
    {
        _parentSolver = instance;
        if (_audioSrc != null) _audioSrc.clip = _parentSolver.PuzzleData.clickedSound;
    }
    public void OnInteract()
    {
        Debug.LogError("Interacted button");
        if (!_interactable) return;

        if (_parentSolver != null)
        {
            _audioSrc.Play();
            _interactable = false;
            _myRenderer.material = _parentSolver.PuzzleData.pressedMaterial;
            _parentSolver.CheckPuzzleState(buttonID);
        }
    }

    public void CorrectAll()
    {
        _myRenderer.material = _parentSolver.PuzzleData.correctMaterial;

        //Just in case
        _interactable = false;
    }

    public void ResetButton()
    {
        
        if (_parentSolver != null)
        {
            _interactable = true;
            _myRenderer.material = _parentSolver.PuzzleData.defaultMaterial;
        }
    }
}
