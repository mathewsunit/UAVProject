using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnmannedAerialVehicleTrainer.Drone
{
    public enum E_Indicator
    {
        E_THROTTLE_UP = 0,
        E_THROTTLE_DOWN = 1,
        E_YAW_CW = 2,
        E_YAW_CCW = 3,
        E_PITCH_FORWARD = 4,
        E_PITCH_BACKWARD = 5,
        E_ROLL_LEFT = 6,
        E_ROLL_RIGHT = 7
    }
    [System.Serializable]
    // If the indicator is present in a task, then its state is implicitly inferred as setting its state to true. 
    public struct Task
    {
        public E_Indicator[] indicators;    //Which indicators to activate
        public float time;          //And the length of time to hold this state
    }

    public class DroneGoalController : MonoBehaviour
    {
        public DroneController drone;
        public DroneGoal current_goal;
        //Add indicators
        public List<DroneGoal> all_goals = new List<DroneGoal>();
        public bool loop_goals;              //Loop through goals
        //Indicators
        public Transform pointer;
        public Indicator throttle_up_indicator;
        public Indicator throttle_down_indicator;
        public Indicator yaw_CCW_indicator;
        public Indicator yaw_CW_indicator;
        public Indicator pitch_forward_indicator;
        public Indicator pitch_back_indicator;
        public Indicator roll_left_indicator;
        public Indicator roll_right_indicator;
        //Renderers
        /* 0: Throttle up
         * 1: Throttle down
         * 2: Yaw CCW
         * 3: Yaw CW
         * 4: Pitch Forward 
         * 5: Pitch Backward
         * 6: Roll Left
         * 7: Roll Right
         */
        private Renderer[] indicator_renderers = new Renderer[8];
        private bool[] indicator_state = new bool[8];
        //Things relevant to all indicators
        public Color[] indicator_diffuse = { Color.white, Color.green };
        public Color[] indicator_emmisive = { Color.black, Color.green };
        //The commands given to the user by t
        public List<Task> task_list = new List<Task>();
        public float distance_tolerance = 0.5f;
        private int current_goal_index = 0;
        // Use this for initialization
        void Start()
        {
            if (!current_goal && all_goals.Count > 0)
            {
                current_goal = all_goals[0];
            }
            indicator_renderers[0] = throttle_up_indicator.GetComponent<Renderer>();
            indicator_renderers[1] = throttle_down_indicator.GetComponent<Renderer>();
            indicator_renderers[2] = yaw_CCW_indicator.GetComponent<Renderer>();
            indicator_renderers[3] = yaw_CW_indicator.GetComponent<Renderer>();
            indicator_renderers[4] =  pitch_forward_indicator.GetComponent<Renderer>();
            indicator_renderers[5] = pitch_back_indicator.GetComponent<Renderer>();
            indicator_renderers[6] = roll_left_indicator.GetComponent<Renderer>();
            indicator_renderers[7] = roll_right_indicator.GetComponent<Renderer>();
            for(int i = 0; i < indicator_renderers.Length; ++i)
            {
                indicator_renderers[i].material.SetColor("_Color", indicator_diffuse[0]);
                indicator_renderers[i].material.SetColor("_Emissive", indicator_emmisive[0]);
            }
            
        }
        public void NextGoal()
        {
            if (current_goal_index == (all_goals.Count - 1) && loop_goals)
            {
                current_goal_index = 0;
            }
            if (current_goal_index + 1 < all_goals.Count)
            {
                current_goal = all_goals[current_goal_index++];
            }
        }
        public void PreviousGoal()
        {
            if (current_goal_index == 0 && loop_goals)
            {
                current_goal_index = all_goals.Count - 1;
            }
            if (current_goal_index - 1 >= 0)
            {
                current_goal = all_goals[current_goal_index--];
            }
        }
        public void AddGoal(DroneGoal goal, int index)
        {
            all_goals.Insert(index, goal);
        }
        public void RemoveGoal(int index)
        {
            all_goals.RemoveAt(index);
        }
        
        private void SetIndicatorState(E_Indicator indicator, bool state)
        {
            int i_state = state ? 1 : 0;
            int i_indicator = (int)indicator;
            indicator_renderers[i_indicator].material.SetColor("_Color", indicator_diffuse[i_state]);
            indicator_renderers[i_indicator].material.SetColor("_Emissive", indicator_emmisive[i_state]);
            indicator_state[i_indicator] = state;
        }

        private bool GetIndicatorState(E_Indicator indicator)
        {
            int i_indicator = (int)indicator;
            return indicator_state[i_indicator];
        }
        IEnumerator Play()
        {
            foreach(var task in task_list)
            {
                for(int i = 0; i < 7; ++i)
                {
                    SetIndicatorState((E_Indicator)i, false);
                }
                for(int i = 0; i < task.indicators.Length; ++i)
                {
                    SetIndicatorState(task.indicators[i], true);
                }
                float hold_time = task.time;
                yield return new WaitForSeconds(hold_time);
            }
        }
        
        //A coroutine for testing the color change.
        IEnumerator TestColorChange()
        {
            E_Indicator current_indicator = E_Indicator.E_THROTTLE_UP;
            while (true)
            {
                bool state = GetIndicatorState(current_indicator);
                SetIndicatorState(current_indicator, !state);
                if (current_indicator == E_Indicator.E_ROLL_RIGHT)
                {
                    current_indicator = E_Indicator.E_THROTTLE_UP;
                }
                else
                {
                    current_indicator = (E_Indicator)(((int)current_indicator) + 1);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }

}
