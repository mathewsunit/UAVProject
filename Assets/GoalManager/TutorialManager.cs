using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnmannedAerialVehicleTrainer.Drone;

public enum E_Indicator
{
    E_THROTTLE_UP = 0,
    E_THROTTLE_DOWN = 1,
    E_YAW_CW = 2,
    E_YAW_CCW = 3,
    E_PITCH_FORWARD = 4,
    E_PITCH_BACKWARD = 5,
    E_ROLL_LEFT = 6,
    E_ROLL_RIGHT = 7,
    E_LEFT_CURSOR = 8,
    E_RIGHT_CURSOR = 9
}
public enum E_TutorialEvent_Command
{
    E_STOP,
    E_NEXT_EVENT,
    E_PREV_EVENT
}

[System.Serializable]
public struct TutorialEvent
{
    public AudioSource audio_cue;        // Audio cue to play 
    public List<E_Indicator> indicators; // Indicators to flash
    public int times_to_flash;           // Number of times to flash the given indicator.
    public float event_time;             // Length of the event
    public E_TutorialEvent_Command command; //Enables starting and stopping. 
}
public class TutorialManager : MonoBehaviour {
    private Color[] indicator_diffuse = { Color.white, Color.green };
    private Color[] indicator_emissive = { Color.black, Color.green };
    private const float FLASH_TIME = .5f;   //Flash should be 2 times per second.
    private int event_index = 0;
    public DroneController drone;
    public Transform goal_pointer;  //Points towards next goal.
    public Indicator Throttle_Up;
    public Indicator Throttle_Down;
    public Indicator Rudder_CCW;
    public Indicator Rudder_CW;
    public Indicator Pitch_Forward;
    public Indicator Pitch_Backward;
    public Indicator Roll_Left;
    public Indicator Roll_Right;
    public Indicator Left_Cursor;
    public Indicator Right_Cursor;
    private IEnumerator coroutine;  //The coroutine.
    public List<TutorialEvent> events;
    /* 0: Throttle up
     * 1: Throttle down
     * 2: Yaw CCW
     * 3: Yaw CW
     * 4: Pitch Forward 
     * 5: Pitch Backward
     * 6: Roll Left
     * 7: Roll Right
     * 8: Left Cursor
     * 9: Right Cursor
     */
    public Renderer[] indicator_renderers = new Renderer[10];
    public bool[] indicator_states = new bool[10];
    public void ToNextEvent()
    {
        this.event_index--;
    }
    public void ToPreviousEvent()
    {
        this.event_index++;
    }
    public void PlayNextEvent()
    {
        ToNextEvent();
        coroutine = PlayEvent(events[this.event_index]);
        StartCoroutine(coroutine);
    }
    public void PlayPreviousEvent()
    {
        ToPreviousEvent();
        coroutine = PlayEvent(events[this.event_index]);
        StartCoroutine(coroutine);
    }
    public void StopEvent()
    {
        StopCoroutine(coroutine);
    }
	// Use this for initialization
	void Start () {
        indicator_renderers[0] = Throttle_Up.GetComponent<Renderer>();
        indicator_renderers[1] = Throttle_Down.GetComponent<Renderer>();
        indicator_renderers[2] = Rudder_CCW.GetComponent<Renderer>();
        indicator_renderers[3] = Rudder_CW.GetComponent<Renderer>();
        indicator_renderers[4] = Pitch_Forward.GetComponent<Renderer>();
        indicator_renderers[5] = Pitch_Backward.GetComponent<Renderer>();
        indicator_renderers[6] = Roll_Left.GetComponent<Renderer>();
        indicator_renderers[7] = Roll_Right.GetComponent<Renderer>();
        indicator_renderers[8] = Left_Cursor.GetComponent<Renderer>();
        indicator_renderers[9] = Right_Cursor.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator PlayEvent(TutorialEvent evnt)
    {
        float audio_length = 0f;
        if(evnt.audio_cue)
        {
            audio_length = evnt.audio_cue.clip.length;
            evnt.audio_cue.Play();
        }
        
        int num_times_to_flash = evnt.times_to_flash;
        audio_length = ((float)num_times_to_flash * FLASH_TIME);
        if (audio_length <= 0f)
        {
            audio_length = 5f;
        }
        for (int i = 0; i < num_times_to_flash; ++i)
        {
            foreach(E_Indicator cator in evnt.indicators)
            {
                Toggle_Indicator(cator);
                yield return new WaitForSeconds(FLASH_TIME);
            }
        }
        Reset_Indicators();
        yield return new WaitForSeconds(audio_length);
    }
    
    private void Activate_Indicator(E_Indicator indicator_code)
    {
        int indicator_index = (int)indicator_code;
        indicator_states[indicator_index] = true;
        indicator_renderers[indicator_index].material.SetColor("_Color", indicator_diffuse[1]);
        indicator_renderers[indicator_index].material.SetColor("_Emissive", indicator_emissive[1]);
    }
    private void Deactivate_Indicator(E_Indicator indicator_code)
    {
        int indicator_index = (int)indicator_code;
        indicator_states[indicator_index] = false;
        int i_state = indicator_states[indicator_index] ? 1 : 0;
        indicator_renderers[indicator_index].material.SetColor("_Color", indicator_diffuse[0]);
        indicator_renderers[indicator_index].material.SetColor("_Emissive", indicator_emissive[0]);
    }
    private void Toggle_Indicator(E_Indicator indicator_code)
    {
        int indicator_index = (int)indicator_code;
        indicator_states[indicator_index] = !indicator_states[indicator_index];
        int i_state = indicator_states[indicator_index] ? 1 : 0;
        indicator_renderers[indicator_index].material.SetColor("_Color", indicator_diffuse[i_state]);
        indicator_renderers[indicator_index].material.SetColor("_Emissive", indicator_emissive[i_state]);
    }
    private void Reset_Indicators()
    {
       for(int i = 0; i < 10; ++i)
        {
            indicator_states[i] = false;
            indicator_renderers[i].material.SetColor("_Color", indicator_diffuse[0]);
            indicator_renderers[i].material.SetColor("_Emissive", indicator_emissive[0]);
        }
    }
    IEnumerator Run_Test()
    {
        E_Indicator indicator_1 = E_Indicator.E_THROTTLE_UP;
        E_Indicator indicator_2 = E_Indicator.E_THROTTLE_DOWN;
        while(true)
        {
            Toggle_Indicator(indicator_1);
            Toggle_Indicator(indicator_2);
            yield return new WaitForSeconds(.5f);
        }
        
    }
}
