using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneTest : MonoBehaviour {

	GameObject body, up, down, backward, forward, right, left;

	Rigidbody rigidbody;

	public float velocity = 0.1f;
	public float rotationSpeed = 0.1f;
	

	// Use this for initialization
	void Start () {
		this.body = GameObject.Find("Body");
		this.up = GameObject.Find("Up");
		this.down = GameObject.Find("Down");
		this.backward = GameObject.Find("Backward");
		this.forward = GameObject.Find("Forward");
		this.right = GameObject.Find("Right");
		this.left = GameObject.Find("Left");
		
		this.rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey(KeyCode.W)) {
			Debug.Log("Pressed W");

			this.up.GetComponent<Renderer>().material.color = Color.white;
			this.down.GetComponent<Renderer>().material.color = Color.white;
			this.backward.GetComponent<Renderer>().material.color = Color.white;
			this.forward.GetComponent<Renderer>().material.color = Color.red;
			this.right.GetComponent<Renderer>().material.color = Color.white;
			this.left.GetComponent<Renderer>().material.color = Color.white;

			Vector3 force = new Vector3(0, 0, 1);
			this.rigidbody.AddForce(force * this.velocity);

			Vector3 torque = new Vector3(0, 0, 1);
			this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);



			/*
			// Get position of target object
			Vector3 targetPosition = gameObject.transform.position;

			// Gives us vector to direction of target
			Vector3 inverseVect = transform.InverseTransformPoint(targetPosition);

			// Calculate angle by which you have to rotate
			// Note -: This angle is calculated every Frame of FixedUpdate
			float rotationAngle = Mathf.Atan2(inverseVect.x, inverseVect.z) * Mathf.Rad2Deg;

			// Now calculate  rotationVelocity to be applied every frame
			Vector3 rotationVelocity = (Vector3.up * rotationAngle) * rotationSpeed * Time.deltaTime;

			// Calaculate his delta velocity   i.e required - current 
			Vector3 deltavel = (rotationVelocity - rigidbody.angularVelocity);

			// Apply the force to rotate
			rigidbody.AddTorque(deltavel, ForceMode.Impulse);
			*/


		} else if (Input.GetKey(KeyCode.S)) {
			Debug.Log("Pressed S");

			this.up.GetComponent<Renderer>().material.color = Color.white;
			this.down.GetComponent<Renderer>().material.color = Color.white;
			this.backward.GetComponent<Renderer>().material.color = Color.red;
			this.forward.GetComponent<Renderer>().material.color = Color.white;
			this.right.GetComponent<Renderer>().material.color = Color.white;
			this.left.GetComponent<Renderer>().material.color = Color.white;

			Vector3 force = new Vector3(0, 0, -1);
			this.rigidbody.AddForce(force * this.velocity);

			Vector3 torque = new Vector3(0, 0, -1);
			this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);
		} else if (Input.GetKey(KeyCode.A)) {
			Debug.Log("Pressed A");

			this.up.GetComponent<Renderer>().material.color = Color.white;
			this.down.GetComponent<Renderer>().material.color = Color.white;
			this.backward.GetComponent<Renderer>().material.color = Color.white;
			this.forward.GetComponent<Renderer>().material.color = Color.white;
			this.right.GetComponent<Renderer>().material.color = Color.white;
			this.left.GetComponent<Renderer>().material.color = Color.red;

			Vector3 force = new Vector3(-1, 0, 0);
			this.rigidbody.AddForce(force * this.velocity);

			Vector3 torque = new Vector3(-1, 0, 0);
			this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);
		} else if (Input.GetKey(KeyCode.D)) {
			Debug.Log("Pressed D");

			this.up.GetComponent<Renderer>().material.color = Color.white;
			this.down.GetComponent<Renderer>().material.color = Color.white;
			this.backward.GetComponent<Renderer>().material.color = Color.white;
			this.forward.GetComponent<Renderer>().material.color = Color.white;
			this.right.GetComponent<Renderer>().material.color = Color.red;
			this.left.GetComponent<Renderer>().material.color = Color.white;

			Vector3 force = new Vector3(1, 0, 0);
			this.rigidbody.AddForce(force * this.velocity);

			Vector3 torque = new Vector3(1, 0, 0);
			this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);
		} else if (Input.GetKey(KeyCode.Q)) {
			Debug.Log("Pressed Q");

			this.up.GetComponent<Renderer>().material.color = Color.red;
			this.down.GetComponent<Renderer>().material.color = Color.white;
			this.backward.GetComponent<Renderer>().material.color = Color.white;
			this.forward.GetComponent<Renderer>().material.color = Color.white;
			this.right.GetComponent<Renderer>().material.color = Color.white;
			this.left.GetComponent<Renderer>().material.color = Color.white;

			Vector3 force = new Vector3(0, 1, 0);
			this.rigidbody.AddForce(force * this.velocity);
		} else if (Input.GetKey(KeyCode.E)) {
			Debug.Log("Pressed E");

			this.up.GetComponent<Renderer>().material.color = Color.white;
			this.down.GetComponent<Renderer>().material.color = Color.red;
			this.backward.GetComponent<Renderer>().material.color = Color.white;
			this.forward.GetComponent<Renderer>().material.color = Color.white;
			this.right.GetComponent<Renderer>().material.color = Color.white;
			this.left.GetComponent<Renderer>().material.color = Color.white;

			Vector3 force = new Vector3(0, -1, 0);
			this.rigidbody.AddForce(force * this.velocity);
		} else {
			this.up.GetComponent<Renderer>().material.color = Color.white;
			this.down.GetComponent<Renderer>().material.color = Color.white;
			this.backward.GetComponent<Renderer>().material.color = Color.white;
			this.forward.GetComponent<Renderer>().material.color = Color.white;
			this.right.GetComponent<Renderer>().material.color = Color.white;
			this.left.GetComponent<Renderer>().material.color = Color.white;
		}
	}
}
