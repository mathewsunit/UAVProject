using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnmannedAerialVehicleTrainer.Drone
{
	public class DronePropAnimator : MonoBehaviour {
		[SerializeField] private Transform m_front_left_prop; 
		[SerializeField] private Transform m_front_right_prop;
		[SerializeField] private Transform m_back_left_prop;
		[SerializeField] private Transform m_back_right_prop;	

		private DroneController m_Drone; //A reference to the DroneController script. 
		private float max_rotor_rpm = 11592; 	//Taken from https://phantompilots.com/threads/motor-rpm.16886/
		private const float k_Rpm_to_Dps = 60f; //Converting between revolutions per minute to degrees per second. 

		private Renderer m_frontLeft_renderer;
		private Renderer m_frontRight_renderer;
		private Renderer m_backLeft_renderer;
		private Renderer m_backRight_renderer;


		private void Awake()
		{
			m_Drone = GetComponent<DroneController> ();
			m_frontLeft_renderer = m_front_left_prop.GetComponent<Renderer> ();
			m_frontRight_renderer = m_front_right_prop.GetComponent<Renderer> ();
			m_backLeft_renderer = m_back_left_prop.GetComponent<Renderer> ();
			m_backRight_renderer = m_back_right_prop.GetComponent<Renderer> ();

		}

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
			//Just update the rotors spinning.
			m_front_left_prop.Rotate (0, -max_rotor_rpm * m_Drone.Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
			m_front_right_prop.Rotate (0, max_rotor_rpm * m_Drone.Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
			m_back_left_prop.Rotate (0, max_rotor_rpm * m_Drone.Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);
			m_back_right_prop.Rotate (0, -max_rotor_rpm * m_Drone.Total_Throttle * Time.deltaTime * k_Rpm_to_Dps, 0);

		}
	}
}