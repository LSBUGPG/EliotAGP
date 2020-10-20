using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStones : MonoBehaviour
{
    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    public float speed;
    float WPradius = 1;
    public GameObject Player;

    private GameObject target = null;
    private Vector3 offset;
    void Start()
    {
        target = null;
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log ("HELLO");
        target = col.gameObject;
        offset = target.transform.position - transform.position;
    }
    void OnCollisionExit(Collision col)
    {
        target = null;
    }
    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
    }
}

