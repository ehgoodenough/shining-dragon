using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    
    private int maxhealth = 100;
    private int health = 100;
	
	void Update() {
		// ..?!
	}
    
    public void beAttacked() {
        health -= 1;
        
        // GameObject.Find("Health").GetComponent<Text>().text = "HP: " + this.health;
        GameObject.Find("HealthBar").GetComponent<HealthBar>().UpdateWidth(this.health, this.maxhealth);
    }
}
