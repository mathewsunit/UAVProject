using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entry2 : MonoBehaviour
{

    public AudioClip clipPassHoop;

    AudioSource audiosource;
    

    //boolean to indicate whether the drone has passed through given hoop
    public bool enteredHoop1;
    public bool enteredHoop2;

    //list of colliders colliding with the hoops
    List<Collider> currentTriggers;
    List<Collider> currentTriggers1;
    List<Collider> currentTriggers2;

    //Affect for hoop colliders
    public Affect pointCollider1;
    public Affect pointCollider2;

    Entry entryObj;

    //Affect for test stage which checks user has traveled to other hoop
    public Affect stagetest;

    //Collider on drone
    public Collider drone;

    public GameObject hangar;
    
    void Start()
    {
        //entryObj = new Entry();

        //runningscore = entryObj.score;


        //initialize booleans to false
        enteredHoop1 = false;
        enteredHoop2 = false;

        audiosource = this.gameObject.AddComponent<AudioSource>();
        audiosource.clip = clipPassHoop;
        
      //  scoreDisplay.text = "Score: " + score.ToString();
    }


    void Update()
    {
        //runningscore = entryObj.score;

        //updates collider lists
        currentTriggers = stagetest.ongoingTriggers;
        currentTriggers1 = pointCollider1.ongoingTriggers;
        currentTriggers2 = pointCollider2.ongoingTriggers;

        
        if (stagetest.triggerExited && checkDrone(currentTriggers))
        {
            enteredHoop1 = false;
            enteredHoop2 = false;
        }
       
        if ((pointCollider1.triggerExited && checkDrone(currentTriggers1)))
        {

            if (!enteredHoop1)
            {
                //begin score with score from previous stage
                audiosource.Play(0);
                enteredHoop1 = true;
                hangar.SendMessage("updateScore");
            }
            else {
              
               
            }
           
        }
        if ((pointCollider2.triggerExited && checkDrone(currentTriggers2)))
        {

            if (!enteredHoop2)
            {
                audiosource.Play(0);
                enteredHoop2 = true;
                hangar.SendMessage("updateScore");
            }
            else {
               
               
            }

        }
    }
    bool checkDrone(List<Collider> currentTriggers)
    {
        return currentTriggers.Contains(drone);
    }
   
}
