﻿/*
Copyright ©2017. The University of Texas at Dallas. All Rights Reserved. 

Permission to use, copy, modify, and distribute this software and its documentation for 
educational, research, and not-for-profit purposes, without fee and without a signed 
licensing agreement, is hereby granted, provided that the above copyright notice, this 
paragraph and the following two paragraphs appear in all copies, modifications, and 
distributions. 

Contact The Office of Technology Commercialization, The University of Texas at Dallas, 
800 W. Campbell Road (AD15), Richardson, Texas 75080-3021, (972) 883-4558, 
otc@utdallas.edu, https://research.utdallas.edu/otc for commercial licensing opportunities.

IN NO EVENT SHALL THE UNIVERSITY OF TEXAS AT DALLAS BE LIABLE TO ANY PARTY FOR DIRECT, 
INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES, INCLUDING LOST PROFITS, ARISING 
OUT OF THE USE OF THIS SOFTWARE AND ITS DOCUMENTATION, EVEN IF THE UNIVERSITY OF TEXAS AT 
DALLAS HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

THE UNIVERSITY OF TEXAS AT DALLAS SPECIFICALLY DISCLAIMS ANY WARRANTIES, INCLUDING, BUT 
NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE. THE SOFTWARE AND ACCOMPANYING DOCUMENTATION, IF ANY, PROVIDED HEREUNDER IS 
PROVIDED "AS IS". THE UNIVERSITY OF TEXAS AT DALLAS HAS NO OBLIGATION TO PROVIDE 
MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
*/

using UnityEngine;
using System.Collections;

public class ControllerHand : MonoBehaviour {
	
	// Enumerate states of virtual hand interactions
	public enum VirtualHandState {
		Open,
		Touching,
		Holding
	};

	// Inspector parameters
	[Tooltip("The tracking device used for tracking the real left hand.")]
	public CommonTracker leftTracker;

    [Tooltip("The tracking device used for tracking the real right hand.")]
    public CommonTracker rightTracker;

    [Tooltip("The interactive used to represent the left virtual hand.")]
	public Affect leftHand;

    [Tooltip("The interactive used to represent the right virtual hand.")]
    public Affect rightHand;

    [Tooltip("One of the buttons (left) used to grab objects.")]
	public CommonButton leftTrigger;

    [Tooltip("One of the buttons (right) used to grab objects.")]
    public CommonButton rightTrigger;

    [Tooltip("One of the grip buttons (left) to reset.")]
    public CommonButton leftGrip;

    [Tooltip("One of the buttons (right) to reset.")]
    public CommonButton rightGrip;

    [Tooltip("The speed amplifier for thrown objects. One unit is physically realistic.")]
	public float speed = 1.0f;
    
    // Private interaction variables
    VirtualHandState state;
	FixedJoint grasp;

    public Transform drone_transform;
    public Vector3 reset_position_offset = Vector3.up;
    [SerializeField]
    private Vector3 initial_drone_position;
    [SerializeField]
    private Quaternion initial_drone_rotation;

	// Called at the end of the program initialization
	void Start () {
        initial_drone_position = drone_transform.position;
        initial_drone_rotation = drone_transform.rotation;

		// Set initial state to open
		state = VirtualHandState.Open;

		// Ensure hand interactive is properly configured
		leftHand.type = AffectType.Virtual;
        rightHand.type = AffectType.Virtual;
    }

	// FixedUpdate is not called every graphical frame but rather every physics frame
	void FixedUpdate ()
	{

        if (leftGrip.GetPress() && rightGrip.GetPress())
        {
            
            // Reset the game
            drone_transform.position = initial_drone_position + reset_position_offset;
            drone_transform.rotation = initial_drone_rotation;
            drone_transform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        }

        // If state is open
        if (state == VirtualHandState.Open) {
			
			// If the hand is touching something
			if (rightHand.triggerOngoing) {

				// Change state to touching
				state = VirtualHandState.Touching;
			}

			// Process current open state
			else {

				// Nothing to do for open
			}
		}

		// If state is touching
		else if (state == VirtualHandState.Touching) {

			// If the hand is not touching something
			if (!rightHand.triggerOngoing) {

				// Change state to open
				state = VirtualHandState.Open;
			}

			// If the hand is touching something and the button is pressed
			else if (rightHand.triggerOngoing && rightTrigger.GetPress() && leftTrigger.GetPress()) {

				// Fetch touched target
				Collider target = rightHand.ongoingTriggers [0];
				// Create a fixed joint between the hand and the target
				grasp = target.gameObject.AddComponent<FixedJoint> ();
				// Set the connection
				grasp.connectedBody = rightHand.gameObject.GetComponent<Rigidbody> ();

				// Change state to holding
				state = VirtualHandState.Holding;
			}

			// Process current touching state
			else {

				// Nothing to do for touching
			}
		}

		// If state is holding
		else if (state == VirtualHandState.Holding) {

			// If grasp has been broken
			if (grasp == null) {
				
				// Update state to open
				state = VirtualHandState.Open;
			}
				
			// If button has been released and grasp still exists
			else if (!rightTrigger.GetPress() && !leftTrigger.GetPress() && grasp != null) {

				// Get rigidbody of grasped target
				Rigidbody target = grasp.GetComponent<Rigidbody> ();
				// Break grasp
				DestroyImmediate (grasp);

				// Apply physics to target in the event of attempting to throw it
				target.velocity = rightHand.velocity * speed;
				target.angularVelocity = rightHand.angularVelocity * speed;

				// Update state to open
				state = VirtualHandState.Open;
			}

			// Process current holding state
			else {

				// Nothing to do for holding
			}
		}
        
	}
}