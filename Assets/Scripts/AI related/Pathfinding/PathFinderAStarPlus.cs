using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderAStarPlus : MonoBehaviour
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
  

    public List<Node> GetPathAStarPlus(Node startNode, Node finishNode)
    {
        _startNode = startNode;
        _finishNode = finishNode;
        _pathToReturn = _aStar.GetPath(_startNode, SatisfiesNode, GetNeighbours, GetCost, Heuristics, 500);
        _pathToReturn = _aStar.CleanPath(_pathToReturn, InView);
        return _pathToReturn;
    }

    private bool SatisfiesNode(Node node)
    {
        return node == _finishNode;
    }

    private List<Node> GetNeighbours(Node node)
    {
        return node.neighbours;
    }
    
    private float GetCost(Node parent, Node child)
    {
        float cost = 0;
        float distanceMultiplier = 1;
        cost += Vector3.Distance(parent.transform.position, child.transform.position) * distanceMultiplier;

        return cost;
    }

    private float Heuristics(Node node)
    {
        float cost = 0;
        float distanceMultiplier = 1;
        cost += Vector3.Distance(node.transform.position, _finishNode.transform.position) * distanceMultiplier;
        return cost;
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
