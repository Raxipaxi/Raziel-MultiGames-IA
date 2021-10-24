using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BFS<N>
{
    //(N)=>bool //Func<N, bool>
    public delegate bool Satisfies(N item);

    //(N)=>List<N> //Func<N,List<N>>
    public delegate List<N> GetNeighbours(N item);

    public List<N> GetPath(N start, Satisfies satisfies, GetNeighbours getNeighbours, int watchdog = 500)
    {
        HashSet<N> visited = new HashSet<N>();
        Queue<N> pending = new Queue<N>();
        Dictionary<N, N> parents = new Dictionary<N, N>();

        pending.Enqueue(start);

        while (pending.Count != 0)
        {
            Debug.Log("BFS");
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
                if (visited.Contains(currNeighbour) || pending.Contains(currNeighbour)) continue;
                pending.Enqueue(currNeighbour);
                parents[currNeighbour] = curr;
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
