using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thug : MonoBehaviour {

    private float speed = 0.25f;
    private string state = "moving";
    
	private SceneManager manager;
	private Hero hero;
    
	private void Start() {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		this.hero = manager.getHero();
	}

    private void Update() {
        if(this.state == "moving") {
            this.Moving();
        } else if(this.state == "attacking") {
            this.Attacking();
        }
    }

    private void Moving() {
        // Normalize the delta.
        float delta = Time.deltaTime / (1f / 60f);
        
        // Calculate the distance to move.
        Vector2 movement = new Vector2(-1 * this.speed * delta, 0f);
        
        // Execute the movement.
        this.transform.Translate(movement);
        
        if(this.transform.position.x <= this.hero.transform.position.x + this.hero.transform.localScale.x) {
            this.state = "attacking";
        }
    }
    
    private void Attacking() {
        if(Input.GetButtonDown("Action")) {
            this.hero.beAttacked();
        }
    }
    
	public void tryToDie() {
		this.beAttacked();
	}

    private void beAttacked() {
        // The thugs are so weak, they
        // are killed in just in one hit!!
		manager.destroyThug();
        
        // Create a new thug to
        // replace this thug.
        manager.createThug();
        
        // TODO: Don't destroy the thug, but instead
        // send it flying and sprawling in a heep.
    }
}
