using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class NPCs : MonoBehaviour
{

    public Transform spawnpoint;
    public StateMachine _sm;

    public NPCs target;
    public BalaEnemiga bullet;
    public float shootCoolDown;
    public float cantShoot;
    public float actionTimer;

    public float speed;

    public List<Collider> obstacles = new List<Collider>();
    public Vector3 dir;
    public Vector3 vectTarget;
    public Vector3 vectAvoidance;
    public Collider closerOb;

    public LayerMask layerObst, layerEnemy;
    
    public float rotationSpeed;
    public float radiusObstacle;
    public float avoidWeight;
    public float targetWeight;

    public int maxHealth;
    public float health;
    public float heal;


    public float viewAngle;
    public float viewDistance;
    public bool targetInSight;

    private Vector3 dirToTarget;
    private float distanceToTarget;
    private float angleToTarget;

    public PathFinder myPathfinder;

    protected Vector3 nextNodeDir;
    public Node closestNode;

    public bool hasPath;
    public bool followingNode;
    public List<Node> myPathList = new List<Node>();

    public Room myRoom;

    public int team;

    public bool donePath;

    public delegate List<Node> howPath();

    private void Awake()
    {
        health = maxHealth;
    }

    protected virtual void Start()
    {
        myRoom = GameManager.instance.room;

        if (team == 0)
        {
            GetComponent<Renderer>().material.color = Color.red;
            GameManager.instance.redOnesList.Add(this);
        }
        else if (team == 1)
        {
            GetComponent<Renderer>().material.color = Color.blue;
            GameManager.instance.blueOnesList.Add(this);
        }

        if (myPathfinder == null)
            myPathfinder = gameObject.AddComponent<PathFinder>();

        myPathfinder.roomNodes = myRoom.myNodes;
        myPathfinder.obsLayer = myRoom.wallsLayer;

        _sm = new StateMachine();

        closestNode = GetClosestWaypoint();

    }


    protected virtual void Update()
    {
        
        if (team == 0)
            target = GameManager.instance.blueOnesList.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault();
        else
            target = GameManager.instance.redOnesList.OrderBy(x => (x.transform.position - transform.position).magnitude).FirstOrDefault();

    }

    public Vector3 GetNode(howPath form)
    {

        closestNode = GetClosestWaypoint(); 

        if (myPathList == null || myPathList.Count == 0) hasPath = false;


        if (!hasPath) 
        {
            myPathList = form(); 
            hasPath = true;
        }
        if (myPathList != null && myPathList.Count > 0 && !followingNode) 
        {
            myPathList.RemoveAt(0); 

            if (myPathList.Count > 0) 
            {

                nextNodeDir = myPathList[0].transform.position;
            }
            followingNode = true; 

        }
        if ((nextNodeDir - transform.position).magnitude < 3)
        {
            donePath = true;
            followingNode = false; 
            hasPath = false; 
        }



        return (nextNodeDir - transform.position).normalized;
    }

    public List<Node> GetPathToTarget()
    {
        return myPathfinder.ReturnPath(closestNode, target.closestNode, PathFindingMethod.THETA);
    }

    public List<Node> GetPathToFlee()
    {
        Vector3 puntoLejano = (target.closestNode.transform.position - transform.position) * 200;
        Node lejano = myRoom.myNodes.OrderBy(x => Vector3.Distance(x.transform.position, puntoLejano)).First();
        return myPathfinder.ReturnPath(closestNode, lejano, PathFindingMethod.THETA);
    }

    public List<Node> GetPathToRandom()
    {
        return myPathfinder.ReturnPath(closestNode, myRoom.myNodes.Skip(UnityEngine.Random.Range(0, myRoom.myNodes.Count - 1)).First(), PathFindingMethod.THETA);

    }


    public Node GetClosestWaypoint()
    {

        Node closest = null;
        float Value = 999999;

        foreach (var item in myRoom.myNodes)
        {
            if ((item.transform.position - transform.position).magnitude < Value)
            {
                Value = (item.transform.position - transform.position).magnitude;
                closest = item;
            }
        }
        return closest;
    }

    public void TargetinSightMethod()
    {
        dirToTarget = (target.transform.position - transform.position).normalized;
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        angleToTarget = Vector3.Angle(transform.forward, dirToTarget);
        if (angleToTarget <= viewAngle && distanceToTarget <= viewDistance)
        {
            RaycastHit hit;
            targetInSight = true;

            if (Physics.Raycast(transform.position, dirToTarget, out hit, distanceToTarget))
            {
                if (hit.collider.gameObject.layer == layerObst)
                {
                    targetInSight = false;
                }
            }
        }
        else
        {
            targetInSight = false;
        }
    }


    public void GetObstacles()
    {
        obstacles.Clear();
        obstacles.AddRange(Physics.OverlapSphere(transform.position, radiusObstacle, layerObst));
    }

    public Collider GetCloserOb()
    {
        if (obstacles.Any())
        {
            Collider closer = null;
            float dist = Mathf.Infinity;
            foreach (var item in obstacles)
            {
                var newDist = Vector3.Distance(item.transform.position, transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    closer = item;
                }
            }
            return closer;
        }
        else
            return null;
    }

    public Vector3 getObstacleAvoidance()
    {
        if (closerOb)
            return transform.position - closerOb.transform.position;
        else return Vector3.zero;
    }

    public virtual void ModifieHP(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            Die();
    }

    virtual public void Die()
    {
        if (team == 0)
        {
            GameManager.instance.redOnesList.Remove(this);
            print(this.name);
        }
        else if (team == 1)
        {
            GameManager.instance.blueOnesList.Remove(this);
            print(this.name);
        }
        Destroy(this.gameObject);
    }

}
