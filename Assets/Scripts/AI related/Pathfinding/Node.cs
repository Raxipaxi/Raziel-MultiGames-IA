using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbours;//<- Esto es lo unico que importa
    //Si utilizan este codigo con los raycast en el start/update/realtime son un punto menos por raycast.
    public bool hasTrap;
    Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        if (hasTrap)
            mat.color = Color.red;
        else
            mat.color = Color.white;
    }
}
