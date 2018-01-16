using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePropTest : MonoBehaviour
{

    GameObject body;
    GameObject[] up, down, backward, forward, right, left;

    GameObject forwardCapsule;

    Rigidbody rigidbody;

	// GameObject[] rotors;

    int numRotors = 4;

    public float velocity = 0.1f;
    public float rotationSpeed = 0.1f;
	public float upDirectionDivisionFactor = 3.0f;


    // Use this for initialization
    void Start()
    {
        this.body = GameObject.Find("pCube2");

        this.up = new GameObject[this.numRotors];
        this.down = new GameObject[this.numRotors];
        this.backward = new GameObject[this.numRotors];
        this.forward = new GameObject[this.numRotors];
        this.right = new GameObject[this.numRotors];
        this.left = new GameObject[this.numRotors];

        this.forwardCapsule = GameObject.Find("ForwardCapsule");
        this.forwardCapsule.GetComponent<Renderer>().material.color = Color.green;
		
		/*
		this.rotors = new GameObject[this.numRotors];
		this.rotors[0] = GameObject.Find("Prop");
		this.rotors[1] = GameObject.Find("Prop1");
		this.rotors[2] = GameObject.Find("Prop2");
		this.rotors[3] = GameObject.Find("Prop3");
		*/

		this.up[0] = GameObject.Find("Up");
        this.down[0] = GameObject.Find("Down");
        this.backward[0] = GameObject.Find("Backward");
        this.forward[0] = GameObject.Find("Forward");
        this.right[0] = GameObject.Find("Right");
        this.left[0] = GameObject.Find("Left");

        for (int i = 1; i < this.numRotors; i++)
        {
            this.up[i] = GameObject.Find(System.String.Format("Up ({0})", i));
            this.down[i] = GameObject.Find(System.String.Format("Down ({0})", i));
            this.backward[i] = GameObject.Find(System.String.Format("Backward ({0})", i));
            this.forward[i] = GameObject.Find(System.String.Format("Forward ({0})", i));
            this.right[i] = GameObject.Find(System.String.Format("Right ({0})", i));
            this.left[i] = GameObject.Find(System.String.Format("Left ({0})", i));
        }

        this.rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
		if (Input.GetKey(KeyCode.W)) {
			Debug.Log("Pressed W");

			for (int i = 0; i < this.numRotors; i++) {
				this.up[i].GetComponent<Renderer>().material.color = Color.red;
				this.down[i].GetComponent<Renderer>().material.color = Color.white;
				this.backward[i].GetComponent<Renderer>().material.color = Color.white;
				this.forward[i].GetComponent<Renderer>().material.color = Color.white;
				this.right[i].GetComponent<Renderer>().material.color = Color.white;
				this.left[i].GetComponent<Renderer>().material.color = Color.white;
			}

			Vector3 force = transform.up;
			// this.rigidbody.AddForce(force * this.velocity);
			this.rigidbody.transform.position += (force * (this.velocity / upDirectionDivisionFactor) * Time.deltaTime);
			/*
			for (int i = 0; i < this.numRotors; i++) {
				this.rotors[i].GetComponent<Rigidbody>().AddForce(force * this.velocity);
			}
			*/
			this.rigidbody.useGravity = false;

		} else if (Input.GetKey(KeyCode.S)) {
			Debug.Log("Pressed S");

			for (int i = 0; i < this.numRotors; i++) {
				this.up[i].GetComponent<Renderer>().material.color = Color.white;
				this.down[i].GetComponent<Renderer>().material.color = Color.red;
				this.backward[i].GetComponent<Renderer>().material.color = Color.white;
				this.forward[i].GetComponent<Renderer>().material.color = Color.white;
				this.right[i].GetComponent<Renderer>().material.color = Color.white;
				this.left[i].GetComponent<Renderer>().material.color = Color.white;
			}

			Vector3 force = -transform.up;
			// this.rigidbody.AddForce(force * this.velocity);
			/*
			for (int i = 0; i < this.numRotors; i++) {
				this.rotors[i].GetComponent<Rigidbody>().AddForce(force * this.velocity);
			}
			*/
			this.rigidbody.useGravity = true;
		} else if (Input.GetKey(KeyCode.A)) {
			Debug.Log("Pressed A");

			// this.rigidbody.rotation.SetEulerAngles(0, -1, 0);
			// Quaternion.Euler(0, -10, 0);
			transform.Rotate(-Vector3.up, this.velocity * Time.deltaTime);
		} else if (Input.GetKey(KeyCode.D)) {
			Debug.Log("Pressed D");

			// this.rigidbody.rotation.SetEulerAngles(0, -1, 0);
			// Quaternion.Euler(0, 10, 0);
			transform.Rotate(Vector3.up, this.velocity * Time.deltaTime);
		} else if (Input.GetKey(KeyCode.UpArrow)) {
			Debug.Log("Pressed Up Arrow");

			for (int i = 0; i < this.numRotors; i++) {
				this.up[i].GetComponent<Renderer>().material.color = Color.white;
				this.down[i].GetComponent<Renderer>().material.color = Color.white;
				this.backward[i].GetComponent<Renderer>().material.color = Color.white;
				this.forward[i].GetComponent<Renderer>().material.color = Color.red;
				this.right[i].GetComponent<Renderer>().material.color = Color.white;
				this.left[i].GetComponent<Renderer>().material.color = Color.white;
			}

			Vector3 force = transform.forward;
			this.rigidbody.AddForce(force * this.velocity);
			/*
			for (int i = 0; i < this.numRotors; i++) {
				this.rotors[i].GetComponent<Rigidbody>().AddForce(force * this.velocity);
			}
			*/

			Vector3 torque = new Vector3(0, 0, 1);
			// this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);



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


		} else if (Input.GetKey(KeyCode.DownArrow)) {
			Debug.Log("Pressed Down Arrow");

			for (int i = 0; i < this.numRotors; i++) {
				this.up[i].GetComponent<Renderer>().material.color = Color.white;
				this.down[i].GetComponent<Renderer>().material.color = Color.white;
				this.backward[i].GetComponent<Renderer>().material.color = Color.red;
				this.forward[i].GetComponent<Renderer>().material.color = Color.white;
				this.right[i].GetComponent<Renderer>().material.color = Color.white;
				this.left[i].GetComponent<Renderer>().material.color = Color.white;
			}

			Vector3 force = -transform.forward;
			this.rigidbody.AddForce(force * this.velocity);
			/*
			for (int i = 0; i < this.numRotors; i++) {
				this.rotors[i].GetComponent<Rigidbody>().AddForce(force * this.velocity);
			}
			*/

			Vector3 torque = new Vector3(0, 0, -1);
			// this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			Debug.Log("Pressed Left Arrow");

			for (int i = 0; i < this.numRotors; i++) {
				this.up[i].GetComponent<Renderer>().material.color = Color.white;
				this.down[i].GetComponent<Renderer>().material.color = Color.white;
				this.backward[i].GetComponent<Renderer>().material.color = Color.white;
				this.forward[i].GetComponent<Renderer>().material.color = Color.white;
				this.right[i].GetComponent<Renderer>().material.color = Color.white;
				this.left[i].GetComponent<Renderer>().material.color = Color.red;
			}

			Vector3 force = -transform.right;
			this.rigidbody.AddForce(force * this.velocity);
			/*
			for (int i = 0; i < this.numRotors; i++) {
				this.rotors[i].GetComponent<Rigidbody>().AddForce(force * this.velocity);
			}
			*/


			Vector3 torque = new Vector3(-1, 0, 0);
			//this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			Debug.Log("Pressed Right Arrow");

			for (int i = 0; i < this.numRotors; i++) {
				this.up[i].GetComponent<Renderer>().material.color = Color.white;
				this.down[i].GetComponent<Renderer>().material.color = Color.white;
				this.backward[i].GetComponent<Renderer>().material.color = Color.white;
				this.forward[i].GetComponent<Renderer>().material.color = Color.white;
				this.right[i].GetComponent<Renderer>().material.color = Color.red;
				this.left[i].GetComponent<Renderer>().material.color = Color.white;
			}

			Vector3 force = transform.right;
			this.rigidbody.AddForce(force * this.velocity);

			Vector3 torque = new Vector3(1, 0, 0);
			//this.rigidbody.AddRelativeTorque(torque * this.rotationSpeed);
		}
	}
}
