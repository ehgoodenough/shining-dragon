using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    
    private int maxhealth = 10;
    private int health = 10;
    
	private string state = "unset";
	private SceneManager manager;

	float timeSinceReady;
	float timeSincePunch;
	float timeSinceStunned;

	float readyDuration;
	float endlagDuration;
	float stunDuration;
	bool hasPunchedSinceStunned;

	void Start () {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		readyDuration = 0.5f;
		timeSinceReady = 0;
		state = "walking";
		endlagDuration = 0.5f;
		stunDuration = 1;
		timeSinceStunned = 0;
		hasPunchedSinceStunned = true;;
	}
    
	void Update() {
		timeSincePunch += Time.deltaTime;
		if (this.state == "stunned") {
			timeSinceStunned += Time.deltaTime;
			if (timeSinceStunned >= stunDuration) {
				this.state = "ready";
				timeSinceReady = 0;
			}
		}

		if (this.state == "ready") {
			timeSinceReady += Time.deltaTime;
			if (timeSinceReady >= readyDuration) {
				this.punch ();
			}
		}

		if (this.state == "endlag") {
			if (timeSincePunch >= endlagDuration) {
				this.state = "walking";
			}
		}

		if (this.state == "walking") {
			this.timeSincePunch += Time.deltaTime;
			this.transform.Translate (new Vector2 (0.01f, 0));
			timeSincePunch += Time.deltaTime;
		}

		//Debug.Log (this.state);
	}
    
	public void beStunned() {
		this.state = "stunned";
		this.timeSinceStunned = 0;
		hasPunchedSinceStunned = false;
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
		this.state = "endlag";
		Thug thug = manager.getThug ();
		thug.tryToDie ();
		timeSincePunch = 0;
	}
}
