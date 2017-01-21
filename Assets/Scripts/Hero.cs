using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    
    private int maxhealth = 100;
    private int health = 100;
    
	private string state = "unset";
	private SceneManager manager;

	float timeBetweenPunches;
	float timeSincePunch;

	void Start () {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		timeBetweenPunches = 3;
		timeSincePunch = 0;
	}
    
	void Update() {
		// ..?!
		timeSincePunch += Time.deltaTime;
		if (timeSincePunch > timeBetweenPunches) {
			
			this.punch ();
		}
	}
    
    public void beAttacked() {
        health -= 1;
        
        GameObject.Find("HealthBar").GetComponent<HealthBar>().UpdateWidth(this.health, this.maxhealth);
    }

	public void punch() {
		Thug thug = manager.getThug ();
		thug.tryToDie ();
		timeSincePunch = 0;
		Debug.Log ("punching");
	}
}
