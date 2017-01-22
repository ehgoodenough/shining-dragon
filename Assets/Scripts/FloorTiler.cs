using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTiler : MonoBehaviour {
    private List<GameObject> GeneratedSprites;

    public bool Debugging;
    public bool DebuggingFine;

    void Start()
    {
        GeneratedSprites = new List<GameObject>();
        RefreshGeneratedSprites();
    }
    
    void Update () {
		
	}
    
    GameObject InitChildPreFab(SpriteRenderer spriteBase)
    {
        var childPrefab = new GameObject();
        var childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childSprite.transform.position = transform.position;
        childSprite.sprite = spriteBase.sprite;
        childSprite.transform.localScale = new Vector3(1, 1);
        childSprite.sortingOrder = spriteBase.sortingOrder;
        childSprite.sortingLayerID = spriteBase.sortingLayerID;
        childSprite.color = spriteBase.color;
        childSprite.name = "Tiled Sprite";
        return childPrefab;
    }
    

    public void RefreshGeneratedSprites()
    {
        var sprite = GetComponent<SpriteRenderer>();

        var spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);
        if(Debugging)
            Debug.Log($"spriteSize = {spriteSize.x} x {spriteSize.y}; sprite bounds = {sprite.bounds.size.x} x {sprite.bounds.size.y} with a transform of {transform.localScale.x} x {transform.localScale.y}");
        int numberOfRepeatedTextures = (int)(sprite.bounds.size.x / spriteSize.x);

        Vector3 usingCenterBaseFix = new Vector3(spriteSize.x / 2, 0, 0);
        var leftEdge = transform.position.x - (sprite.bounds.size.x / 2);
        var widthPer = spriteSize.x;

        int currentHorizontalIndex = 0;
        
        GameObject childPrefab = null;
        if (Debugging)
        {
            Debug.Log($"Have a sprite with bounds {sprite.bounds.size.x} x {sprite.bounds.size.y} located at {transform.position.x}, {transform.position.y} with a scale of {transform.localScale.x} x {transform.localScale.y}");
            Debug.Log($"Effectively, the sprite is meant to go from x={transform.position.x - (sprite.bounds.size.x / 2)} to x={transform.position.x + (sprite.bounds.size.x / 2)}");
            Debug.Log($"Plan is to replace this with {numberOfRepeatedTextures} textures that are {spriteSize.x} x {spriteSize.y}");
        }
        for (; currentHorizontalIndex < numberOfRepeatedTextures; currentHorizontalIndex++)
        {
            GameObject child;
            if(currentHorizontalIndex < GeneratedSprites.Count)
            {
                child = GeneratedSprites[currentHorizontalIndex];
            }else
            {
                if (childPrefab == null)
                {
                    childPrefab = InitChildPreFab(sprite);
                }

                child = Instantiate(childPrefab) as GameObject;
                child.transform.SetParent(transform);
                child.transform.localPosition = new Vector3(0, 0, 0);
                GeneratedSprites.Add(child);
            }
            
            child.transform.position = new Vector3(leftEdge + currentHorizontalIndex * widthPer, transform.position.y, transform.position.z) + usingCenterBaseFix;
            
            if(DebuggingFine)
                Debug.Log($"Placed one at {child.transform.position.x}, {child.transform.position.y}; this effectively goes from x={child.transform.position.x - (spriteSize.x / 2)} to x={child.transform.position.x + (spriteSize.x / 2)}");

        }

        for(var counter = GeneratedSprites.Count - 1; counter >= currentHorizontalIndex; counter--)
        {
            var child = GeneratedSprites[counter];
            GeneratedSprites.RemoveAt(counter);
            Destroy(child);
        }
        
        if(childPrefab != null)
            Destroy(childPrefab);
        sprite.enabled = false;
    }
}
