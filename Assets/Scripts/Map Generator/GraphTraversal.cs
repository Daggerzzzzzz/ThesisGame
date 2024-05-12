using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GraphTraversal
{
    private int vertex;
    private List<int>[] adjacency;
    private (int, int)[] distances;

    public GraphTraversal(int v1)
    {
        vertex = v1;
        adjacency = new List<int>[v1];
        distances = new (int, int)[v1];

        for (int i = 0; i < v1; i++)
        {
            if (adjacency != null) adjacency[i] = new List<int>();
        }
    }

    public void AddEdge(int v1, int v2)
    {
        adjacency[v1].Add(v2);
    }

    public void BFS(int start)
    {
        bool[] visited = new bool[vertex];

        Queue<int> queue = new Queue<int>();
        visited[start] = true;
        queue.Enqueue(start);
        distances[start] = (start, 0);

        while (queue.Count != 0)
        {
            int current = queue.Dequeue();

            foreach (int neighbor in adjacency[current])
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.Enqueue(neighbor);
                    distances[neighbor] = (neighbor, distances[current].Item2 + 1);
                }
            }
        }
    }
    
    public HashSet<(int, int)> GetDistancesAsHashSet()
    {
        HashSet<(int, int)> distanceSet = new HashSet<(int, int)>();
        foreach (var distance in distances)
        {
            distanceSet.Add(distance);
        }
        return distanceSet;
    }
    
    public void SortByDistance()
    {
        distances = distances.OrderBy(d => d.Item2).ToArray();
    }
    
    public void PrintTraversal()
    {
        for (int i = 0; i < vertex; i++)
        {
            List<int> neighbors = adjacency[i];
            string neighborsList = string.Join(", ", neighbors);
            Debug.Log($"{i + 1}: {neighborsList}");
        }
    }
}
