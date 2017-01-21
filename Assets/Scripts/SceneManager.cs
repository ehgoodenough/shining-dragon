using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	// Declare prefabs (for assigning in inspector)
	public GameObject Hero;
	public GameObject Thug;

	// Declare the entities in our scene
	private Thug thug;
	private Hero hero;
    
	void Awake () {
		this.createHero();
		this.createThug();
	}

	public Hero getHero() {
		return hero;
	}

	public Thug getThug() {
		return thug;
	}

	public Thug createThug() {
		this.thug = Object.Instantiate(Thug, new Vector2(12f, 1.75f), Quaternion.identity).GetComponent<Thug>();
		this.thug.gameObject.name = "Thug" + Random.Range(100, 999);
		return this.thug;
	}

	public Hero createHero() {
		this.hero = Instantiate(Hero, new Vector2(-7.5f, 1.75f), Quaternion.identity).GetComponent<Hero>();
        this.hero.gameObject.name = "Hero";
		return this.hero;
	}

	public void destroyThug() {
        Object.Destroy(this.thug.gameObject);
	}
}
