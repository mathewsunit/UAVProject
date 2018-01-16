using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnmannedAerialVehicleTrainer.Drone;

/*
 * This script is used to display the arrows for the forward and velocity vectors onto the drone.
 * The velocity vector increases according to the distance and the velocity vector
 * The forward vector only increases based on the distance
 * 
 * The script is deployed onto a widget which is a child of the drone itself.
 * 
 * */
// [RequireComponent(typeof(LineRenderer))]
public class ArrowController : MonoBehaviour {


    //declare the transform components
    public Transform forward_Arrow;
    public Transform head_tracker;
    public DroneController drone;
    //obtain the multipliers for the two vectors
    //used to optimize the size of the arrows on testing
    private const float velocity_multiplier = 4.0f;
    private const float distance_multiplier = 0.7f;

    //optional line renderer which can be used to track the drone
    //we have another script for arrow pointing to the drone at all times 
	//LineRenderer lineRenderer;


    Rigidbody droneRigidBody;

    //transform components of the drone and the controller
    //Transform drone;
	//Transform controller;

	Vector3 rotationVector;

	float distance = 0.0f;

	// Use this for initialization
	void Start () 
	{
       
        //drone is the parent of the widget
        droneRigidBody = this.GetComponentInParent<Rigidbody>();

		//this.controller= GameObject.Find ("Vive HMD").transform;
        
		//lineRenderer = gameObject.GetComponent<LineRenderer>();

		//lineRenderer.widthMultiplier = 0.02f;


	}

	// Update is called once per frame
	void FixedUpdate () {

		//set the direction based on the velocity vector
        //Vector3 vel_direction = droneRigidBody.transform.TransformDirection(droneRigidBody.velocity);

        //set the line renderer points
        //lineRenderer.SetPosition(0, droneRigidBody.transform.position);
		//lineRenderer.SetPosition (1, droneRigidBody.velocity);

        //calculate the distance between the drone and controller position vectors
        float distance = Vector3.Distance (droneRigidBody.transform.position, head_tracker.transform.position);

        //set the local scale of the two arrows 
        //forward arrow only scales based on the distance
        forward_Arrow.transform.localScale = Vector3.one*distance*distance_multiplier;

        //the velocity arrow scales according to the current velocity vector as well as the distance


		}
	}

