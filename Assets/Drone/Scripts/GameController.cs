using System.Collections;
using System.Collections.Generic;
using UnmannedAerialVehicleTrainer.Drone;
using UnityEngine;

public class GameController : MonoBehaviour {
    public CommonAxis left_joystick;
    public CommonAxis right_joystick;
    public CommonButton left_trigger;
    public CommonButton right_trigger; 
    public DroneController drone;
    public enum E_Arrow
    {
        E_THROTTLE_UP,
        E_THROTTLE_DOWN,
        E_RUDDER_CW,
        E_RUDDER_CCW, 
        E_PITCH_FORWARD,
        E_PITCH_BACKWARD,
        E_ROLL_LEFT,
        E_ROLL_RIGHT,
    }
    //Meaningful virtual indicators
    [SerializeField] private Indicator throttle_up_arrow;           //The throttle up arrow
    [SerializeField] private Indicator throttle_down_arrow;         //The throttle down arrow
    [SerializeField] private Indicator rudder_CW_arrow;             //The CW rudder arrow
    [SerializeField] private Indicator rudder_CCW_arrow;            //The CCW rudder arrow
    [SerializeField] private Indicator pitch_forward_arrow;         //The pitch forwards arrow
    [SerializeField] private Indicator pitch_backward_arrow;        //The pitch backwards arrow
    [SerializeField] private Indicator roll_left_arrow;             //THe roll left arrow
    [SerializeField] private Indicator roll_right_arrow;            //The roll right arrow
    [SerializeField] private Transform left_cursor;                 //The cursor for the left touchpad
    [SerializeField] private Transform right_cursor;                //The cursor for the right touchpad
    //[SerializeField] private Indicator reset_indicator;             //The label that notes when the reset button is pressed. 

    //Accessory indicators 
    [SerializeField] private Indicator throttle_up_indicator;
    [SerializeField] private Indicator throttle_down_indicator;
    [SerializeField] private Indicator rudder_CW_indicator;
    [SerializeField] private Indicator rudder_CCW_indicator;
    [SerializeField] private Indicator pitch_forward_indicator;
    [SerializeField] private Indicator pitch_backward_indicator;
    [SerializeField] private Indicator roll_left_indicator;
    [SerializeField] private Indicator roll_right_indicator;

    //Indicator members: 
    public Color indicator_off_color = Color.white;
    public Color indicator_on_color = Color.green;
    //Joystick metaphors
    private Vector3 left_joy_start_position;
    private Vector3 right_joy_start_position;
    //Drone original rotation and location
    private Vector3 initial_drone_position;
    private Quaternion initial_drone_rotation;

    private Vector3 save_state_drone_position;
    private Quaternion save_state_drone_rotation;

    private float max_translation_deflection = 0.003247f;   //Maximum amount that the cursor is allowed to move on either axis.
    private float input_tolerance = 0.1f;
	// Use this for initialization
	void Start () {
        left_joy_start_position = left_cursor.localPosition;
        right_joy_start_position = right_cursor.localPosition;
        initial_drone_position = drone.transform.position;
        initial_drone_rotation = drone.transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 raw_left = left_joystick.GetAxis();
        Vector2 raw_right = right_joystick.GetAxis();
        //Debug.Log("Raw Left: " + raw_left);
        //Debug.Log("Raw Right: " + raw_right);
        Vector2 left_input = raw_left*max_translation_deflection;
        Vector2 right_input = raw_right*max_translation_deflection;
        Vector3 left_3 = new Vector3(left_input.x, 0f, left_input.y);
        Vector3 right_3 = new Vector3(right_input.x, 0f, right_input.y);
        left_cursor.localPosition = left_joy_start_position - left_3;
        right_cursor.localPosition = right_joy_start_position - right_3;
        if(left_trigger.GetPress() && right_trigger.GetPress())
        {
            drone.transform.position = initial_drone_position;
            drone.transform.rotation = initial_drone_rotation;
            drone.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            drone.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
	}
}
