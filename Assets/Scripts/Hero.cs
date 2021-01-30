using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    public Bala disparo;
    public float speed;
    public Rigidbody rb;
    public float timer = 0;
    public float reloadtime;
    public GameObject hero;
    bool shooting;
    float inertia;
    public static Vector3 cameraPos;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        

        float axisValue = Input.GetAxis("Horizontal");
        Vector3 currentVelocity = Vector3.right * axisValue * speed;
        axisValue = Input.GetAxis("Vertical");
        currentVelocity += Vector3.forward * axisValue * speed;
        rb.velocity = currentVelocity;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.forward = -hero.transform.right;
            inertia = -rb.velocity.x / 2;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.forward = hero.transform.right;
            inertia = rb.velocity.x / 2;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.forward = hero.transform.forward;
            inertia = rb.velocity.z / 2;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.forward = -hero.transform.forward;
            inertia = -rb.velocity.z / 2;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            shooting = true;
        }
        else shooting = false;

        if (timer <= 0)
        {
            timer = 0;
            if (shooting)
            {
                disparar();
                timer = reloadtime;
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }


    }


    public void disparar()
    {
        Bala obj = Instantiate(disparo);
        obj.transform.position = transform.position;
        obj.transform.forward = transform.forward;
        obj.speed += inertia;
    }

    private void OnTriggerEEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BalaEnemiga"))
        {
            print("Ouch");
        }
    }
}

