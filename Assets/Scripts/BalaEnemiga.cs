using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEnemiga : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public NPCs target;
    public int myteam;
    public int damage;
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, lifetime);

    }

    // Update is called once per frame
    void Update()
    {
        if(target)
            transform.forward = target.transform.position-transform.position;
        transform.position += transform.forward * speed * Time.deltaTime;

    }
    public void OnTriggerEnter(Collider collision)
    {
        NPCs n = collision.gameObject.GetComponent<NPCs>();
        if (n && n.team != myteam)
        {
            n.ModifieHP(damage);
            Destroy(this.gameObject);
        }
    }


}


