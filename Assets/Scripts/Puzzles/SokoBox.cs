using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SokoBox : MonoBehaviour
{
    [SerializeField] private SokoBoxDat data;

    private Coroutine _moveTask;

    private Rigidbody _rb;

    [SerializeField]private HashSet<Vector3> _possibleLocations;
    [SerializeField] private LayerMask floorLayerMask;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void InitiateMove(Vector3 dir)
    {
        if (_moveTask != null) return;

        var raycastThrow = Physics.Raycast(transform.position, dir, 2);

        if (raycastThrow) return;

        _moveTask = StartCoroutine(MoveSokoBox(dir));
    }



    public IEnumerator MoveSokoBox(Vector3 dir)
    {
        var totalDistance = 0f;
        var initialPos = transform.position;
        var finalPos = initialPos + dir * data.totalMovement;
        var _counter = 0f;

        while (_counter <= data.timeToMove)
        {
            var newPos = Vector3.Lerp(initialPos, finalPos, _counter / data.timeToMove);
            _rb.MovePosition(newPos);
            _counter += Time.deltaTime;
            yield return null;
        }

        transform.position = finalPos;
        _moveTask = null;
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
}

