using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra<N>
{
    public delegate bool Satisfies(N item);
    public delegate List<N> GetNeighbours(N item);
    public delegate float GetCost(N parent, N child);

    public List<N> GetPath(N start, Satisfies satisfies, GetNeighbours getNeighbours, GetCost getCost, int watchdog = 500)
    {
        HashSet<N> visited = new HashSet<N>();
        PriorityQueue<N> pending = new PriorityQueue<N>();
        Dictionary<N, N> parents = new Dictionary<N, N>();
        Dictionary<N, float> _costs = new Dictionary<N, float>();
        pending.Enqueue(start, 0);
        _costs[start] = 0;
        while (!pending.IsEmpty)
        {
            Debug.Log("DIJKSTRA");
            N curr = pending.Dequeue();
            if (satisfies(curr))
            {
                return BuildPath(curr, parents);
            }
            visited.Add(curr);
            List<N> neighbours = getNeighbours(curr);
            for (int i = 0; i < neighbours.Count; i++)
            {
                var currNeighbour = neighbours[i];
                if (visited.Contains(currNeighbour)) continue;
                float totalCost = _costs[curr] + getCost(curr, currNeighbour);
                if (_costs.ContainsKey(currNeighbour) && _costs[currNeighbour] < totalCost) continue;
                pending.Enqueue(currNeighbour, totalCost);
                parents[currNeighbour] = curr;
                _costs[currNeighbour] = totalCost;
            }
            watchdog--;
            if (watchdog <= 0)
            {
                break;
            }
        }
        return new List<N>();
    }
    List<N> BuildPath(N end, Dictionary<N, N> parents)
    {
        List<N> path = new List<N>();
        path.Add(end);
        while (parents.ContainsKey(path[path.Count - 1]))
        {
            path.Add(parents[path[path.Count - 1]]);
        }
        path.Reverse();
        return path;
    }
}
