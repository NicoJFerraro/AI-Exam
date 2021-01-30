using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PathFindingMethod
{
    ASTAR,
    THETA
}


public class PathFinder : MonoBehaviour
{
    public List<Node> roomNodes;

    public Node _start;
    public Node _end;

    private List<Node> _path = new List<Node>();

    private List<Node> _nodesChecked;

    public LayerMask obsLayer;

    public List<Node> ReturnPath(Node start, Node end, PathFindingMethod p)
    {
        _start = start;
        _end = end;
        if (_start == null || _end == null) return null;

        _nodesChecked = new List<Node>();
        _nodesChecked.Clear();

        List<Node> path = null;

        switch (p)
        {

            case PathFindingMethod.ASTAR:
                path = AStar.Run(_start, Satisfies, GetWeightedNeighbours, Heuristic);
                break;

            case PathFindingMethod.THETA:
                path = ThetaStar.Run(_start, Satisfies, GetWeightedNeighbours, Heuristic, InSight, EuclideanDist);
                break;
        }



        return path;
    }




    private bool Satisfies(Node n)
    {
        _nodesChecked.Add(n);
        return n.Equals(_end);
    }

    private List<Tuple<Node, float>> GetWeightedNeighbours(Node current)
    {
        var neighbours = new List<Tuple<Node, float>>();
        foreach (var item in current.neighbours)
        {
            if (item.transitable)
            {
                neighbours.Add(Tuple.Create(item, Vector3.Distance(item.transform.position, current.transform.position)));
            }
        }
        return neighbours;
    }

    private float Heuristic(Node n)
    {
        return EuclideanDist(_end, n) * 1;
    }

    private float EuclideanDist(Node x, Node y)
    {

        return Vector3.Distance(x.transform.position, y.transform.position);
    }

    private bool InSight(Node a, Node b)
    {

        var hit = Physics.Linecast(a.transform.position, b.transform.position, obsLayer);


        return !hit;
    }



}
