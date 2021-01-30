using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlockingNPCs : NPCs
{
    public Color redColor, blueColor;

    public Boss leader;

    public int damage;

    public LayerMask LayerBoidEnemies;
    public List<Collider> boidEnemies;

    public float flockRadius;
    public float detectradius;
    public float targetrradius;

    public float aggroWeight;
    public float leaderWeight;
    public float cohesionWeight;
    public float alineationWeight;
    public float separationWeight;

    private Vector3 _vectLider;
    private Vector3 _vectCohesion;
    private Vector3 _vectAlineacion;
    private Vector3 _vectSeparacion;

    protected override void Start()
    {
        base.Start();
        
        if (GameManager.instance.bosses.Where(x => x.team == team).Any())
            leader = GameManager.instance.bosses.Where(x => x.team == team).First();


        if (myPathfinder == null)
            myPathfinder = gameObject.AddComponent<PathFinder>();

        _sm.AddState(new StateFlock(_sm, this));
        _sm.AddState(new StateSeekNPCs(_sm, this));
        _sm.AddState(new StateFleeNPCs(_sm, this));
    }

    protected override void Update()
    {
        base.Update();

        closestNode = GetClosestWaypoint();


        if (target)
            TargetinSightMethod();

        if (team == 0 && GameManager.instance.blueOnesList.Any())
            target = GameManager.instance.blueOnesList.OrderBy(x => (x.transform.position - transform.position).magnitude).First();
        else if (team == 1 && GameManager.instance.redOnesList.Any())
            target = GameManager.instance.redOnesList.OrderBy(x => (x.transform.position - transform.position).magnitude).First();


        if (health < maxHealth * 0.5f)
            _sm.SetState<StateFleeNPCs>();
        else if (!_sm.IsActSt<StateFlock>() && leader != null)
            _sm.SetState<StateFlock>();       
        else if (!_sm.IsActSt<StateSeekNPCs>() && (leader == null || targetInSight))
        {
            _sm.SetState<StateSeekNPCs>();
        }
        

        _sm.Update();

    }

    public override void Die()
    {
        if (leader)
            leader.spawned.Remove(this);
        base.Die();
    }

    void Move()
    {
        transform.forward = Vector3.Slerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
    public void Flock()
    {
        closerOb = GetCloserOb();
        _vectCohesion = getCohesion() * cohesionWeight;
		_vectAlineacion = getAlignment() * alineationWeight;
		_vectSeparacion = getSeparation() * separationWeight;
		_vectLider = getLeader() * leaderWeight;
        vectAvoidance = getObstacleAvoidance() * avoidWeight;

        dir = vectAvoidance;
		dir += _vectCohesion + _vectAlineacion + _vectSeparacion + _vectLider;
        dir = new Vector3(dir.x, 0, dir.z);
        Move();


    }


    public void GetBoidEnemiesAndObstacles()
    {
		boidEnemies.Clear ();
		boidEnemies.AddRange(Physics.OverlapSphere (transform.position, detectradius, LayerBoidEnemies).Where(x=>x.GetComponent<NPCs>() && x.GetComponent<NPCs>().team == team));
        GetObstacles();

	}

    public void OnTriggerEnter(Collider collision)
    {
        NPCs n = collision.gameObject.GetComponent<NPCs>();
        if (n && n.team != team)
        {
            n.ModifieHP(damage);
            Die();
        }
    }

    #region Vectores

    //Alineacion
    Vector3 getAlignment()
    {
        Vector3 alig = new Vector3();
        foreach (var item in boidEnemies)
            alig += item.transform.forward;
        return alig /= boidEnemies.Count;
    }

    //Separacion
    Vector3 getSeparation()
    {
		Vector3 separation = new Vector3 ();
        foreach (var item in boidEnemies)
        {
            Vector3 f = new Vector3();
            f = transform.position - item.transform.position;
            float magnitude = flockRadius - f.magnitude;
            f.Normalize();
            f *= magnitude;
            separation += f;
        }
        return separation /= boidEnemies.Count;
    }

    //Cohesion
    Vector3 getCohesion()
    {
		Vector3 cohesion = new Vector3 ();
        foreach (var item in boidEnemies)
            cohesion += item.transform.position - transform.position;
        return cohesion /= boidEnemies.Count;
    }


    //Seguir al lider
    Vector3 getLeader()
    {
           if (leader != null)
                return leader.transform.position - transform.position;
            else
                return Vector3.zero;     
    }

    
    #endregion
}
