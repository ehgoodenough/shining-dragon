using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thug : MonoBehaviour {
    public AudioClip attackSFX;

    private AudioSource source;

    private float speed = 0.05f;
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

	private float heroBufferXWidth = 0.4f;

	private void Start() {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		this.hero = manager.getHero();

		attackDistance = 1.4f;
		maxSweetSpot = 0.7f;
		minSweetSpot = 0.6f;

		sweetSpot = hero.transform.position.x + attackDistance - 0.4f;
		sweetSpotIndicator = Instantiate (SweetSpotIndicator, new Vector3(sweetSpot, -0.78f, 0), Quaternion.identity);
		sweetSpotIndicator.transform.parent = this.transform;

        source = GetComponent<AudioSource>();
    }

    private void Update() {
        if(this.state == "moving") {
            this.Moving();
        } else if(this.state == "attacking") {
            this.Attacking();
        }
		sweetSpot = hero.transform.position.x + attackDistance - 0.4f;
		float sweetSpotDiff = sweetSpot - sweetSpotIndicator.transform.position.x;
		sweetSpotIndicator.transform.Translate (new Vector3(sweetSpotDiff, 0, 0));
    }

    private void Moving() {
        // Normalize the delta.
        float delta = Time.deltaTime / (1f / 60f);
        
		float distanceFromHero = this.transform.position.x - hero.transform.position.x;

        // Calculate the distance to move.
        Vector2 movement = new Vector2(-1 * this.speed * delta, 0f);
        
        // Execute the movement.
        this.transform.Translate(movement);

		if(distanceFromHero < (this.speed*40)){
			this.hero.beginPreemptivePunch();
		}

		if (distanceFromHero > attackDistance || distanceFromHero < minSweetSpot) {
			stunPower = 0;
			Debug.Log (stunPower);

		} else if (distanceFromHero < maxSweetSpot && distanceFromHero > minSweetSpot) {
			stunPower = 1;
			Debug.Log (stunPower);

		} else {
			stunPower = 1 / ((distanceFromHero - (maxSweetSpot - 1))*(distanceFromHero - (maxSweetSpot - 1)));
			Debug.Log (stunPower);

		}

		//If we're close enough to the hero to jump
		if(distanceFromHero <= attackDistance) {
			if (Input.GetKeyDown (KeyCode.Space) && !((this.transform.position.x + movement.x - heroBufferXWidth) <= hero.transform.position.x)) {
				//Debug.Log ("got here");
				//Should tween nicely when we have time
				this.transform.position = new Vector2 (this.hero.transform.position.x + heroBufferXWidth, this.transform.position.y);
				this.state = "attacking";
				//Debug.Log (stunPower);
				this.hero.beStunned (stunPower);
			}

			//If the thug reaches the hero without pressing space
			if ((this.transform.position.x + movement.x - heroBufferXWidth) <= hero.transform.position.x) {
				this.transform.position = new Vector2 (this.hero.transform.position.x + heroBufferXWidth, this.transform.position.y);
				if (this.state != "attacking") {
					float tint = 0.0f;
					this.GetComponent<SpriteRenderer> ().color = new Color (tint, tint, tint, 1);
				}
			}
        }
        
        Vector3 cameraMovement = new Vector3(0f, 0f, 0f);
        cameraMovement.x = (this.transform.position.x - 1f - Camera.main.transform.position.x) / 16;
        Camera.main.transform.Translate(cameraMovement);
    }

    private void Attacking() {

        if(Input.GetButtonDown("Action")) {
            this.hero.beAttacked();
            source.PlayOneShot(attackSFX);
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
