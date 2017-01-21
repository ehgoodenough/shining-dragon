using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    
    private int maxhealth = 100;
    private int health = 100;
    
	private string state = "unset";
	private SceneManager manager;

	void Start () {
		this.manager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
	}
    
	void Update() {
		// ..?!
	}
    
    public void beAttacked() {
        health -= 1;
        
        GameObject.Find("HealthBar").GetComponent<HealthBar>().UpdateWidth(this.health, this.maxhealth);
    }

	public void punch() {
		
	}
}
