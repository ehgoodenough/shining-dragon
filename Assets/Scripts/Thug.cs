using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thug : MonoBehaviour {

    private float speed = 0.25f;
    private const float HERO_POSITION = -4.5f;
    
    private void Update() {
        this.Move();
    }

    private void Move() {
        // Normalize the delta.
        float delta = Time.deltaTime / (1f / 60f);
        
        // Calculate the distance to move.
        Vector2 movement = new Vector2(-1 * this.speed * delta, 0f);
        
        // Execute the movement.
        this.transform.Translate(movement);
        
        if(this.transform.position.x <= HERO_POSITION) {
            this.Die();
        }
    }
    
    private void Die() {
        GameObject nextThug = Object.Instantiate(this.gameObject, new Vector2(12f, 1.75f), Quaternion.identity);
        nextThug.name = "Thug" + Random.Range(100, 999);
        
        Object.Destroy(this.gameObject);
    }
}
