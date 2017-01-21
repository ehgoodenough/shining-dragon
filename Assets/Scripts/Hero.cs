using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    
    private float health = 100f;
	
	void Update() {
		// ..?!
	}
    
    public void beAttacked() {
        health -= 1;
        
        GameObject.Find("Health").GetComponent<Text>().text = "HP: " + this.health;
    }
}
