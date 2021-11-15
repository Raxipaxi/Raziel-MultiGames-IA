using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SokoBox : MonoBehaviour,IReseteable
{
    [SerializeField] private SokoBoxDat data;

    private Coroutine _moveTask;

    private Rigidbody _rb;

    private Vector3 _initialPosition;

    [SerializeField] private LayerMask goalLayerMask;

    [SerializeField]private HashSet<Vector3> _possibleLocations;
    [SerializeField] private LayerMask floorLayerMask;

    private bool _unmovable;

    [SerializeField] private UnityEvent OnGoalReach;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        OnGoalReach.AddListener(() => _unmovable = true);
        _initialPosition = transform.position;
    }


    public void InitiateMove(Vector3 dir)
    {
        if (_moveTask != null || _unmovable) return;

        var raycastThrow = Physics.Raycast(transform.position, dir, 2);

        if (raycastThrow) return;

        _moveTask = StartCoroutine(MoveSokoBox(dir));
    }



    public IEnumerator MoveSokoBox(Vector3 dir)
    {
        var initialPos = transform.position;
        var finalPos = initialPos + dir * data.totalMovement;
        var counter = 0f;

        while (counter <= data.timeToMove)
        {
            var newPos = Vector3.Lerp(initialPos, finalPos, counter / data.timeToMove);
            _rb.MovePosition(newPos);
            counter += Time.deltaTime;
            yield return null;
        }

        transform.position = finalPos;
        _moveTask = null;

        if (CheckGoalReach()) OnGoalReach?.Invoke();
    }

    

    private bool CheckGoalReach()
    {
        var isGoal = Physics.Raycast(transform.position, Vector3.down, 2, goalLayerMask);

        return isGoal;
    }
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            var dir = other.contacts[0].normal;
            dir.y = 0;
            if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.z)) dir.z = 0;
            else dir.x = 0;
            dir = dir.normalized;
            InitiateMove(dir);
        }
    }

    public void OnLevelReset()
    {
        StopAllCoroutines();
        _moveTask = null;
        transform.position = _initialPosition;
    }
}

