using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thug : MonoBehaviour {
    public AudioClip[] attackSFXList = new AudioClip[3];
    public AudioClip[] runSFX = new AudioClip[2];
    public AudioClip screamSFX;
    public AudioClip stunAttackSFX;
    public AudioClip hitGroundSFX;
    public AudioClip[] flySFXList = new AudioClip[5];
    private int flySFX = 0;

    private AudioSource source;
    private Animator animator;
    private Rigidbody2D body;
    private SpriteRenderer render;

    private float speed = 0.05f;
    private string state = "moving";
    
    private float attackDistance;
    
	private SceneManager manager;
	private Hero hero;
    
	private float sweetSpot;

	public GameObject SweetSpotIndicator;
	private GameObject sweetSpotIndicator;
	private GameObject sweetSpotIndicatorForeground;

	public GameObject NiceMessage;
	private GameObject niceMessage;

	public GameObject GoodMessage;
	private GameObject goodMessage;

	public GameObject MehMessage;
	private GameObject mehMessage;

	private float maxSweetSpot;
	private float minSweetSpot;

	private float stunPower = 0;

	private float heroBufferXWidth = 0.3f;
    
    private float timeSinceDead = 0f;
	private float timeSinceAlive = 0f;

    private float combo = 0;
    private const float PITCH_CHANGE = 0.3f;

    private int attackSfx = 0;
    

	private void Start() {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
		this.hero = manager.getHero();
        attackSfx = Random.Range(0, attackSFXList.Length);
        flySFX = Random.Range(0, flySFXList.Length);

        attackDistance = 1.4f;
		maxSweetSpot = 0.7f;
		minSweetSpot = 0.6f;

		sweetSpot = hero.transform.position.x + attackDistance - 0.4f;
		sweetSpotIndicator = Instantiate (SweetSpotIndicator, new Vector3(sweetSpot, -0.45f, 0), Quaternion.identity);
		//sweetSpotIndicator = Instantiate (SweetSpotIndicator, new Vector3(sweetSpot, -0.795f, 0), Quaternion.identity);
		sweetSpotIndicator.transform.parent = this.transform;
		sweetSpotIndicatorForeground = sweetSpotIndicator.transform.FindChild ("SweetSpotIndicatorForeground").gameObject;

        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

		sweetSpotIndicator.SetActive (false);
        render = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if(this.state == "moving") {
            this.Moving();
			timeSinceAlive += Time.deltaTime;
        } else if(this.state == "attacking") {
            this.Attacking();
			timeSinceAlive += Time.deltaTime;
        } else if(this.state == "dead") {
            timeSinceDead += Time.deltaTime;
            if(timeSinceDead > 0.35) {
				manager.createThug(this.stunPower);
                this.state = "very dead"; 
            }
            return;
        } else if(this.state == "very dead") {
            return;
        } else if(this.state == "victory!") {
            return;
        }
        
		sweetSpot = hero.transform.position.x + attackDistance - 0.4f;
		float sweetSpotDiff = sweetSpot - sweetSpotIndicator.transform.position.x;
		sweetSpotIndicator.transform.Translate (new Vector3(sweetSpotDiff, 0, 0));

		if (timeSinceAlive > 0.6f) {
			sweetSpotIndicator.SetActive (true);
		}
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

		if (distanceFromHero < minSweetSpot) {
			stunPower = 0;
			//Debug.Log (stunPower);

		} else if (distanceFromHero < maxSweetSpot && distanceFromHero > minSweetSpot) {
			stunPower = 1f;
			//Debug.Log (stunPower);

		} else {
			stunPower = 1 / ((distanceFromHero - (maxSweetSpot - 1))*(distanceFromHero - (maxSweetSpot - 1)));
			//Debug.Log (stunPower);
		}

		if (stunPower > 0.9f) {
			stunPower = 0.9f;
		}

		float currentXPos =  sweetSpotIndicatorForeground.transform.position.x;
		float xPosTarget = sweetSpotIndicator.transform.position.x - (stunPower*0.4f);
		sweetSpotIndicatorForeground.transform.localScale = new Vector3(1-stunPower, 1, 1);
		sweetSpotIndicatorForeground.transform.Translate ((xPosTarget - currentXPos), 0, 0);

		//If we're close enough to the hero to jump
		if(distanceFromHero <= attackDistance) {
            if(manager.hasCompletedTutorial == false) {
                Time.timeScale = 0.1f;
                manager.tutorialMessage.text = "Hit SPACEBAR to attack!!";
            }
			if (distanceFromHero <= minSweetSpot) {
				if (this.state != "attacking") {
					sweetSpotIndicatorForeground.GetComponent<SpriteRenderer> ().color = new Color (1f, 0.2f, 0.2f, 1);
				}
			}

			if (Input.GetKeyDown (KeyCode.Space) && !((this.transform.position.x + movement.x - heroBufferXWidth) <= hero.transform.position.x)) {
				//Debug.Log ("got here");
				//Should tween nicely when we have time
				this.transform.position = new Vector2 (this.hero.transform.position.x + heroBufferXWidth, this.transform.position.y);
                this.state = "attacking";
                animator.Play("AttackIdle");
				//Debug.Log (stunPower);
				this.hero.beStunned (stunPower);
				//stunPower = 0;

				if (stunPower > 0.85f) {

					niceMessage = Instantiate (NiceMessage, sweetSpotIndicator.transform.position + new Vector3 (-0.55f, 0.3f, 0), Quaternion.identity);
					niceMessage.transform.parent = sweetSpotIndicator.transform;
				} else if (stunPower > 0.6) {
					goodMessage = Instantiate (GoodMessage, sweetSpotIndicator.transform.position + new Vector3 (-0.55f, 0.3f, 0), Quaternion.identity);
					goodMessage.transform.parent = sweetSpotIndicator.transform;
				} else {
					mehMessage = Instantiate (MehMessage, sweetSpotIndicator.transform.position + new Vector3 (-0.55f, 0.3f, 0), Quaternion.identity);
					mehMessage.transform.parent = sweetSpotIndicator.transform;
				}
                
                Time.timeScale = 1f;
                
                if(manager.hasCompletedTutorial == false) {
                    manager.tutorialMessage.text = "Now button-mash the SPACEBAR!!";
                }
			}

			if (distanceFromHero <= minSweetSpot) {
				if (this.state != "attacking") {
					
                    if(manager.hasCompletedTutorial == false) {
                        manager.tutorialMessage.text = "Oh no!! Too late :(";
                    }
                    
					sweetSpotIndicatorForeground.GetComponent<SpriteRenderer> ().color = new Color (1f, 0.2f, 0.2f, 1);
                    render.color = new Color(0.5f, 0.5f, 0.5f, 1f);
				}
			}

			//If the thug reaches the hero without pressing space
			if ((this.transform.position.x + movement.x - heroBufferXWidth) <= hero.transform.position.x) {
				this.transform.position = new Vector2 (this.hero.transform.position.x + heroBufferXWidth, this.transform.position.y);
				if (this.state != "attacking") {
                    animator.Play("Death");
				}
			}
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(hitGroundSFX);
        print("collide");
    }

    private void Attacking() {

        if(Input.GetButtonDown("Action")) {
            this.hero.beAttacked();
            
            source.pitch = 1 + combo * PITCH_CHANGE;
            combo++;
            source.PlayOneShot(attackSFXList[attackSfx]);
            animator.Play("Attack");
            
            if(combo > 4) {
                if(manager.hasCompletedTutorial == false) {
                    manager.hasCompletedTutorial = true;
                    manager.tutorialMessage.text = "";
                }
            }
            
            manager.comboMessage.text = combo + (combo >= 10 ? "!" : "") + (combo >= 15 ? "!" : "");
            
            if(this.hero.health <= 0) {
                this.state = "victory!";
                
                // remove the indicator, cuz the hero is dead
                // and flying away and you don't need it anymore!!
                sweetSpotIndicator.SetActive(false);
                
                animator.Play("Victory");
            }
        }
    }
    
	public void tryToDie() {
		this.beAttacked();
	}

    private void beAttacked() {
        
        // Change the state.
        state = "dead";
        
        // Set the animation.
        animator.Play("Death");
        
		sweetSpotIndicator.SetActive (false);
        source.volume = 0.75f;
        source.pitch = 1f;
        source.PlayOneShot(flySFXList[flySFX]);
        

        // Send the thug flying!
        GetComponent<CapsuleCollider2D>().isTrigger = false;
        body = gameObject.AddComponent<Rigidbody2D>() as Rigidbody2D;
        body.isKinematic = false;
        body.AddForce(new Vector2(Random.Range(4f, 6f), 5f), ForceMode2D.Impulse);
        body.AddTorque(Random.Range(-0.5f, -1f), ForceMode2D.Impulse);
        
        render.sortingOrder -= 1;
        render.color = new Color(255, 255, 255, 1);
    }

	public void modifySpeed(float speedModifier) {
		this.speed = this.speed * 0.8f + (this.speed * 0.6f * speedModifier);
	}
}
