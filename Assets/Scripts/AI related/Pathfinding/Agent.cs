using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    BFS<Node> _bfs;
    DFS<Node> _dfs;

    public Node start;
    public Node finit;
    List<Node> _path;
    void Awake()
    {
        _bfs = new BFS<Node>();
        _dfs = new DFS<Node>();
    }
    public void SetPathBFS()
    {
        _path = _bfs.GetPath(start, Satisfies, GetNeighbours, 500);
        //_crash.SetWayPoints(_path);
        //_box.SetWayPoints(_path);
    }
    public void SetPathDFS()
    {
        _path = _dfs.GetPath(start, Satisfies, GetNeighbours, 500);
        //_crash.SetWayPoints(_path);
        //_box.SetWayPoints(_path);
    }

    bool Satisfies(Node curr)
    {
        return !curr.hasTrap;
    }
    List<Node> GetNeighbours(Node curr)
    {
        var newList = new List<Node>();
        for (int i = 0; i < curr.neighbours.Count; i++)
        {
            var currNeigh = curr.neighbours[i];
            //if (currNeigh.hasTrap && Random.value <= .5f) continue;
            newList.Add(currNeigh);
        }
        return newList;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var y = 0;
        if (_path != null)
        {
            for (int i = 0; i < _path.Count - 1; i++)
            {
                var initLine = _path[i].transform.position;
                var finitLine = _path[i + 1].transform.position;
                initLine.y = y;
                finitLine.y = y;
                Gizmos.DrawLine(initLine, finitLine);
            }
        }
    }
}
