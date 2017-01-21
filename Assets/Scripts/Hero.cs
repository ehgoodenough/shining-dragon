using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    
    private int maxhealth = 10;
    private int health = 10;
    
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
		timeSincePunch += Time.deltaTime;
		if (timeSincePunch > timeBetweenPunches) {
			
			this.punch ();
		}
	}
    
    public void beAttacked() {
        this.health -= 1;
        if(this.health <= 0) {
            Debug.Log("You Win!!");
        }
        
        if(this.manager.healthbar != null) {
            this.manager.healthbar.UpdateWidth(this.health, this.maxhealth);
        }
    }

	public void punch() {
		Thug thug = manager.getThug ();
		thug.tryToDie ();
		timeSincePunch = 0;
		Debug.Log ("punching");
	}
}
