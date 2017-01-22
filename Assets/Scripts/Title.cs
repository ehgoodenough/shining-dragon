using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {

	private float timer = 0f;
    
    private SpriteRenderer render;
    
    private bool gameHasStarted = false;
    
    void Start() {
        this.render = GetComponent<SpriteRenderer>();
    }
	
	void Update() {
        if(gameHasStarted == false) {
		    gameHasStarted = GameObject.Find("SceneManager").GetComponent<SceneManager>().gameHasStarted;
        } else {
            this.render.color = new Color(1, 1, 1, this.render.color.a - 0.1f);
        }
	}
}
