using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	//Declare prefabs (for assigning in inspector)
	public GameObject Hero;
	public GameObject Thug;

	//Declare the hero in our scene
	private ArrayList thugs;
	private Hero hero;
    
	void Awake () {
		thugs = new ArrayList ();

		hero = this.createHero ();
		this.createThug();
	}

	public void handleThugDeath() {
		this.createThug();
	}

	public Hero getHero() {
		return hero;
	}

	public ArrayList getThugs() {
		return thugs;
	}

	private GameObject createThug() {
		GameObject nextThug = Object.Instantiate(Thug, new Vector2(12f, 1.75f), Quaternion.identity);
		nextThug.name = "Thug" + Random.Range(100, 999);
		thugs.Add(nextThug);
		return nextThug;
	}

	private Hero createHero(){
		GameObject heroObject = Instantiate(Hero, new Vector2(-7.5f, 1.75f), Quaternion.identity);
		return heroObject.GetComponent<Hero>();
	}
}
