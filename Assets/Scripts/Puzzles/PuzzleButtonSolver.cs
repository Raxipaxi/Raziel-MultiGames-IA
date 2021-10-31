using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleButtonSolver : MonoBehaviour,ITask
{

    [SerializeField] private List<PuzzleButton> buttons;
    [SerializeField] private ButtonOrderData _puzzleData;
    public ButtonOrderData PuzzleData => _puzzleData;

    [SerializeField] private int _taskID;
    public int TaskID => _taskID;

    private AudioSource _audioSrc;


    private int index = 0;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
    }
    private void Start()
    {
      
    }

    private void ResetPuzzle()
    {
        index = 0;
        foreach (var item in buttons)
        {
            item.ResetButton();
        }       
    }

    private void PuzzleComplete()
    {
        _audioSrc.clip = _puzzleData.correctSound;
        _audioSrc.Play();

        foreach (var item in buttons)
        {
            item.CorrectAll();
        }

       // GameManager.Instance.AccomplishedMission(_taskID);
    }
    
    public void CheckPuzzleState (int buttonID)
    {
        if (_puzzleData.buttonSolveOrder [index] == buttonID)
        {
            //Correct button, go on.
            index++;

            if (_puzzleData.buttonSolveOrder.Count <= index)
            {
                //Puzzle completed.

                PuzzleComplete();
            }
        }
        else
        {
            //Incorrect combination. Reset Puzzle and play sound
            _audioSrc.clip = _puzzleData.incorrectSound;
            _audioSrc.Play();
            ResetPuzzle();

        }
    }

#if UNITY_EDITOR
    public void GetChildButtons()
    {
        buttons = new List<PuzzleButton>();

        foreach (Transform item in transform)
        {
            if (item.parent != transform) continue;
           
            var button = item.GetComponent<PuzzleButton>();
            button.SetParentSolver(this);
            buttons.Add(button);

            EditorUtility.SetDirty(button);
        }

        EditorUtility.SetDirty(this);
    }
#endif
}


#if UNITY_EDITOR

[CustomEditor (typeof (PuzzleButtonSolver))]
class ButtonSolverEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var curr = target as PuzzleButtonSolver;

        if (GUILayout.Button ("Get Children Buttons"))
        {
            curr.GetChildButtons();
        }
    }
}
#endif
