using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

public class SetNodesNeighboursWindow : EditorWindow
{

    [MenuItem("Window/NodeNeighboursBaker")]
    private static void Init()
    {
        var window = (SetNodesNeighboursWindow)EditorWindow.GetWindow(typeof(SetNodesNeighboursWindow));
        window.Show();
    }

    private int neighboursGenerated;

    private void OnGUI()
    {
        //Layer mask selection
        LayerMask tempMask = EditorGUILayout.MaskField("ObstaclesMask",InternalEditorUtility.LayerMaskToConcatenatedLayersMask(obstaclesLayerMask), InternalEditorUtility.layers);
        obstaclesLayerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);

        
        if (GUILayout.Button("Generate Node Neighbours"))
        {
            GenerateSceneNodeNeighbours();
            Repaint();
            Debug.Log($"{neighboursGenerated} neighbours generated");
        }
       
    }
    
    public LayerMask obstaclesLayerMask;
    
    public void GenerateSceneNodeNeighbours()
    {
        var nodes = FindObjectsOfType<Node>();
        neighboursGenerated = 0;

        for (int i = 0; i < nodes.Length; i++)
        {
            var currNode = nodes[i];
            
            GetNeighbours(currNode, Vector3.left);
            GetNeighbours(currNode, Vector3.right);
            GetNeighbours(currNode,Vector3.back);
            GetNeighbours(currNode,Vector3.forward);
            
            EditorUtility.SetDirty(currNode);
        }
        
        
    }

    private void GetNeighbours(Node n, Vector3 dir)
    {
        if (!Physics.Raycast(n.transform.position, dir, out var hit, 2.2f)) return;
        
        var node = hit.collider.GetComponent<Node>();
        
        if (node == null) return;
        
        neighboursGenerated++;
        n.neighbours.Add(node);

    }
    
}
