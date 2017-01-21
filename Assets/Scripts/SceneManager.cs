using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
	public GameObject hero;
	public GameObject thug;

	// Use this for initialization
	void Awake () {
		GameObject firstThug = Object.Instantiate(thug, new Vector2(12f, 1.75f), Quaternion.identity);
		firstThug.transform.parent = this.transform;
		firstThug.name = "Thug" + Random.Range (100, 999);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void handleThugDeath(){
		GameObject nextThug = Object.Instantiate(thug, new Vector2(12f, 1.75f), Quaternion.identity);
		nextThug.transform.parent = this.transform;
		nextThug.name = "Thug" + Random.Range(100, 999);
	}
}
