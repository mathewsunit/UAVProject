using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course2 : MonoBehaviour
{

    public AudioClip audioClip;
    AudioSource audioSource;

    //Affect for path colliders
    public Affect path1;
    public Affect path2;
    public Affect path3;
    public Affect path4;

    //list of colliders colliding with the path stages
    List<Collider> currentTriggers1;
    List<Collider> currentTriggers2;
    List<Collider> currentTriggers3;
    List<Collider> currentTriggers4;

    //List of GameObjects
    List<GameObject> gs = new List<GameObject>();

    //Collider on drone
    public Collider drone;

    //Materials to set
    public Material transparent;
    public Material defaultMaterial;

    //int to control blinking
    int blinkCheck;

    //boolean to flip the direction
    bool flipDirection;

    // Use this for initialization
    void Start()
    {
        //get transform array of all children for current GameObject
        Transform[] ts = this.GetComponentsInChildren<Transform>();

        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.01f;
        audioSource.playOnAwake = false;
        if(audioSource.clip != audioClip)
        {
            audioSource.clip = audioClip;
        }

        //if not null
        if (ts == null)
        {
        }
        foreach (Transform t in ts)
        {
            //transfer into list of GameObjects, includes the parent and children of this
            if (t != null && t.gameObject != null)
            {
                gs.Add(t.gameObject);
            }

        }

        //intialize direction to false
        flipDirection = false;

        //remove test stage and this, so it only consists of the children stages
        gs.Remove(this.gameObject);
        gs.Remove(GameObject.Find("stagetest"));

        //initialize blinkCheck to 70
        blinkCheck = 70;
       
    }

    // Update is called once per frame
    void Update()
    {

        //if blinkCheck reaches 0, reset to 70
        if(blinkCheck == 0) {
            blinkCheck = 70;
        }
        //otherwise decrement
        blinkCheck--;

        //updates collider lists
        currentTriggers1 = path1.ongoingTriggers;
        currentTriggers2 = path2.ongoingTriggers;
        currentTriggers3 = path3.ongoingTriggers;
        currentTriggers4 = path4.ongoingTriggers;
  
        //if drone is colliding with path stage 4
        if (path4.triggerOngoing && checkDrone(currentTriggers4))
        {
            audioSource.Stop();
            //flip the directions
            flipDirection = false;
            foreach (GameObject gobj in gs)
            {
                //make all other stages transparent, and current stage green
                if (!gobj.name.Equals("st1_4"))
                {
                    gobj.GetComponent<Renderer>().material = transparent;
                }
                else
                {
                    gobj.GetComponent<Renderer>().material = defaultMaterial;
                    gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
                }
            }
        }
        //if drone is colliding with path stage 3
        else if (path3.triggerOngoing && checkDrone(currentTriggers3))
        {
            audioSource.Stop();
            //flip the directions
            flipDirection = true;
            foreach (GameObject gobj in gs)
            {
                //make all other stages transparent, and current stage green
                if (!gobj.name.Equals("st1_3"))
                {
                    gobj.GetComponent<Renderer>().material = transparent;
                }
                else
                {
                    gobj.GetComponent<Renderer>().material = defaultMaterial;
                    gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
                }
            }
        }
        //if drone is colliding with path stage 3
        else if (path2.triggerOngoing && checkDrone(currentTriggers2))
        {
            foreach (GameObject gobj in gs)
            {
                audioSource.Stop();
                //if current stage, make it green
                //make next stage to travel to, blink yellow
                //makes rest of the stages transparent
                if (!gobj.name.Equals("st1_2"))
                {
                    if(gobj.name.Equals("st1_3")  && blinkCheck > 35 && blinkCheck <=70 && !flipDirection)
                    {
                        
                            gobj.GetComponent<Renderer>().material = defaultMaterial;
                            gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.yellow);
                            
                        
                       
                    }
                    else if (gobj.name.Equals("st1_4") && blinkCheck > 35 && blinkCheck <= 70 && flipDirection)
                    {

                        gobj.GetComponent<Renderer>().material = defaultMaterial;
                        gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.yellow);



                    }
                    else {
                        
                        gobj.GetComponent<Renderer>().material = transparent;
                    }
                   
                }
                else
                {
                    gobj.GetComponent<Renderer>().material = defaultMaterial;
                    gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
                }
            }
        }

        //if drone is colliding with path stage 1
        else if (path1.triggerOngoing && checkDrone(currentTriggers1))
        {
            audioSource.Stop();
            foreach (GameObject gobj in gs)
            {
                //make all other stages transparent, and current stage green
                if (!gobj.name.Equals("st1_1"))
                {
                    gobj.GetComponent<Renderer>().material = transparent;
                }

                else
                {
                    gobj.GetComponent<Renderer>().material = defaultMaterial;
                    gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
                }
            }
        }

        //if out of bounds, make all stages red
        else
        {
            if(!audioSource.isPlaying)
            audioSource.PlayDelayed(0.0f);
            
            foreach (GameObject gobj in gs)
            {
                
                gobj.GetComponent<Renderer>().material = defaultMaterial;
                gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
            }

        }





    }


    bool checkDrone(List<Collider> currentTriggers)
    {
        return currentTriggers.Contains(drone);
    }

}
