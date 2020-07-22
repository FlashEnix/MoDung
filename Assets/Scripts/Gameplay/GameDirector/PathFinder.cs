using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    List<Tail> result = new List<Tail>();
    List<Node> checkedNodes;
    List<Node> waitingNodes;
    int pathLimit;
    private Tail[] _tails;

    public static PathFinder instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        _tails = FindObjectsOfType<Tail>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tail> getLastResult()
    {
        return result;
    }

    public void clearResult()
    {
        result.Clear();
    }

    public List<Tail> getPath(Tail start, Tail end, int limit = 1000)
    {
        pathLimit = limit;
        
        result = new List<Tail>();
        checkedNodes = new List<Node>();
        waitingNodes = new List<Node>();

        if (Vector3.Distance(start.transform.position, end.transform.position) < 0.05f) return result;

        Node startNode = new Node(start, 0, start.transform.position, end.transform.position, null);
        

        checkedNodes.Add(startNode);
        waitingNodes.AddRange(getChildNodes(startNode));
        if (waitingNodes.Count == 0) return result;

        int maxCheck = 100;
        int curCheck = 0;

        while (result.Count == 0)
        {
            if (curCheck >= maxCheck) break;
            if (waitingNodes.Count == 0) break;
            int minF = waitingNodes.Min(x => x.F);
            var nodes = waitingNodes.Where(x => x.F == minF);
            List<Node> newNodes = new List<Node>();

            foreach (Node node in nodes)
            {
                curCheck++;
                newNodes.AddRange(getChildNodes(node));
                if (result.Count > 0) return result;
            }

            waitingNodes.AddRange(newNodes);
            foreach (Node n in  checkedNodes)
            {
                waitingNodes.Remove(n);
            }
        }
        return result;
    }

    private List<Node> getChildNodes(Node node)
    {
        List<Node> childNodes = new List<Node>();

        Collider[] colliders = Physics.OverlapSphere(node.position, 1, 1 << 8);

        foreach(Collider col in colliders)
        {
            Node childNode = new Node(col.GetComponent<Tail>(),node.G + 1, col.gameObject.transform.position, node.targetPosition, node);

            if (checkedNodes.Exists(x => Mathf.Abs(Vector3.Distance(x.position,childNode.position)) < 0.05))
            {
                continue;
            }

            if (childNode.position == node.targetPosition)
            {
                addResult(childNode);
                childNodes.Add(childNode);
                return childNodes;
            }
            if (!checkTailFree(col.transform.position)) continue;
            childNodes.Add(childNode);
        }
        checkedNodes.Add(node);
        return childNodes;
    }

    private void addResult(Node node)
    {
        if (node.previousNode != null)
        {
            addResult(node.previousNode);
        }
        if (result.Count <= pathLimit)
        {
            result.Add(node.tail);
        }
    }

    public bool checkTailFree(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);

        foreach (Collider col in colliders)
        {
            if (col.GetComponent<ITailObject>() != null && col.gameObject.tag != "DeathPlayer") return false;
        }
        return true;
    }

    public List<Vector3> GetPath(Vector3 start, Vector3 end, int limit = 1000)
    {
        List<Vector3> result = new List<Vector3>();
        Tail startTail = GetTailObject(start);
        Tail endTail = GetTailObject(end);

        List<Tail> path = getPath(startTail, endTail, limit);

        foreach (Tail t in path)
        {
            result.Add(t.transform.position);
        }
        return result;
    }

    private Tail GetTailObject(GameObject obj)
    {
        return _tails.OrderBy(x => Vector3.Distance(obj.transform.position, x.transform.position)).FirstOrDefault();
    }

    private Tail GetTailObject(Vector3 position)
    {
        return _tails.OrderBy(x => Vector3.Distance(position, x.transform.position)).FirstOrDefault();
    }
}

public class Node
{
    public Tail tail;
    public Vector3 position;
    public Vector3 targetPosition;
    public Node previousNode;
    public int F; // G+H
    public int G; // Расстояние от старта до ноды
    public int H; // Расстояние от ноды до финиша

    public Node (Tail Tail,int g, Vector3 nodePosition, Vector3 targetPos, Node prevNode)
    {
        tail = Tail;
        position = nodePosition;
        targetPosition = targetPos;
        previousNode = prevNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - position.x) + (int)Mathf.Abs(targetPosition.z - position.z);
        F = G + H;
    }
}
