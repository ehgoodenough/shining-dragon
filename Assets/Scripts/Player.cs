using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private float speed = 0.25f;
    private Rigidbody2D body;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Move();
    }

    private void Move() {
        float delta = Time.deltaTime / (1f / 60f);

        Vector2 movement = new Vector2(0f, 0f);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

		body.position += movement * speed * delta;
    }
}
