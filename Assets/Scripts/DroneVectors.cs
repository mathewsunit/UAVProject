using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneVectors : MonoBehaviour {
    // Use this for initialization
    Rigidbody drone_body;
    LineRenderer velocity_renderer;
    public float velocity_scale = 0.3f;

	void Start () {
        drone_body = gameObject.GetComponent<Rigidbody>();
        velocity_renderer = gameObject.GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    { 
        if(drone_body)
        {
            //Draw the direction vector of the drone. 
            Vector3 velocity = drone_body.velocity;
            Debug.Log("Drone's velocity vector" + velocity);
            Vector3 vel_point = drone_body.transform.TransformDirection(velocity) * velocity_scale;
            velocity_renderer.SetPosition(0, gameObject.transform.position);
            velocity_renderer.SetPosition(1, vel_point);
            
            //Draw the velocity vector of the drone. 

            //Draw the thrust vectors of the drone. 

            //Draw the 
        }
        
          
    }
}
