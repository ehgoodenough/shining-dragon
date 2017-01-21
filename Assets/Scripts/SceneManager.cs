using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	//Declare prefabs (for assigning in inspector)
	public GameObject Hero;
	public GameObject Thug;
	private ArrayList thugs;

	//Declare the hero in our scene
	private Hero hero;

	// Use this for initialization
	void Awake () {
		thugs = new ArrayList ();

		hero = this.createHero ();
		this.createNewThug ();

		hero.transform.parent = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void handleThugDeath(){
		this.createNewThug ();
	}

	public Hero getHero(){
		return hero;
	}

	public ArrayList getThugs(){
		return thugs;
	}

	private GameObject createNewThug(){
		GameObject nextThug = Object.Instantiate(Thug, new Vector2(12f, 1.75f), Quaternion.identity);
		nextThug.name = "Thug" + Random.Range(100, 999);
		thugs.Add (nextThug);
		return nextThug;
	}

	private Hero createHero(){
		GameObject heroObject = Instantiate(Hero, new Vector2(-7.5f, 1.75f), Quaternion.identity);
		heroObject.transform.parent = this.transform;
		return heroObject.GetComponent<Hero>();
	}
}
