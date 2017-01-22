using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thug : MonoBehaviour {

    private float speed = 0.25f;
    private string state = "moving";
    
    private float attackDistance;
    
	private SceneManager manager;
	private Hero hero;
    
	private float sweetSpot;

	public GameObject SweetSpotIndicator;
	private GameObject sweetSpotIndicator;

	private float maxSweetSpot;
	private float minSweetSpot;

	private float stunPower = 0;

	private void Start() {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		this.hero = manager.getHero();

		attackDistance = 7.0f;
		maxSweetSpot = 3.4f;
		minSweetSpot = 3.0f;

		sweetSpot = hero.transform.position.x + attackDistance - 2;
		sweetSpotIndicator = Instantiate (SweetSpotIndicator, new Vector3(sweetSpot, 2, 0), Quaternion.identity);
		sweetSpotIndicator.transform.parent = this.transform;

        // set above the ground
        Vector3 position = transform.position;
        position.y = 2f;
        transform.position = position;
    }

    private void Update() {
        if(this.state == "moving") {
            this.Moving();
        } else if(this.state == "attacking") {
            this.Attacking();
        }
		sweetSpot = hero.transform.position.x + attackDistance - 2;
		float sweetSpotDiff = sweetSpot - sweetSpotIndicator.transform.position.x;
		sweetSpotIndicator.transform.Translate (new Vector3(sweetSpotDiff, 0, 0));
    }

    private void Moving() {
        // Normalize the delta.
        float delta = Time.deltaTime / (1f / 60f);
        
		float distanceFromSweetSpot = this.transform.position.x - sweetSpot;
		float distanceFromHero = this.transform.position.x - hero.transform.position.x;

        // Calculate the distance to move.
        Vector2 movement = new Vector2(-1 * this.speed * delta, 0f);
        
        // Execute the movement.
        this.transform.Translate(movement);

		if (distanceFromHero > attackDistance || distanceFromHero < minSweetSpot) {
			stunPower = 0;
		} else if (distanceFromHero < maxSweetSpot && distanceFromHero > minSweetSpot) {
			stunPower = 1;
		} else {
			stunPower = 1 / ((distanceFromHero - (maxSweetSpot - 1))*(distanceFromHero - (maxSweetSpot - 1)));
		}

		//If we're close enough to the hero to jump
		if(distanceFromHero <= attackDistance) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("got here");
				//Should tween nicely when we have time
				this.transform.position = new Vector2 (this.hero.transform.position.x + this.transform.localScale.x, this.transform.position.y);
				this.state = "attacking";
				Debug.Log (stunPower);
				this.hero.beStunned (stunPower);
			}

			//If the thug reaches the hero without pressing space
			if ((this.transform.position.x + movement.x - this.transform.localScale.x) <= hero.transform.position.x) {
				this.transform.position = new Vector2 (this.hero.transform.position.x + this.transform.localScale.x, this.transform.position.y);
				this.state = "attacking";
				Debug.Log (stunPower);

				this.hero.beStunned (stunPower);
			}
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
