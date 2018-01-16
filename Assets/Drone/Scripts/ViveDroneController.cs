using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnmannedAerialVehicleTrainer.Drone
{
    public class ViveDroneController : MonoBehaviour
    {
        public DroneController drone;
        public CommonAxis left_joystick;
        public CommonAxis right_joystick;
        public CommonButton left_button;    //Press and hold.
        public CommonButton right_button;   //Press and hold. 
        public CommonButton left_trigger;   //For reset
        public CommonButton right_trigger;  //For reset
        public Transform left_cursor;
        public Transform right_cursor;
        private Vector3 left_joy_start_pos;
        private Vector3 right_joy_start_pos;
        private float max_translation_deflection = 0.003247f;
        private float input_tolerance = 0.1f;

        private Vector3 drone_start_position;
        private Quaternion drone_start_rotation;

        // Use this for initialization
        void Start()
        {
            left_joy_start_pos = left_cursor.localPosition;
            right_joy_start_pos = right_cursor.localPosition;
            drone_start_position = drone.transform.position;
            drone_start_rotation = drone.transform.rotation;
        }
        void FixedUpdate()
        {
            Vector2 raw_left = left_joystick.GetAxis();
            Vector2 raw_right = right_joystick.GetAxis();
            Vector2 input_left = raw_left * max_translation_deflection;
            Vector2 input_right = raw_right * max_translation_deflection;
            Vector3 left_deflection = new Vector3(input_left.x, 0f, input_left.y);
            Vector3 right_deflection = new Vector3(input_right.x, 0f, input_right.y);
            if(left_trigger.GetPress() && right_trigger.GetPress())
            {
                drone.transform.position = drone_start_position;
                drone.transform.rotation = drone_start_rotation;
                drone.GetComponent<Rigidbody>().velocity = Vector3.zero;
                drone.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            if (left_button.GetPress())
            {
                left_cursor.localPosition = left_joy_start_pos - left_deflection;
            }
            else
            {
                left_cursor.localPosition = left_joy_start_pos;
            }
            if (right_button.GetPress())
            {
                right_cursor.localPosition = right_joy_start_pos - right_deflection;
            }
            else
            {
                right_cursor.localPosition = right_joy_start_pos;
            }
        }
    }

}
