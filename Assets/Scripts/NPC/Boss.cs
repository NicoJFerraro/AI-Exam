using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Boss : NPCs {

    public float shootDist;

    public FlockingNPCs _spawneado;


    public float coeficientShoot;
    public float coeficientSpawn;
    public float coeficientIdle;
    public float coeficientMove;

    public List<float> initialValues = new List<float>();

    public float timer = 0;
    public float timemove;
    public float timeidle;
    public float timeshoot;
    public float timespawn;

    public float spawnCooldown;
    public int spawnAmount;

    public float minDistance;

    public Slider helthBar;

    public List<FlockingNPCs> spawned = new List<FlockingNPCs>();


    protected override void Start()
    {
        base.Start();
        
        if (team == 0)
        {         
            GameManager.instance.redOnesList.Add(this);
        }
        else if (team == 1)
        {            
            GameManager.instance.blueOnesList.Add(this);
        }

        if (myPathfinder == null)
            myPathfinder = gameObject.AddComponent<PathFinder>();

        
        _sm.AddState(new StateIdleNPCs(_sm, this));
        _sm.AddState(new StateSeekNPCs(_sm, this));
        _sm.AddState(new StateFleeNPCs(_sm, this));
        _sm.AddState(new StateShootNPCs(_sm, this));
        _sm.AddState(new StateSpawnNPCs(_sm, this));
        _sm.AddState(new StateDie(_sm, this));

        initialValues.Add(coeficientIdle);//idle = 0
        initialValues.Add(coeficientMove);//move = 1
        initialValues.Add(coeficientShoot);//shoot = 2
        initialValues.Add(coeficientSpawn);//spawn = 3

        timer = timemove;
        shootCoolDown = timeshoot / (cantShoot - 0.1f);
        spawnCooldown = timespawn / (spawnAmount - 0.1f);


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        helthBar.value = health / maxHealth;

        timer -= Time.deltaTime;
        actionTimer -= Time.deltaTime;

        if (target)
            TargetinSightMethod();

        if(target && targetInSight && Vector3.Distance(transform.position, target.transform.position) > minDistance)
        {
            initialValues[1] = coeficientMove; //move
            initialValues[0] = 0;
        }
        else
        {
            initialValues[0] = coeficientIdle;
            initialValues[1] = 0;
        }
        RaycastHit hit;

        if (target && Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, shootDist))
        {
            if(hit.collider.GetComponent<NPCs>()== target)
            initialValues[2] = coeficientShoot;
            else
                initialValues[2] = 0;
        }
        else
            initialValues[2] = 0;

        if (spawned.Count < 4)
            initialValues[3] = 15;
        else if (spawned.Count < 7)
            initialValues[3] = 8;
        else if (spawned.Count < 10)
            initialValues[3] = 3;
        else
            initialValues[3] = 0;

        if (timer <= 0 || donePath == true)
        {
            donePath = false;

            if (health <= maxHealth * 0.35f && GameManager.instance.bosses.Count == 2 && GameManager.instance.bosses.Where(x => x != this).First().health > health)
            {
                _sm.SetState<StateFleeNPCs>();
                print("StateFleeNPCs");
                timer = timemove;
            }
            else if (health <= maxHealth * 0.35f)
            {
                _sm.SetState<StateFleeNPCs>();
                print("StateFleeNPCs");
                timer = timemove;
            }
            else
            {
                int decisionIndex = RouletteWheelSelection(initialValues);
                switch (decisionIndex)
                {
                    case 0:
                        _sm.SetState<StateIdleNPCs>();
                        print("StateIdleNPCs");
                        timer = timeidle;
                        break;
                    case 1:
                        _sm.SetState<StateSeekNPCs>();
                        print("StateSeekNPCs");
                        timer = timemove;
                        break;
                    case 2:
                        _sm.SetState<StateShootNPCs>();
                        print("StateShootNPCs");
                        timer = timeshoot;
                        break;
                    case 3:
                        _sm.SetState<StateSpawnNPCs>();
                        print("StateSpawnNPCs");
                        timer = timespawn;
                        break;
                }
            }
            
        }       
        _sm.Update();

    }

    public override void ModifieHP(float dmg)
    {
        base.ModifieHP(dmg);
        helthBar.value =  health / maxHealth;
    }



    public static int RouletteWheelSelection(List<float> values)
    {
        float sumatoria = 0;

        sumatoria = values.Sum();

        List<float> coefList = new List<float>();

        for (int i = 0; i < values.Count; i++)
        {
            coefList.Add(values[i] / sumatoria);
        }

        System.Random rnd = new System.Random();
        int rndPercent = rnd.Next(100);
        float r = rndPercent / 100f;

        float sumCoef = 0;
        for (int i = 0; i < values.Count; i++)
        {
            sumCoef += coefList[i];

            if (sumCoef > r)
                return i;
        }

        return -1;


    }

}
