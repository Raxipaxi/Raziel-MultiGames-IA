using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : INode
{
    private RouletteWheel _roulette;
    Dictionary<INode, float> _items;
    public RandomNode(Dictionary <INode,float> items)
    {
        _roulette = new RouletteWheel();
        _items = items;
    }

    public RandomNode (RouletteWheel roulette, Dictionary<INode, float> items)
    {
        _roulette = roulette;
        _items = items;
    }
    public void Execute()
    {
        INode node = _roulette.Run(_items);
        node.Execute();
    }
}
