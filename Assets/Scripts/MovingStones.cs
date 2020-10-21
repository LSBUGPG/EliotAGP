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
    Rigidbody physics = null;

    private ThirdPersonMovement player = null;
    void Start()
    {
        physics = GetComponent<Rigidbody>();
    }
    public void PushPlayer(ThirdPersonMovement player)
    {
        this.player = player;
    }
    public void StopPushingPlayer()
    {
        player = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }
        Vector3 previous = transform.position;
        if (physics != null)
        {
            Vector3 moveTo = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
            physics.MovePosition(moveTo);
            if (player != null)
            {
                player.Push(moveTo - previous);
            }
        }
    }
}
