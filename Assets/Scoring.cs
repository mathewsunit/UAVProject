using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour {
    public int score;
    public Text scoreDisplay;

	// Use this for initialization
	void Start () {
        //begin score with zero
        score = 0;

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void updateScore()
    {
     //   print("updateScore");
        score += 10;
        scoreDisplay.text = "Score: " + score.ToString();
        
    }
}
