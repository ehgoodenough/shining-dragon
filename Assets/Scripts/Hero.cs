using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    public AudioClip attackSFX;
    
    private int maxhealth = 50;
    private int health = 50;
    
	private string state = "unset";
	private SceneManager manager;
    private AudioSource source;

    private Animator animator;

    float timeSinceReady;
	float timeInEndlag;
	float timeSinceStunned;

	float readyDuration;
	float endlagDuration;
	float stunDuration;

	void Start () {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		readyDuration = 0.5f;
		timeSinceReady = 0;
		state = "walking";
		endlagDuration = 0.5f;
		stunDuration = 1;
		timeSinceStunned = 0;

        // rotate the character to face the enemy
        Vector3 vector = transform.localScale;
        vector.x *= -1;
        transform.localScale = vector;

        // set above the ground
        Vector3 position = transform.position;
        position.y = 2.15f;
        transform.position = position;

        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
	}
    
	void Update() {
        if(this.manager.gameHasEnded == true) {
            return;
        }
        
		if (this.state == "stunned") {
			timeSinceStunned += Time.deltaTime;
			if (timeSinceStunned >= stunDuration) {
				this.state = "ready";

                animator.Play("Attack");
                timeSinceReady = 0;
			}
			float tint = 1 - (stunDuration - timeSinceStunned)/stunDuration;
			this.GetComponent<SpriteRenderer> ().color = new Color (tint, tint, tint, 1);
		}

		if (this.state == "ready") {
			timeSinceReady += Time.deltaTime;
			if (timeSinceReady >= readyDuration) {
				this.punch ();
			}
			this.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
		}

		if (this.state == "endlag") {
			timeInEndlag += Time.deltaTime;

			if (timeInEndlag >= endlagDuration) {
				this.state = "walking";
			}
		}

		if (this.state == "walking") {
			this.transform.Translate(new Vector2 (0.01f, 0));
            
            if(this.transform.position.x >= 9)
            {
                manager.gameEnd(false);
                Debug.Log("You Lose!!");
            }
		}

		//Debug.Log (this.state);
	}
    
	public void beStunned(float stunPower) {
		this.stunDuration = stunPower;
		this.state = "stunned";
		this.timeSinceStunned = 0;
		animator.Play("Block");
	}

    public void beAttacked() {
        this.health -= 1;
        if(this.health <= 0) {
            this.health = 0;
            manager.gameEnd(true);
            Debug.Log("You Win!!");
        }
        
        if(this.manager.healthbar != null) {
            this.manager.healthbar.UpdateWidth(this.health, this.maxhealth);
        }
    }

	public void punch() {
        source.PlayOneShot(attackSFX);
		this.state = "endlag";
		Thug thug = manager.getThug ();
		thug.tryToDie ();
		timeInEndlag = 0;
	}
}
