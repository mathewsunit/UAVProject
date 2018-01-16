using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DronePointer : MonoBehaviour {
	public GameObject arrow;
	public GameObject drone;
    public Transform parent1;
	// Use this for initialization
	void Start () {
        //Debug.Log("start");
        //parent1 = GetComponentInParent<Transform>();
        //this.transform.SetParent(parent1);
    }

    // Update is called once per frame
    void Update () {
        //this.transform.SetParent(parent1);
        //Debug.Log("parent"+parent1);
        if (arrow)
        {
            //arrow.transform.localPosition = parent1.transform.position;
            arrow.transform.LookAt(drone.transform);
        }
		
	}
}