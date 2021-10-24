using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class QuestionNode : INode
{
    private Func<bool> _question;
    private INode _trueNode, _falseNode;

    public QuestionNode (Func <bool> question, INode trueNode,INode falseNode)
    {
        _question = question;
        _trueNode = trueNode;
        _falseNode = falseNode;
    }

    public void Execute()
    {   
        if (_question())
        {
            _trueNode.Execute();
        }
        else
        {
            _falseNode.Execute();
        }
    }
}
