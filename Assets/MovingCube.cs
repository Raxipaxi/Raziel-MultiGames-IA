using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour, IMove
{
    // Start is called before the first frame update
    public float vel;

    public void Move(Vector3 dir)
    {
        transform.position += Time.deltaTime * dir * vel;
        transform.forward = Vector3.Lerp(transform.forward, dir, vel * Time.deltaTime);
    }
}
