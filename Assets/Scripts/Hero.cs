using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    public AudioClip attackSFX;
    public AudioClip[] hitSFXList = new AudioClip[2];
    public AudioClip[] VOSFXList = new AudioClip[6];
    private int VOSFX;

    private int hitSFX;
    
    public int maxhealth = 50;
    public int health = 50;
    
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

    float moveBackToGo;

	void Start () {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		readyDuration = 0.6f;
		timeSinceReady = 0;
		state = "walking";
		endlagDuration = 0.5f;
		stunDuration = 1;
		timeSinceStunned = 0;
        hitSFX = Random.Range(0, hitSFXList.Length);

        // rotate the character to face the enemy
        Vector3 vector = transform.localScale;
        vector.x *= -1;
        transform.localScale = vector;

        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
	}
    
	void Update() {
        if(this.manager.gameHasEnded == true) {
            return;
        }
        
        if(moveBackToGo > 0.01f)
        {
            var amount = Mathf.Min(Time.deltaTime / 4, moveBackToGo);
            
            moveBackToGo -= amount;
            transform.position = new Vector3(transform.position.x - amount, transform.position.y, transform.position.z);
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
            
            if(this.transform.position.x >= 13)
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
        source.PlayOneShot(hitSFXList[hitSFX]);

        moveBackToGo = stunPower / 15;
	}

    public void beAttacked() {
        this.health -= 1;
        if(this.health <= 0) {
            this.health = 0;
            manager.gameEnd(true);
            
            Rigidbody2D body = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
            body.isKinematic = false;
            body.AddForce(new Vector2(-6f, 5f), ForceMode2D.Impulse);
            body.AddTorque(-1f, ForceMode2D.Impulse);
        }
        
        if(this.manager.healthbar != null) {
            this.manager.healthbar.UpdateWidth(this.health, this.maxhealth);
        }
    }

	public void beginPreemptivePunch() {
		if (this.state == "walking") {
			this.state = "ready";
			animator.Play("Attack");
			timeSinceReady = 0;
		}
	}

	public void punch() {
        source.PlayOneShot(attackSFX);
		this.state = "endlag";
		Thug thug = manager.getThug ();
		thug.tryToDie ();
		timeInEndlag = 0;
        
        Time.timeScale = 1f;
        manager.tutorialMessage.text = "";

        VOSFX = Random.Range(0, VOSFXList.Length);
        source.PlayOneShot(VOSFXList[VOSFX]);
	}
}
