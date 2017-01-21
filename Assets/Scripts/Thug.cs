using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : MonoBehaviour {

    private float speed = 0.25f;
    private string state = "moving";
    
    private const float HERO_POSITION = -6.25f;
    
    private void Update() {
        if(this.state == "moving") {
            this.Move();
        } else if(this.state == "attacking") {
            this.Attack();
        }
    }

    private void Move() {
        // Normalize the delta.
        float delta = Time.deltaTime / (1f / 60f);
        
        // Calculate the distance to move.
        Vector2 movement = new Vector2(-1 * this.speed * delta, 0f);
        
        // Execute the movement.
        this.transform.Translate(movement);
        
        if(this.transform.position.x <= HERO_POSITION) {
            // this.state = "attacking";
            this.Die();
        }
    }
    
    private void Attack() {
        if(Input.GetButtonDown("Action")) {
            Debug.Log("!!");
        }
    }
    
    private void Die() {
        GameObject nextThug = Object.Instantiate(this.gameObject, new Vector2(12f, 1.75f), Quaternion.identity);
        nextThug.name = "Thug" + Random.Range(100, 999);
        
        Object.Destroy(this.gameObject);
    }
}
