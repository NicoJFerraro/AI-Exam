using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour
{
    [HideInInspector]
    public bool transitable = true;

    public List<Node> neighbours = new List<Node>();

    public Room myRoom;
    public float NeighbourDistanceAdd;

    public LayerMask wallLayer;
    private void Awake()
    {
        
    }

    private void Start()
    {

        myRoom = GameManager.instance.room;
        wallLayer = myRoom.wallsLayer;

        foreach (var item in myRoom.myNodes)
        {
            if ((item.transform.position - transform.position).magnitude < NeighbourDistanceAdd && item != this)
            {
                if (InSight(this, item))
                    neighbours.Add(item);
            }
        }
    }

    private bool InSight(Node a, Node b)
    {
        var hit = Physics.Linecast(a.transform.position, b.transform.position, wallLayer);

        return !hit;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, NeighbourDistanceAdd);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {                        
            transitable = false;

            foreach (var item in neighbours)
            {
                item.neighbours.Remove(this);
            }

            GameManager.instance.room.myNodes.Remove(this);
            Destroy(gameObject);

        }
    }

}
