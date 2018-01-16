using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* TODO: Come up with a more realistic flight model for this script. 
 * 		 Develop a method by which the rotors can be disabled (as if they were stopped or damaged by an in-flight collision)
 * 		 Break this script up into several scripts for organization purposes (not necessary, but this class is gettig a little crowded)
 */
namespace UnmannedAerialVehicleTrainer.Drone
{

	[RequireComponent(typeof(Rigidbody))]
	public class DroneController : MonoBehaviour { 
		[SerializeField] private float m_RollEffect = .3f;				//Scalar to determine effect of roll inputs. Defaults at 1. 
		[SerializeField] private float m_PitchEffect = .3f;				//Scalar to determine effect of pitch inputs. Defaults at 1. 
		[SerializeField] private float m_YawEffect = .3f; 				//Scalar to determine effect of yaw inputs. Defaults at 1. 
		[SerializeField] private float stability = 0.2f;				//Value that determines the strength at which the drone "returns" after input along an axis is released. 
		[SerializeField] private float rotation_speed = 1.0f;			//Value that determines the speed at which the drone returns after input along an axis is released. 
		[SerializeField] private float m_ThrottleChangeSpeed = 0.05f;	//Value that effects the amount at which throttle is applied. 

		[SerializeField] private float m_autolevel_max_pitch = 42f;		//The maximum angle allowed on pitch before further input is refused. (Unimplemented).
		[SerializeField] private float m_autolevel_min_pitch = -42f; 	//The minimum angle allowed on pitch before further input is refused. (Unimplemented).
		[SerializeField] private float m_autolevel_max_roll = 42f;		//The maximum angle allowed on roll before further input is refused. (Unimplemented).
		[SerializeField] private float m_autolevel_min_roll = -42f;		//The minimum angle allowed on roll before further input is refused. (Unimplemented).

		[SerializeField] private Transform FrontLeftRotor; 		//The attachment for the front left rotor
		[SerializeField] private Transform FrontRightRotor;		//The attachment for the front right rotor
		[SerializeField] private Transform BackLeftRotor;		//The attachment for the back left rotor
		[SerializeField] private Transform BackRightRotor; 		//The attachment for the back right rotor

		private const float max_rotor_rpm = 11592; 	//Taken from https://phantompilots.com/threads/motor-rpm.16886/
		private const float k_Rpm_to_Dps = 60f; //Converting between revolutions per minute to degrees per second. 
		private const float max_rotor_force = 4.905f;	//Maximum force allowed for the rotors. 
		private const float max_rotor_torque = 10f;	//Maximum torque allowed for the rotors. 
        private const float min_input = -1f;
        private const float max_input = 1f;
        private const float min_throttle_value = 0f;
        private const float max_throttle_value = 1f;
        private const float min_input_threshold = -0.8f;
        private const float max_input_threshold = 0.8f;

        private const float dead_zone_radius = 0.05f;   //Deadzone for controls. 

        private const float fl_rotor_max_force = 5f;
        private const float fr_rotor_max_force = 5f;
        private const float bl_rotor_max_force = 5f;
        private const float br_rotor_max_force = 5f;

        private const float fl_rotor_max_torque = 2f;
        private const float fr_rotor_max_torque = 2f;
        private const float bl_rotor_max_torque = 2f;
        private const float br_rotor_max_torque = 2f;

		public float Altitude { get; private set; }	//The current altitude of the drone in meters. 

		//This block of variables affects rotor speed
		public float Front_Left_Throttle {get; private set;}
		public float Front_Right_Throttle {get; private set;}
		public float Back_Left_Throttle {get; private set;}
		public float Back_Right_Throttle {get; private set;}
	    
        //This block of variables affects rotor torque: 
        public float Front_Left_Torque { get; private set; }
        public float Front_Right_Torque { get; private set; }
        public float Back_Left_Torque { get; private set; }
        public float Back_Right_Torque { get; private set; }

        //This block of variables affects rotor force. 
		public float Front_Left_RotorPower { get; private set; }
		public float Front_Right_RotorPower { get; private set; }
		public float Back_Left_RotorPower { get; private set; }
		public float Back_Right_RotorPower { get; private set; }

		public float Total_Throttle { get; private set; }   //The total throttle for this vehicle. 
		//State variables for input / current state of the drone. 
		public float Throttle_Input{ get; private set; }    //The current input for throttle (might not be necessary)
		public float Speed {get; private set;} 			    //The current speed of the drone.   (Doesn't appear to do anything at the moment)
		public float PitchAngle {get; private set;}		    //The current pitch angle           (Yaw angle unnecessary)
		public float RollAngle { get; private set; }	    //The current roll angle            (Yaw angle unnecessary)
		public float PitchInput{ get; private set; }	    //The current pitch input
		public float RollInput { get; private set; }	    //The current roll input
		public float YawInput  { get; private set; }    	//The current yaw input
		//The rigid body that is used to simulate the drone as a physics object
		private Rigidbody m_RigidBody; 
        //For resetting the drone.
        private bool m_Immobilized = false;         //Was the drone immobilized?

        //For our controllers
        public CommonAxis Throttle_Yaw;
        public CommonAxis Pitch_Roll;
        public CommonButton left_joypad;
        public CommonButton right_joypad;

        //Audio stuff
        public AudioSource sound_source;

        // Use this for initialization
        void Awake()
        {
            //Initialize rigid body.
            m_RigidBody = GetComponent<Rigidbody>();

            sound_source = GetComponent<AudioSource>();
        }
        public void Move(float pitchInput, float rollInput, float yawInput, float throttleInput)
		{
            //1. Get input
			RollInput = rollInput;
			PitchInput = pitchInput;
			YawInput = yawInput;
			Throttle_Input = throttleInput;
			ClampInputs ();
            //2. Get the current state of the quadcopter.
			CalculatePitchRollAngles();
            CalculateAltitude();
            HandleThrottle();
			CalculateLinearForces ();
			CalculateTorque ();
		}
		private void ClampInputs()
		{
			RollInput = Mathf.Clamp (RollInput, -1, 1);
			PitchInput = Mathf.Clamp (PitchInput, -1, 1);
            YawInput = Mathf.Clamp(YawInput, -1, 1);
            Throttle_Input = Mathf.Clamp(Throttle_Input, min_input_threshold, max_input_threshold);
		}
        private void HandleThrottle()
        {
            if(m_Immobilized)
            {
                Total_Throttle = 0f;
            }
            //Map the input from [-0.9,0.9] |-> [0,1]
            Total_Throttle = Mathf.Clamp01(linear_stretch(Throttle_Input, min_input_threshold, max_input_threshold, min_throttle_value, max_throttle_value)); //Clamp might not be necessary, but is included to ensure that it remains within the 0-1 range. 
            
            Front_Left_RotorPower = Total_Throttle * max_rotor_force;
            Front_Right_RotorPower = Total_Throttle * max_rotor_force;
            Back_Left_RotorPower = Total_Throttle * max_rotor_force;
            Back_Right_RotorPower = Total_Throttle * max_rotor_force;
        }
        private float linear_stretch(float x, float old_min, float old_max, float new_min, float new_max)
        {
            return new_min + ((x - old_min)*(new_max - new_min))/(old_max - old_min);
        }
        //Constrains input based on current vehicle state. (DEPRECATED)
		private void ControlThrottle()
		{
			//If the drone cannot move, then override throttle input. 
			if (m_Immobilized) {
				Throttle_Input = 0f;	
			}
			//Handle input
			Total_Throttle = Mathf.Clamp01(Total_Throttle + Throttle_Input*Time.deltaTime*m_ThrottleChangeSpeed);
			//Calculate the throttle and power on each rotor based on the inputs and the current rotation angles. 

			Front_Left_RotorPower = Total_Throttle * max_rotor_force;
			Front_Right_RotorPower = Total_Throttle * max_rotor_force;
			Back_Left_RotorPower = Total_Throttle * max_rotor_force; 
			Back_Right_RotorPower = Total_Throttle * max_rotor_force;

		}
        private void CalculatePitchRollAngles()
        {
            var flatForward = transform.forward;
            flatForward.y = 0;
            if(flatForward.sqrMagnitude > 0f)
            {
                flatForward.Normalize();
                var localFlatForward = transform.InverseTransformDirection(flatForward);
                PitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z);
                var flatRight = Vector3.Cross(Vector3.up, flatForward);
                var localFlatRight = transform.InverseTransformDirection(flatRight);
                RollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x);
            }
        }
		//A simple implementation that calculates the linear forces applied to this vehicle by the rotors. 
		private void CalculateLinearForces()
		{
			var forces = Vector3.zero; 
			forces += Front_Left_RotorPower * FrontLeftRotor.transform.up;
			forces += Front_Right_RotorPower * FrontRightRotor.transform.up;
			forces += Back_Left_RotorPower * BackLeftRotor.transform.up;
			forces += Back_Right_RotorPower * BackRightRotor.transform.up;
			m_RigidBody.AddForce (forces);
		}
        //TODO: Make a more accurate flight model that involves torque from the props.
		private void CalculateTorque()
		{
			var torque = Vector3.zero;
			torque += -PitchInput * m_PitchEffect * transform.right;
			torque += YawInput * m_YawEffect * transform.up;
			torque += RollInput * m_RollEffect * transform.forward;
            float roll_in = RollInput * m_RollEffect;
			//Autolevel
			var predicted_up = Quaternion.AngleAxis (m_RigidBody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / rotation_speed, m_RigidBody.angularVelocity) * transform.up;
			var auto_level_torque = Vector3.Cross (predicted_up, Vector3.up)*rotation_speed*rotation_speed;
			torque += auto_level_torque;
			m_RigidBody.AddTorque (torque);
		}

		private void CalculateAltitude()
		{
			var ray = new Ray (transform.position - Vector3.up * 10, -Vector3.up);
			RaycastHit hit; 
			Altitude = Physics.Raycast (ray, out hit) ? hit.distance + 10f : transform.position.y;
		}
		private void Immobilize()
		{
			m_Immobilized = true; 
		}
        private void Reset()
        {
            m_Immobilized = false;
        }

        void FixedUpdate()
        {
            /* Controls: 
			 * Throttle: W-S
			 * Rudder: D-A
			 * Pitch: up-down
			 * Ailerons: right-left
			 * D - Yaw Right
			 */
            if (Pitch_Roll != null && Throttle_Yaw != null)
            {
                Vector2 pitch_roll = Vector2.zero;
                Vector2 throttle_yaw = Vector2.zero;
                if(left_joypad.GetPress())
                {
                    Debug.Log("LEFT TRIGGERED");
                    throttle_yaw = Throttle_Yaw.GetAxis();
                    if(Mathf.Abs(throttle_yaw.x) < dead_zone_radius)
                    {
                        throttle_yaw.x = 0f;
                    }
                    if (Mathf.Abs(throttle_yaw.y) < dead_zone_radius)
                    {
                        throttle_yaw.y = 0f;
                    }
                }
                if(right_joypad.GetPress())
                {
                    Debug.Log("RIGHT TRIGGERED!");
                    pitch_roll = Pitch_Roll.GetAxis();
                    if(Mathf.Abs(pitch_roll.x) < dead_zone_radius)
                    {
                        pitch_roll.x = 0f;
                    }
                    if(Mathf.Abs(pitch_roll.y) < dead_zone_radius)
                    {
                        pitch_roll.y = 0f;
                    }
                }
                
                float pitch_input = pitch_roll.y;
                float roll_input = pitch_roll.x;
                float throttle_input = throttle_yaw.y;
                float yaw_input = throttle_yaw.x;

                //Debug.Log("throttle_input: " + throttle_input);
                Move(pitch_input, roll_input, yaw_input, throttle_input);
            }
            else
            {
                float pitch_input = Input.GetAxis("Elevators");
                float roll_input = Input.GetAxis("Ailerons");
                float throttle_input = Input.GetAxis("Throttle");
                float yaw_input = Input.GetAxis("Rudder");
                Move(pitch_input, roll_input, yaw_input, throttle_input);
            }
            //

            this.FrontLeftRotor.Rotate(0, -max_rotor_rpm * Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
            this.FrontRightRotor.Rotate(0, max_rotor_rpm * Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
            this.BackLeftRotor.Rotate(0, max_rotor_rpm * Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
            this.BackRightRotor.Rotate(0, -max_rotor_rpm * Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
            //Adjust the pitch of the sound source. 
            sound_source.pitch = linear_stretch(this.Total_Throttle, 0.0f, 1.0f, 0.0f, 2.0f);
        }
	}

}
