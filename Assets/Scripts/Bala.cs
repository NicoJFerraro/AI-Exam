using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{

    public float speed;
    public float lifetime;

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, lifetime);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("NPC") || collision.gameObject.layer == LayerMask.NameToLayer("Flock"))
        {
            Destroy(this.gameObject);
        }
    }
}
