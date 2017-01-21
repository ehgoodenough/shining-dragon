using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thug : MonoBehaviour {

    private float speed = 0.25f;
    private string state = "moving";
    private int hits = 0;
    
    private const float HERO_POSITION = -6.25f;

	private SceneManager manager;
    
	private void Start() {
		manager = this.transform.parent.GetComponent<SceneManager>();
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
        
        if(this.transform.position.x <= HERO_POSITION) {
            this.state = "attacking";
        }
    }
    
    private void Attacking() {
        if(Input.GetButtonDown("Action")) {
            GameObject.Find("Hero").GetComponent<Hero>().beAttacked();
            hits += 1;
            if(hits >= 3) {
                this.Die();
            }
        }
    }
    
    private void Die() {
		manager.handleThugDeath ();
        
        Object.Destroy(this.gameObject);
    }
}
