using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderAStar : MonoBehaviour
{

    private List<Node> _pathToReturn;

    private Node _startNode;
    private Node _finishNode;

    [SerializeField] private LayerMask obstaclesMask;
    private void Awake()
    {
        _aStar = new AStar<Node>();
    }

   
    private AStar<Node> _aStar;
    private Dijkstra<Node> _dijkstra;

    public PathFinderAStar(Node startNode, Node finishNode,)
    {
        _startNode = startNode;
        _finishNode = finishNode;
        _pathToReturn = _aStar.GetPath()
    }

    private bool SatisfiesNode(Node node)
    {
        return node == _finishNode;
    }

    private List<Node> GetNeighbours(Node node)
    {
        return node.neighbours;
    }

    private bool InView(Node from, Node to)
    {
        return InView(from.transform.position, to.transform.position);
    }

    private bool InView(Vector3 from, Vector3 to)
    {
        var dir = to - from;
        return !Physics.Raycast(from, dir.normalized, dir.magnitude, obstaclesMask);
    }

}
