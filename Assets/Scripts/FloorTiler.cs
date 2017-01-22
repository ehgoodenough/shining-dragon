using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTiler : MonoBehaviour {

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();

        var spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

        var childPrefab = new GameObject();

        var childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childSprite.transform.position = transform.position;
        childSprite.sprite = sprite.sprite;
        childSprite.transform.localScale = new Vector3(1, 1);
        childSprite.sortingOrder = sprite.sortingOrder;
        childSprite.sortingLayerID = sprite.sortingLayerID;
        childSprite.color = sprite.color;

        GameObject child;

       // Debug.Log($"Have a sprite with bounds {sprite.bounds.size.x} x {sprite.bounds.size.y} located at {transform.position.x}, {transform.position.y} with a scale of {transform.localScale.x} x {transform.localScale.y}");
       // Debug.Log($"Effectively, the sprite is meant to go from x={transform.position.x - (sprite.bounds.size.x / 2)} to x={transform.position.x + (sprite.bounds.size.x / 2)}");

        int numberOfRepeatedTextures = (int)(sprite.bounds.size.x / spriteSize.x);
        //Debug.Log($"Plan is to replace this with {numberOfRepeatedTextures} textures that are {spriteSize.x} x {spriteSize.y}");
        Vector3 usingCenterBaseFix = new Vector3(spriteSize.x / 2, 0, 0);
        var leftEdge = transform.position.x - (sprite.bounds.size.x / 2);
        var widthPer = spriteSize.x;
        
        for (int currentHorizontalIndex = 0; currentHorizontalIndex < numberOfRepeatedTextures; currentHorizontalIndex++)
        {
            child = Instantiate(childPrefab) as GameObject;
            child.transform.position = new Vector3(leftEdge + currentHorizontalIndex * widthPer, transform.position.y, transform.position.z) + usingCenterBaseFix;
            child.transform.parent = transform;
            
            //Debug.Log($"Placed one at {child.transform.position.x}, {child.transform.position.y}; this effectively goes from x={child.transform.position.x - (spriteSize.x / 2)} to x={child.transform.position.x + (spriteSize.x / 2)}");
        }

        Destroy(childPrefab);
        sprite.enabled = false;
    }
    
    void Update () {
		
	}
}
