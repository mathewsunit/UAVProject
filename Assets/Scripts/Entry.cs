using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Entry : MonoBehaviour {
    

    //boolean to indicate whether the drone has passed through given hoop
	public bool enteredHoop1;
    public bool enteredHoop2;
    public bool enteredHoop3;

    

    //list of colliders colliding with the hoops
    List<Collider> currentTriggers1;
    List<Collider> currentTriggers2;
    List<Collider> currentTriggers3;

    //Next stage to redirect to
    public GameObject nextStage;

    //GameObject of parent
    GameObject gameObjParent;

    //Affect for hoop colliders
    public Affect pointCollider1;
    public Affect pointCollider2;
    public Affect pointCollider3;

    //Audio sources to play
    
    AudioSource audioSource;

    public AudioClip clipPassHoop;
    public AudioClip clipCompleteCourse;

    //Collider on drone
    public Collider drone;

    //reset position and orientation on drone
    private Vector3 drone_start_position;
    private Quaternion drone_start_rotation;

    public GameObject hangar;
	
    // Use this for initialization
	void Start () {

        //initialize booleans to false
		enteredHoop1 = false;
        enteredHoop2 = false;
        enteredHoop3 = false;

        audioSource = this.gameObject.AddComponent<AudioSource>();
       
        if (audioSource.clip != clipPassHoop)
        audioSource.clip = clipPassHoop;

        //get parent of current GameObject
        gameObjParent = this.transform.parent.gameObject;

        //initialize beginning and end positions
        drone_start_position = drone.gameObject.transform.position;
        drone_start_rotation = drone.gameObject.transform.rotation;

       


    }


    void Update()
    {
        //updates collider lists
        currentTriggers1 = pointCollider1.ongoingTriggers;
        currentTriggers2 = pointCollider2.ongoingTriggers;
        currentTriggers3 = pointCollider3.ongoingTriggers;


        //if drone goes through hoop 1, or has gone through it before 
        if ((pointCollider1.triggerExited && checkDrone(currentTriggers1)) || enteredHoop1)
        {

            if (!enteredHoop1)
            {
                //if first time, update score, play sound and display score
                enteredHoop1 = true;
                hangar.SendMessage("updateScore");
                //   print(audioPassHoop.name);
                audioSource.Play(0);
            }
            else { }
            //if drone goes through hoop 2, or has gone through it before 
            if ((pointCollider2.triggerExited && checkDrone(currentTriggers2)) || enteredHoop2)
            {
                //if first time, update score, play sound and display score
                if (!enteredHoop2)
                {
                 //   print(audioPassHoop.name);
                    audioSource.Play(0);
                    enteredHoop2 = true;
                    hangar.SendMessage("updateScore");
                }
                else { }

                //if drone goes through hoop 3, or has gone through it before 
                if ((pointCollider3.triggerExited && checkDrone(currentTriggers3)) || enteredHoop3)
                {

                    if (!enteredHoop3)
                    {
                        //if first time, update score, play sound and display score
                        audioSource.clip = clipCompleteCourse;
                        audioSource.PlayDelayed(0.0f);
                        enteredHoop3 = true;
                        hangar.SendMessage("updateScore");
                        droneReset(drone.gameObject);

                        
                        StartCoroutine(delaySetActive());

                       
                        
                           

                    }
                    else { }
                }

            }
        }
    
       
    }

    //check that drone is one of the colliders in given list
    //returns true or false
    bool checkDrone(List<Collider> currentTriggers)
    {
        return currentTriggers.Contains(drone);
    }

    //reset function for the drone
    //set the drone intial position and orientation
    private void droneReset(GameObject droneGameObj)
    {
        droneGameObj.transform.position = drone_start_position;
        droneGameObj.transform.rotation = drone_start_rotation;
        droneGameObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        droneGameObj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void playAudio()
    {
        
    }

    IEnumerator delaySetActive()
    {
        
        yield return new WaitForSeconds(2);

       
            gameObjParent.SetActive(false);
            nextStage.SetActive(true);

       
    }

}
