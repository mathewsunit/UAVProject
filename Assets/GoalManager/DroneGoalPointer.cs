using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGoalPointer : MonoBehaviour {
    public Transform transform_arrow;
    public DroneGoal current_target;
    public List<DroneGoal> all_goals = new List<DroneGoal>();
	// Use this for initialization
	void Start () {
		if(all_goals.Count > 0)
        {
            current_target = all_goals[0];
        }
	}
	
	// Update is called once per frame
	void Update ()
    { 
        if(current_target)
        {
            transform_arrow.LookAt(current_target.transform);
        }
	}

}
