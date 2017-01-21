using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	//Declare prefabs (for assigning in inspector)
	public GameObject Hero;
	public GameObject Thug;

	//Declare the hero in our scene
	private Thug thug;
	private Hero hero;
    
	void Awake () {
		hero = this.createHero ();
		thug = this.createThug();
	}

	public void handleThugDeath() {
		thug = this.createThug();
	}

	public Hero getHero() {
		return hero;
	}

	public Thug getThug() {
		return thug;
	}

	private Thug createThug() {
		GameObject nextThug = Object.Instantiate(Thug, new Vector2(12f, 1.75f), Quaternion.identity);
		nextThug.name = "Thug" + Random.Range(100, 999);
		return nextThug.GetComponent<Thug>();
	}

	private Hero createHero(){
		GameObject heroObject = Instantiate(Hero, new Vector2(-7.5f, 1.75f), Quaternion.identity);
		return heroObject.GetComponent<Hero>();
	}
}
