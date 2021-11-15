using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodesChanceController : MonoBehaviour
{
    [SerializeField] private List<Node> sceneNodes;

    [SerializeField] private float timeToUpdate;

    [SerializeField] private Transform target;

    private float _counter;

    private RouletteWheel _roulette;
    public Dictionary<Node, float> NodesChanceDictionary { get; private set; }

    private void Awake()
    {
        NodesChanceDictionary = new Dictionary<Node, float>();
        //Initialize with a chance of 1, so equal chance of getting any node
        foreach (var item in sceneNodes)
        {
            NodesChanceDictionary.Add(item,1);
        }
        _counter = 0;
        _roulette = new RouletteWheel();
    }

    
    public Node GetRandomNode()
    {
        var node = _roulette.Run(NodesChanceDictionary);
        return node;
    }
    

    private void SetChancesBasedOnTargetPosition()
    {
        var distance = 0f;
        foreach (var item in sceneNodes)
        {
            distance = Vector3.Distance(item.transform.position, target.transform.position);
            //Safeguard to avoid 1/0
            if (distance == 0) distance = 0.01f;
            var inverseDistance = 1 / distance;
            NodesChanceDictionary[item] = inverseDistance;
        }
    }

    public void GetSceneNodes()
    {
        sceneNodes = new List<Node>();
        var nodes = FindObjectsOfType<Node>();
        for (int i = 0; i < nodes.Length; i++)
        {
            sceneNodes.Add(nodes[i]);       
        }
        
        Debug.Log($"Got {sceneNodes.Count} nodes");
    }
    private void FixedUpdate()
    {
        _counter += Time.fixedDeltaTime;

        if (!(_counter >= timeToUpdate)) return;
        
        _counter = 0;
        SetChancesBasedOnTargetPosition();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NodesChanceController))]
internal class NodesChanceControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get nodes on scene"))
        {
            var curr = target as NodesChanceController;
            curr.GetSceneNodes();
            EditorUtility.SetDirty(curr);
        }
    }
}
#endif