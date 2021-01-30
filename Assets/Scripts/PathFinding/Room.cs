using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Node> myNodes;
    public GameObject parentNodes;
    public LayerMask wallsLayer;
    

    private void Awake()
    {
        foreach (Transform item in parentNodes.transform)
        {
            if (item.GetComponent<Node>() != null)
            {
                myNodes.Add(item.GetComponent<Node>());
            }
        }
    }

}
