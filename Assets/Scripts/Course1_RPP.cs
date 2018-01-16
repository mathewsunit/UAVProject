using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Course1_RPP : MonoBehaviour
{

    Entry entryobj;

    public Affect path1;
    public Affect path2;
    public Affect path3;
    public Affect path4;
    public Affect path5;
    public Affect path6;

    List<Collider> currentTriggers1;
    List<Collider> currentTriggers2;
    List<Collider> currentTriggers3;
    List<Collider> currentTriggers4;
    List<Collider> currentTriggers5;
    List<Collider> currentTriggers6;

    public GameObject Hoop1;
    public GameObject Hoop2;
    public GameObject Hoop3;


    List<GameObject> gs = new List<GameObject>();

    public Collider drone;

    public Material transparent;
    public Material defaultMaterial;

    // Use this for initialization
    void Start()
    {
        Transform[] ts = this.GetComponentsInChildren<Transform>();
        entryobj = new Entry();

        if (ts == null)
        {
        }
        foreach (Transform t in ts)
        {
            if (t != null && t.gameObject != null)
            {
                gs.Add(t.gameObject);
            }

        }
        Hoop1.SetActive(true);
        Hoop2.SetActive(false);
        Hoop3.SetActive(false);
        gs.Remove(this.gameObject);


    }

    // Update is called once per frame
    void Update()
    {
        currentTriggers1 = path1.ongoingTriggers;
        currentTriggers2 = path2.ongoingTriggers;
        currentTriggers3 = path3.ongoingTriggers;
        currentTriggers4 = path4.ongoingTriggers;
        currentTriggers5 = path5.ongoingTriggers;
        currentTriggers6 = path6.ongoingTriggers;

        if (path6.triggerOngoing && checkDrone(currentTriggers6))
        {
            Debug.Log("Press forward and throttle");

            setPathSettings("st_6", hoop3: true);
        }
        else if (path5.triggerOngoing && checkDrone(currentTriggers5))
        {
            Debug.Log("Press throttle upward");

            setPathSettings("st_5", hoop3: true);
        }
        else if (path4.triggerOngoing && checkDrone(currentTriggers4))
        {
            Debug.Log("Press forward and throttle");

            setPathSettings("st_4", hoop2: true);
        }
        else if (path3.triggerOngoing && checkDrone(currentTriggers3))
        {
            Debug.Log("Press throttle upward");

            setPathSettings("st_3", hoop2: true);
        }
        else if (path2.triggerOngoing && checkDrone(currentTriggers2))
        {
            Debug.Log("Press forward and throttle");

            setPathSettings("st_2", hoop1: true);
        }
        else if (path1.triggerOngoing && checkDrone(currentTriggers1))
        {
            Debug.Log("Press throttle upward");

            setPathSettings("st_1", hoop1: true);
        }


        else
        {
            if (entryobj.enteredHoop1)
            {
                Hoop1.SetActive(false);
                Hoop2.SetActive(true);
                Hoop3.SetActive(false);
            }
            else if (entryobj.enteredHoop2)
            {
                Hoop1.SetActive(false);
                Hoop2.SetActive(false);
                Hoop3.SetActive(true);
            }


            foreach (GameObject gobj in gs)
            {
                gobj.GetComponent<Renderer>().material = defaultMaterial;
                gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.red);
            }

        }





    }

    void setPathSettings(string stageName, bool hoop1 = false, bool hoop2 = false, bool hoop3 = false)
    {
        Hoop1.SetActive(hoop1);
        Hoop2.SetActive(hoop2);
        Hoop3.SetActive(hoop3);
        foreach (GameObject gobj in gs)
        {

            if (!gobj.name.Equals(stageName))
                gobj.GetComponent<Renderer>().material = transparent;
            else
            {
                gobj.GetComponent<Renderer>().material = defaultMaterial;
                gobj.GetComponent<Renderer>().material.SetColor("_TintColor", Color.green);
            }
        }
    }

    void enforeControllerInput()
    {

    }

    bool checkDrone(List<Collider> currentTriggers)
    {
        return currentTriggers.Contains(drone);
    }

}
